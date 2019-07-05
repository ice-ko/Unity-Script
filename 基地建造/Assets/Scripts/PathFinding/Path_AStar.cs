using Priority_Queue;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Path_AStar
{
    Queue<Tile> path;
    public Path_AStar(World world, Tile tileStart, Tile tileEnd)
    {
        //检查是否有效的图块图
        if (world.tileGraph == null)
        {
            world.tileGraph = new Path_TileGraph(world);
        }
        //所有有效的可步行节点的字典。
        Dictionary<Tile, Path_Node<Tile>> nodes = world.tileGraph.nodes;
        //确保我们的开始/结束节点位于节点列表中！
        if (nodes.ContainsKey(tileStart) == false)
        {
            Debug.LogError("Path_AStar：起始节点不在节点列表中！");
            return;
        }
        if (nodes.ContainsKey(tileEnd) == false)
        {
            Debug.LogError("Path_AStar：结束节点不在节点列表中！");
            return;
        }
       
        //开始节点
        Path_Node<Tile> start = nodes[tileStart];
        //目标节点
        Path_Node<Tile> goal = nodes[tileEnd];

        

        //主要遵循这个pseusocode：
        // https://en.wikipedia.org/wiki/A*_search_algorithm
        //关闭的tile
        List<Path_Node<Tile>> ClosedSet = new List<Path_Node<Tile>>();

        /*		List<Path_Node<Tile>> OpenSet = new List<Path_Node<Tile>>();
                OpenSet.Add( start );
        */
        //打开简单优先级队列
        SimplePriorityQueue<Path_Node<Tile>> OpenSet = new SimplePriorityQueue<Path_Node<Tile>>();
        OpenSet.Enqueue(start, 0);
        //路径节点
        Dictionary<Path_Node<Tile>, Path_Node<Tile>> Came_From = new Dictionary<Path_Node<Tile>, Path_Node<Tile>>();
        //每个节点的得分
        Dictionary<Path_Node<Tile>, float> g_score = new Dictionary<Path_Node<Tile>, float>();
        foreach (Path_Node<Tile> n in nodes.Values)
        {
            g_score[n] = Mathf.Infinity;
        }
        g_score[start] = 0;
        //
        Dictionary<Path_Node<Tile>, float> f_score = new Dictionary<Path_Node<Tile>, float>();
        foreach (Path_Node<Tile> n in nodes.Values)
        {
            f_score[n] = Mathf.Infinity;
        }
        f_score[start] = Heuristic_Cost_Estimate(start, goal);

        while (OpenSet.Count > 0)
        {
            Path_Node<Tile> current = OpenSet.Dequeue();

            if (current == goal)
            {
                //我们达到了目标！ 
                //让我们把它转换成
                // tile的实际序列，然后结束这个构造函数！
                Reconstruct_path(Came_From, current);
                return;
            }

            ClosedSet.Add(current);

            foreach (Path_Edge<Tile> edge_neighbor in current.path_Edges)
            {
                Path_Node<Tile> neighbor = edge_neighbor.node;

                if (ClosedSet.Contains(neighbor) == true)
                    continue; //忽略这个已经完成的邻居

                float movement_cost_to_neighbor = neighbor.data.movementCost * Dist_Between(current, neighbor);

                float tentative_g_score = g_score[current] + movement_cost_to_neighbor;

                if (OpenSet.Contains(neighbor) && tentative_g_score >= g_score[neighbor])
                    continue;

                Came_From[neighbor] = current;
                g_score[neighbor] = tentative_g_score;
                f_score[neighbor] = g_score[neighbor] + Heuristic_Cost_Estimate(neighbor, goal);

                if (OpenSet.Contains(neighbor) == false)
                {
                    OpenSet.Enqueue(neighbor, f_score[neighbor]);
                }

            } // foreach 
        } // while

        //如果我们到达这里，就意味着我们已经烧毁了整个
        // OpenSet没有达到current == goal的点。
        //当从开始到目标没有路径时会发生这种情况
        //（所以有墙或缺少地板或其他东西）。

        //也许我们没有失败状态？ 只是那个
        //路径列表将为null。
    }
    /// <summary>
    /// 启发式成本估算
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    float Heuristic_Cost_Estimate(Path_Node<Tile> a, Path_Node<Tile> b)
    {

        return Mathf.Sqrt(
            Mathf.Pow(a.data.x - b.data.x, 2) +
            Mathf.Pow(a.data.y - b.data.y, 2)
        );

    }
    /// <summary>
    /// 路径之间的分支
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    float Dist_Between(Path_Node<Tile> a, Path_Node<Tile> b)
    {
        //我们可以做出假设，因为我们知道我们正在努力
        //此时在网格上。

        // Hori / Vert邻居的距离为1
        if (Mathf.Abs(a.data.x - b.data.x) + Mathf.Abs(a.data.y - b.data.y) == 1)
        {
            return 1f;
        }

        // Diag邻居的距离为1.41421356237
        if (Mathf.Abs(a.data.x - b.data.x) == 1 && Mathf.Abs(a.data.y - b.data.y) == 1)
        {
            return 1.41421356237f;
        }

        //否则，做实际的数学运算。
        return Mathf.Sqrt(
            Mathf.Pow(a.data.x - b.data.x, 2) +
            Mathf.Pow(a.data.x - b.data.x, 2)
        );

    }
    /// <summary>
    /// 重建路径
    /// </summary>
    /// <param name="Came_From"></param>
    /// <param name="current"></param>
    void Reconstruct_path(
        Dictionary<Path_Node<Tile>, Path_Node<Tile>> Came_From,
        Path_Node<Tile> current
    )
    {
        //所以在这一点上，目前是目标。
        //所以我们想要做的是向后走过Came_From
        // map，直到我们到达那张地图的“结尾”......这将是
        //我们的起始节点！
        Queue<Tile> total_path = new Queue<Tile>();
        total_path.Enqueue(current.data); //这个“最后”步骤是路径是目标！

        while (Came_From.ContainsKey(current))
        {
            // Came_From是一张地图，其中
            // key =>价值关系是真实的说法
            // some_node => we_got_there_from_this_node
            current = Came_From[current];
            total_path.Enqueue(current.data);
        }

        //此时，total_path是一个正在运行的队列
        //从END磁贴向后转到START磁贴，让我们反转它。
        path = new Queue<Tile>(total_path.Reverse());

    }
    /// <summary>
    /// 获取下一个tile
    /// </summary>
    /// <returns></returns>
    public Tile Dequeue()
    {
        return path.Dequeue();
    }
    /// <summary>
    /// 路径长度
    /// </summary>
    /// <returns></returns>
    public int Length()
    {
        if (path == null)
        {
            return 0;
        }
        return path.Count;
    }
}

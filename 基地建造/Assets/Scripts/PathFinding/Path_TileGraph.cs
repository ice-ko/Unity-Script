using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 这个类构造了一个简单的路径查找兼容图
/// 我们的世界 每个图块都是一个节点。 每个WALKABLE邻居
/// 来自图块是通过边连接链接的。
/// </summary>
public class Path_TileGraph
{
    public Dictionary<Tile, Path_Node<Tile>> nodes;
    public Path_TileGraph(World world)
    {
        //遍历世界上所有的瓷砖
        //对于每个图块，创建一个节点
        //我们是否为非地砖创建节点？ 没有！
        //我们是否为完全不可行走的瓷砖（即墙壁）创建节点？ 没有！
        nodes = new Dictionary<Tile, Path_Node<Tile>>();

        for (int x = 0; x < world.width; x++)
        {
            for (int y = 0; y < world.height; y++)
            {
                Tile tile = world.GetTileAt(x, y);
                //移动成本为0的瓷砖是不能进行移动的位置
                //if (tile.movementCost > 0)
                //{
                Path_Node<Tile> n = new Path_Node<Tile>();
                n.data = tile;
                nodes.Add(tile, n);
                // }
            }
        }

        //现在再次遍历所有节点
        //为邻居创建边
        int edgeCount = 0;
        foreach (Tile tile in nodes.Keys)
        {
            Path_Node<Tile> n = nodes[tile];

            List<Path_Edge<Tile>> edges = new List<Path_Edge<Tile>>();

            // //获取Tile的邻居列表
            Tile[] neighbours = tile.GetNeighbours(true); //注意：某些数组点可能为空。

            //如果邻居是可步行的，则为相关节点创建边缘。
            for (int i = 0; i < neighbours.Length; i++)
            {
                //if (neighbours[i] != null && neighbours[i].movementCost > 0)
                if (neighbours[i] != null && neighbours[i].movementCost > 0 && IsClippingCorner(tile, neighbours[i]) == false)
                {
                    {
                        //这个邻居存在且可以步行，所以创造一个边缘。
                        Path_Edge<Tile> e = new Path_Edge<Tile>();
                        e.cost = neighbours[i].movementCost;
                        e.node = nodes[neighbours[i]];

                        //将边添加到我们的临时（和可扩展的！）列表中
                        edges.Add(e);

                        edgeCount++;
                    }
                }
                n.path_Edges = edges.ToArray();
            }
        }
        /// <summary>
        /// 是否斜角
        /// </summary>
        /// <param name="curr"></param>
        /// <param name="neigh"></param>
        /// <returns></returns>
        bool IsClippingCorner(Tile curr, Tile neigh)
        {
            //如果从curr到neigh的运动是对角线的（例如N-E）
            //然后检查以确保我们没有斜角（例如N和E都可以走路）
            int dX = curr.x - neigh.x;
            int dY = curr.y - neigh.y;

            if (Mathf.Abs(dX) + Mathf.Abs(dY) == 2)
            {
                //我们是对角的
                if (curr.world.GetTileAt(curr.x - dX, curr.y).movementCost == 0)
                {
                    //东方或西方是不可行的，因此这将是一个斜角的运动。
                    return true;
                }

                if (curr.world.GetTileAt(curr.x, curr.y - dY).movementCost == 0)
                {//北或南是不可行的，因此这将是一个斜角的运动。
                    return true;
                }

                //如果我们到达这里，我们是对角线，但不是斜角
            }
            //如果我们在这里，我们要么不斜角，要么不对角
            return false;
        }
    }
}

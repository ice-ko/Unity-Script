using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Node
{
    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }
    /// <summary>
    /// 父级
    /// </summary>
    public Node Parent { get; set; }
    /// <summary>
    /// 坐标
    /// </summary>
    public Vector3Int Position { get; set; }
    public Node(Vector3Int position)
    {
        this.Position = position;
    }
}
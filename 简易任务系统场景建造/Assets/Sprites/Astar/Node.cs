using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 节点
/// </summary>
public class Node
{
    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }
    //
    public Node Parent { get; set; }
    public Vector3Int Position { get; set; }

    public Node(Vector3Int position)
    {
        this.Position = position;
    }
}

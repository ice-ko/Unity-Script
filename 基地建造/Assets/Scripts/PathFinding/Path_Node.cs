using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_Node<T>
{
    public T data;
    //路径边缘
    public Path_Edge<T>[] path_Edges;
}

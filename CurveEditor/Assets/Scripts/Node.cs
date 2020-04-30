using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NODETYPE
{
    EMPTY = 0,
    NORMAL,
    EDGE,
    DOUBLE_EDGE
}

[System.Serializable]
public class Node
{
    public Vector3 Position;
    public NODETYPE type;
    public Node Left;
    public Node Right;
    public Node Back;
    public Node Forward;
    public Node Up;
    public Node Down;

}

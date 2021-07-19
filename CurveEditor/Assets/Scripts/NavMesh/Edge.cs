using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Edge
{
    public int index1;
    public int index2;

    public Edge(int firstIndex, int secondIndex)
    {
        index1 = firstIndex;
        index2 = secondIndex;
    }
}

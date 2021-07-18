using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point
{
    int Index = 0;
    public Vector3 Position;
    public List<Point> ConnectingPoints;

    public Point(Vector3 pos, int index)
    {
        Index = index;
        Position = pos;
        ConnectingPoints = new List<Point>();
    }
}
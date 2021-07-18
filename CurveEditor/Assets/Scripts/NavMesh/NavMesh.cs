using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class NavMesh
{
    [SerializeField]
    public List<Point> points;

    public List<Vector3> PointPositions;
    public List<int> Tris;

    [SerializeField]
    public List<Tuple<int, int>> Edges;

    [SerializeField]
    public float verticalOffsetFromGround = .1f;

    public NavMesh(Vector3 center)
    {
        RaycastHit hit;
        Vector3 offset = Vector3.zero;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(center, Vector3.down, out hit, Mathf.Infinity))
        {
            offset.y = -hit.distance + verticalOffsetFromGround;
        }
        
        points = new List<Point>
        {
            new Point(offset + Vector3.left, 0),
            new Point(offset + Vector3.forward, 1),
            new Point(offset + Vector3.right, 2),
            new Point(offset + Vector3.back, 3) 
        };

        //points[0].ConnectingPoints.Add(points[1]);
        //points[0].ConnectingPoints.Add(points[2]);
        //points[0].ConnectingPoints.Add(points[3]);

        PointPositions = new List<Vector3>
        {
            offset + Vector3.left,
            offset + Vector3.forward,
            offset + Vector3.right,
            offset + Vector3.back
        };

        Tris = new List<int> { 0, 1, 2, 2, 3, 0};
        
        Edges = new List<Tuple<int, int>> { new Tuple<int, int>(0, 1), new Tuple<int, int>(1, 2), new Tuple<int, int>(2, 0),
                                             new Tuple<int, int>(2, 3), new Tuple<int, int>(3, 0)};

    }
    public int NumPoints
    {
        get
        {
            return points.Count;
        }
    }
    public int NumFaces
    {
        get
        {
            return points.Count - 2;
        }
    }

    public void AddPointEdge(int index1, int index2, Vector3 point)
    {
        //points.Insert(index, new Point(point, points.Count));

        PointPositions.Add(point);

    }

    public void MovePoint(int index, Vector3 position)
    {
        points[index].Position = position;

        PointPositions[index] = position;
    }
}

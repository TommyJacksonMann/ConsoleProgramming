using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class NavMesh
{

    public List<Vector3> PointPositions;
    public List<int> Tris;

    [SerializeField]
    public List<Edge> Edges;

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

        PointPositions = new List<Vector3>
        {
            offset + Vector3.left,
            offset + Vector3.forward,
            offset + Vector3.right,
            offset + Vector3.back
        };

        Tris = new List<int> { 0, 1, 2, 2, 3, 0};
        
        Edges = new List<Edge> { new Edge(0, 1), new Edge(1, 2), new Edge(2, 0),
                                             new Edge(2, 3), new Edge(3, 0)};

    }
    public int NumPoints
    {
        get
        {
            return PointPositions.Count;
        }
    }
    public int NumFaces
    {
        get
        {
            return PointPositions.Count - 2;
        }
    }

    public void AddPointEdge(int index1, int index2, Vector3 point)
    {
        //points.Insert(index, new Point(point, points.Count));

        PointPositions.Add(point);
        int[] newTri = new int[] { PointPositions.Count - 1, index2, index1};
        Tris.AddRange(newTri);

        Edges.Add(new Edge(index1, PointPositions.Count - 1));
        Edges.Add(new Edge(PointPositions.Count - 1, index2));
    }

    public void MovePoint(int index, Vector3 position)
    {
        //points[index].Position = position;

        PointPositions[index] = position;
    }
}

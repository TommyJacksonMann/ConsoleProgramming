using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class NavMesh
{
    [SerializeField]
    public List<Vector3> points;

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
        
        points = new List<Vector3>
        {
            offset + Vector3.left,
            offset + Vector3.forward,
            offset + Vector3.right
        };
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

    public void InsertPoint(int index, Vector3 point)
    {
        points.Insert(index, point);
    }

    public void MovePoint(int index, Vector3 position)
    {
        points[index] = position;
    }
}

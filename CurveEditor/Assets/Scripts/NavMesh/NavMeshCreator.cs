using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class NavMeshCreator : MonoBehaviour
{
    [HideInInspector]
    public NavMesh navMesh;

    public UnityEngine.Color PointColor = UnityEngine.Color.red;
    public UnityEngine.Color PointHoverColor = UnityEngine.Color.green;
    public float PointSize;
    

    public void CreateNavMesh()
    {
        navMesh = new NavMesh(transform.position);
    }

    private void Reset()
    {
        CreateNavMesh();
    }

    public void UpdateMesh(List<Vector3> points)
    {
        Vector3[] verts = new Vector3[points.Count];
        Vector2[] uvs = new Vector2[verts.Length];
        int[] tris = new int[] { 0, 1, 2 };
        Vector3[] normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up };

        verts = points.ToArray();
        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(0, 1);
        uvs[2] = new Vector2(1, 0);

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.normals = normals;
        

        GetComponent<MeshFilter>().mesh = mesh;
    }
}

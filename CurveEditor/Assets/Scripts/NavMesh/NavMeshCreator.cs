using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class NavMeshCreator : MonoBehaviour
{

    public NavMesh navMesh;

    public UnityEngine.Color PointColor = UnityEngine.Color.red;
    public UnityEngine.Color PointHoverColor = UnityEngine.Color.green;
    public float PointSize;

    

    public void CreateNavMesh()
    {
        navMesh = new NavMesh(transform.position);
        UpdateMesh();
    }

    private void Reset()
    {
        CreateNavMesh();
    }

    public void UpdateMesh()
    {
        Vector3[] verts = navMesh.PointPositions.ToArray();
        Vector2[] uvs = new Vector2[verts.Length];
        int[] tris = navMesh.Tris.ToArray();
        Vector3[] normals = new Vector3[navMesh.PointPositions.Count];


        for(int i = 0; i < navMesh.PointPositions.Count; i++)      //********FILL VERTS AND NORMALS***************
        {
            normals[i] = Vector3.up;

            uvs[i] = new Vector2(i/ (navMesh.PointPositions.Count-1), i / (navMesh.PointPositions.Count - 1));
        }
        
        //uvs[0] = new Vector2(0, 0);
        //uvs[1] = new Vector2(0, .5f);
        //uvs[2] = new Vector2(.5f, 0);
        //uvs[2] = new Vector2(1, 1);

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.normals = normals;
        
        GetComponent<MeshFilter>().mesh = mesh;
    }
}

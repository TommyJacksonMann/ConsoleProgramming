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
        int[] numSurfacesPerVertex = new int[navMesh.PointPositions.Count];

        for (int i = 0; i < navMesh.PointPositions.Count; i++)      //********FILL VERTS AND NORMALS***************
        {
            numSurfacesPerVertex[i] = 0;

            uvs[i] = new Vector2(i / (navMesh.PointPositions.Count - 1), i / (navMesh.PointPositions.Count - 1));
        }

        for (int i = 0; i < navMesh.Tris.Count/3; i++)
        {
            int triIndex = i * 3;
            Vector3 surfNormal = HelperFunctions.CalculateSurfaceNormal(verts[tris[triIndex]], verts[tris[triIndex+1]], verts[tris[triIndex+2]]);

            numSurfacesPerVertex[tris[triIndex]] += 1;
            numSurfacesPerVertex[tris[triIndex+1]] += 1;
            numSurfacesPerVertex[tris[triIndex+2]] += 1;

            normals[tris[triIndex]] += surfNormal;
            normals[tris[triIndex+1]] += surfNormal;
            normals[tris[triIndex+2]] += surfNormal;
        }

        for (int i = 0; i < navMesh.PointPositions.Count; i++)      //********FILL VERTS AND NORMALS***************
        {
            normals[i] /= numSurfacesPerVertex[i];
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.normals = normals;
        
        GetComponent<MeshFilter>().mesh = mesh;
    }
}

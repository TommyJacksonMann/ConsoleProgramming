using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NavMeshCreator))]
public class NavMeshEditor : Editor
{
    NavMeshCreator creator;

    Vector3 testPoint = Vector3.zero;

    NavMesh NavMesh
    {
        get
        {
            return creator.navMesh;
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Create New"))
        {
            Undo.RecordObject(creator, "Create New");
            creator.CreateNavMesh();
            SceneView.RepaintAll();
        }

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    private void OnSceneGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {
            //creator.UpdateMesh(NavMesh.points);
        }
        Draw();
        Input();
    }

    private void OnEnable()
    {
        creator = (NavMeshCreator)target;
    }

    void Draw()
    {
        
        for (int i = 0; i < NavMesh.points.Count; i++)
        {
            Handles.color = creator.PointColor;
            float handleSize = creator.PointSize;
            Vector3 newPos = Handles.FreeMoveHandle(NavMesh.points[i] + creator.transform.position, Quaternion.identity, handleSize, Vector3.zero, Handles.CylinderHandleCap);
            if (NavMesh.points[i] != newPos - creator.transform.position)
            {
                Undo.RecordObject(creator, "Move point");
                creator.navMesh.MovePoint(i, newPos - creator.transform.position);
                creator.UpdateMesh();
            }
        }
    }
    void Input()
    {
        Event guiEvent = Event.current;
        Vector3 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        HandleUtility.AddDefaultControl(0);
    }
}

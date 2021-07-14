using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NavMeshCreator))]
public class NavMeshEditor : Editor
{
    NavMeshCreator creator;

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
    }

    private void OnSceneGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {
            creator.UpdateMesh(NavMesh.points);
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
        for (int i = 0; i < creator.navMesh.points.Count; i++)
        {
            Handles.color = creator.PointColor;
            float handleSize = creator.PointSize;
            Vector3 newPos = Handles.FreeMoveHandle(creator.navMesh.points[i], Quaternion.identity, handleSize, Vector3.zero, Handles.CylinderHandleCap);
            if (creator.navMesh.points[i] != newPos)
            {
                Undo.RecordObject(creator, "Move point");
                creator.navMesh.MovePoint(i, newPos);
            }
        }
    }
    void Input()
    {
        Event guiEvent = Event.current;
        Vector3 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
    }
}

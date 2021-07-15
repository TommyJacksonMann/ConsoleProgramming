using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NavMeshCreator))]
public class NavMeshEditor : Editor
{
    NavMeshCreator creator;

    Event guiEvent;

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
        Input();
        Draw();
        
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
            Vector3 oldPos = NavMesh.points[i] + creator.transform.position;
            Vector3 newPos = Handles.FreeMoveHandle(NavMesh.points[i] + creator.transform.position, Quaternion.identity, handleSize, Vector3.zero, Handles.CylinderHandleCap);

            if (NavMesh.points[i] != newPos - creator.transform.position )
            {
                Undo.RecordObject(creator, "Move point");

                RaycastHit hit;
                Vector3 rayDir = newPos - Camera.current.transform.position;
                float rayDist = (Camera.current.transform.position - newPos).magnitude;

                if (guiEvent.shift && Physics.Raycast(Camera.current.transform.position, rayDir, out hit, 15))
                {
                    creator.navMesh.MovePoint(i, hit.point + new Vector3(0, NavMesh.verticalOffsetFromGround, 0) - creator.transform.position);
                }
                else
                {
                    creator.navMesh.MovePoint(i, newPos - creator.transform.position);
                }
                
                creator.UpdateMesh();
            }
        }
    }
    void Input()
    {
        guiEvent = Event.current;
        Vector2 screenSpaceMousePos = guiEvent.mousePosition;
        screenSpaceMousePos.y = Camera.current.pixelRect.height - screenSpaceMousePos.y; //Done to invert the Y pixel coordinates
        Vector3 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if(guiEvent.control)
        {
            float distanceToLine = 100;
            int closestLineIndex = -1;
            for (int i = 0; i < NavMesh.points.Count - 1; i++)
            {
                Vector2 point1 = Camera.current.WorldToScreenPoint(NavMesh.points[i] + creator.transform.position);
                Vector2 point2 = Camera.current.WorldToScreenPoint(NavMesh.points[i+1] + creator.transform.position);

                float tempDistance = HandleUtility.DistancePointLine(screenSpaceMousePos, point1, point2);
                if(tempDistance < distanceToLine && tempDistance < 15)
                {
                    distanceToLine = tempDistance;
                    closestLineIndex = i;
                }
            }
            if(closestLineIndex != -1)
            {
                Handles.color = creator.PointHoverColor;
                float handleSize = creator.PointSize;
                Handles.DrawLine(NavMesh.points[closestLineIndex] + creator.transform.position,
                    NavMesh.points[closestLineIndex + 1] + creator.transform.position);
            }
                
        }

        HandleUtility.AddDefaultControl(0);
    }

    
}

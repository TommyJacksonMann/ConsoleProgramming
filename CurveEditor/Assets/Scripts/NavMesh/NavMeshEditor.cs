using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.XR;

[CustomEditor(typeof(NavMeshCreator))]
public class NavMeshEditor : Editor
{
    NavMeshCreator creator;

    Event guiEvent;
    int closestPointIndex1 = -1;
    int closestPointIndex2 = -1;

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
        Draw();
        Input();
        
        
    }

    private void OnEnable()
    {
        creator = (NavMeshCreator)target;
    }

    void Draw()
    {
        
        for (int i = 0; i < NavMesh.PointPositions.Count; i++)
        {
            Handles.color = creator.PointColor;
            float handleSize = creator.PointSize;
            Vector3 oldPos = NavMesh.PointPositions[i] + creator.transform.position;
            Vector3 newPos = Handles.FreeMoveHandle(NavMesh.PointPositions[i] + creator.transform.position, Quaternion.identity, handleSize, Vector3.zero, Handles.CylinderHandleCap);
            Handles.Label(NavMesh.PointPositions[i] + creator.transform.position, i.ToString());

            if (NavMesh.PointPositions[i] != newPos - creator.transform.position)
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
        SceneView.RepaintAll();
    }
    void Input()
    {
        guiEvent = Event.current;
        Vector2 screenSpaceMousePos = guiEvent.mousePosition;
        screenSpaceMousePos.y = Camera.current.pixelRect.height - screenSpaceMousePos.y; 

        if (guiEvent.type == EventType.MouseMove)
        {
            float distanceToLine = 15;
            
            closestPointIndex1 = -1;
            closestPointIndex2 = -1;
            
            for (int i = 0; i < NavMesh.Edges.Count; i++)
            {
                int triIndex = i * 3;
                Vector2 point1 = Camera.current.WorldToScreenPoint(NavMesh.PointPositions[NavMesh.Edges[i].Item1] + creator.transform.position);
                Vector2 point2 = Camera.current.WorldToScreenPoint(NavMesh.PointPositions[NavMesh.Edges[i].Item2] + creator.transform.position);
            
                float tempDistance = HandleUtility.DistancePointLine(screenSpaceMousePos, point1, point2);
            
                if (tempDistance < distanceToLine)
                {
                    distanceToLine = tempDistance;
                    closestPointIndex1 = NavMesh.Edges[i].Item1;
                    closestPointIndex2 = NavMesh.Edges[i].Item2;
                }
            
            }
        }


        if (guiEvent.control)
        {
            if (closestPointIndex1 != -1)
            {
                Handles.color = creator.PointHoverColor;
                Handles.DrawLine(NavMesh.points[closestPointIndex1].Position + creator.transform.position,
                    NavMesh.points[closestPointIndex2].Position + creator.transform.position);

            }
        }

        HandleUtility.AddDefaultControl(0);
    }

    
}

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NavMeshCreator))]
public class NavMeshEditor : Editor
{
    NavMeshCreator creator;

    Event guiEvent;
    public Edge closestEdge = null;

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
        Draw();
        Input();
        
        
    }

    private void OnEnable()
    {
        creator = (NavMeshCreator)target;
    }

    void Draw()
    {
        //Handles.color = creator.PointHoverColor;
        //testPoint = Handles.FreeMoveHandle(testPoint, Quaternion.identity, creator.PointSize, Vector3.zero, Handles.CylinderHandleCap);

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

            closestEdge = null;


            for (int i = 0; i < NavMesh.Edges.Count; i++)
            {
                int triIndex = i * 3;
                Vector2 point1 = Camera.current.WorldToScreenPoint(NavMesh.PointPositions[NavMesh.Edges[i].index1] + creator.transform.position);
                Vector2 point2 = Camera.current.WorldToScreenPoint(NavMesh.PointPositions[NavMesh.Edges[i].index2] + creator.transform.position);
            
                float tempDistance = HandleUtility.DistancePointLine(screenSpaceMousePos, point1, point2);
            
                if (tempDistance < distanceToLine)
                {
                    distanceToLine = tempDistance;
                    closestEdge = NavMesh.Edges[i];
                }
            
            }
        }


        if (guiEvent.control)
        {
            if (closestEdge != null)
            {
                Handles.color = creator.PointHoverColor;
                Handles.DrawLine(NavMesh.PointPositions[closestEdge.index1] + creator.transform.position,
                    NavMesh.PointPositions[closestEdge.index2] + creator.transform.position);
                if(guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
                {
                    Vector2 point1 = Camera.current.WorldToScreenPoint(NavMesh.PointPositions[closestEdge.index1] + creator.transform.position);
                    Vector2 point2 = Camera.current.WorldToScreenPoint(NavMesh.PointPositions[closestEdge.index2] + creator.transform.position);
                    Vector2 closestPointScreenSpace = HelperFunctions.FindNearestPointOnLine(point1, point2, screenSpaceMousePos);

                    Ray closestPointRay = Camera.current.ScreenPointToRay(closestPointScreenSpace);

                    testPoint = HelperFunctions.GetIntersectWithLineAndPlane(closestPointRay.origin, closestPointRay.direction, 
                                                                Vector3.up, NavMesh.PointPositions[closestEdge.index1] + creator.transform.position);

                    NavMesh.AddPointEdge(closestEdge.index1, closestEdge.index2, testPoint - creator.transform.position);
                    creator.UpdateMesh();

                    //FOR FINDING THE INTERSECTION OF A LINE TO A PLANE
                    //************** https://www.youtube.com/watch?v=Td9CZGkqrSg**********************
                }
            }
        }

        HandleUtility.AddDefaultControl(0);
    }

    
}

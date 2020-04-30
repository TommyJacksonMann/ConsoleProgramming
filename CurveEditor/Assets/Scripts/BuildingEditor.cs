using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BuildingCreator))]
public class BuildingEditor : Editor
{
    BuildingCreator creator;
    public GameObject cube;
    bool displayAvailableNodes;
    bool displayCorners;


    Event guiEvent;
    Vector3 mousePos;
    Vector3 mouseDir;
    RaycastHit hit;
    Node highlightedNode;

    

    Building Building
    {
        get
        {
            return creator.building;
        }
    }
    Grid Grid
    {
        get
        {
            return creator.grid;
        }
    }
    private void OnSceneGUI()
    {
        Input();
        Draw();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        //if (GUILayout.Button("Create New"))
        //{
        //    Undo.RecordObject(creator, "Create New");
        //    SceneView.RepaintAll();
        //}

        cube = EditorGUILayout.ObjectField("Cube Piece", cube, typeof(GameObject), false) as GameObject;

        displayAvailableNodes = GUILayout.Toggle(displayAvailableNodes, "Show Available Nodes");
        displayCorners = GUILayout.Toggle(displayCorners, "Show Corners");


        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }


    void Input()
    {
        guiEvent = Event.current;
        mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        mouseDir = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).direction;
        // Does the ray intersect any objects excluding the player layer
        

        if (guiEvent.type == EventType.MouseDown && (guiEvent.button == 0 || guiEvent.button == 1) && guiEvent.shift)
        {

            if (Physics.Raycast(mousePos, mouseDir, out hit, Mathf.Infinity))
            {
                Object pObject = Resources.Load("Assets/Cube");
                GameObject a = PrefabUtility.InstantiatePrefab(pObject) as GameObject;

                float xOffset = 0;
                float zOffset = 0;
                if (hit.point.x <= 0)
                {
                    xOffset = -.5f;
                }
                else
                {
                    xOffset = .5f;
                }
                if (hit.point.z <= 0)
                {
                    zOffset = -.5f;
                }
                else
                {
                    zOffset = .5f;
                }


                if (guiEvent.button == 0)
                {
                    //highlightedNode = creator.GetClosestNode(new Vector3((int)(hit.point.x) + xOffset, hit.point.y + .5f, (int)hit.point.z + zOffset), true);
                    highlightedNode = creator.GetClosestNode(hit.point, true);
                    GameObject newPiece = Instantiate(cube, highlightedNode.Position, Quaternion.identity);
                    highlightedNode.type = NODETYPE.NORMAL;
                    Building.addBuildingPiece(newPiece);
                    creator.UpdateAvailableNodes();
                }
                else if (guiEvent.button == 1 && hit.collider.tag == "BuildingPiece")
                {
                    highlightedNode = creator.GetClosestNode(hit.point, false);
                    highlightedNode.type = NODETYPE.EMPTY;

                    Building.removeBuildingPiece(hit.collider.gameObject);
                    DestroyImmediate(hit.collider.gameObject);

                    creator.UpdateAvailableNodes();
                }
            }
        }

        HandleUtility.AddDefaultControl(0);
    }
    void Draw()
    {

        if (creator.availableNodes != null )
        {
            if(displayAvailableNodes)
            {
                for (int i = 0; i < creator.availableNodes.Count; i++)
                {
                    
                    if (displayAvailableNodes)
                    {
                        Handles.color = Color.green;
                        float handleSize = .3f;
                        Vector3 newPos = Handles.FreeMoveHandle(creator.availableNodes[i].Position, Quaternion.identity, handleSize, Vector3.zero, Handles.SphereHandleCap);
                    }
                }
            }
        }

        if(creator.closedNodes != null)
        {
            if (displayCorners)
            {
                DrawCorners();
            }
        }
    }

    void DrawCorners()
    {
        for (int i = 0; i < creator.closedNodes.Count; i++)
        {
            Handles.color = Color.blue;
            float handleSize = .2f;

            bool down = creator.closedNodes[i].Down == null || creator.closedNodes[i].Down.type == NODETYPE.EMPTY ? true : false;
            bool up = creator.closedNodes[i].Up == null || creator.closedNodes[i].Up.type == NODETYPE.EMPTY ? true : false;
            bool left = creator.closedNodes[i].Left == null || creator.closedNodes[i].Left.type == NODETYPE.EMPTY ? true : false;
            bool right = creator.closedNodes[i].Right == null || creator.closedNodes[i].Right.type == NODETYPE.EMPTY ? true : false;
            bool back = creator.closedNodes[i].Back == null || creator.closedNodes[i].Back.type == NODETYPE.EMPTY ? true : false;
            bool forward = creator.closedNodes[i].Forward == null || creator.closedNodes[i].Forward.type == NODETYPE.EMPTY ? true : false;


            if (up)
            {
                Vector3 offset = new Vector3(0, .5f, 0);
                if (left)
                {
                    if (back)
                    {
                        Vector3 horizontalOffset = new Vector3(-.5f, 0, -.5f);
                        Vector3 newPos = Handles.FreeMoveHandle(creator.closedNodes[i].Position + offset + horizontalOffset, Quaternion.identity,
                                                            handleSize, Vector3.zero, Handles.SphereHandleCap);
                        if(down)
                        {
                            newPos = Handles.FreeMoveHandle(creator.closedNodes[i].Position - offset + horizontalOffset, Quaternion.identity,
                                                            handleSize, Vector3.zero, Handles.SphereHandleCap);
                        }
                    }
                    if (forward)
                    {
                        Vector3 horizontalOffset = new Vector3(-.5f, 0, .5f);
                        Vector3 newPos = Handles.FreeMoveHandle(creator.closedNodes[i].Position + offset + horizontalOffset, Quaternion.identity,
                                                            handleSize, Vector3.zero, Handles.SphereHandleCap);
                        if (down)
                        {
                            newPos = Handles.FreeMoveHandle(creator.closedNodes[i].Position - offset + horizontalOffset, Quaternion.identity,
                                                            handleSize, Vector3.zero, Handles.SphereHandleCap);
                        }
                    }
                }
                if (right)
                {
                    if (back)
                    {
                        Vector3 horizontalOffset = new Vector3(.5f, 0, -.5f);
                        Vector3 newPos = Handles.FreeMoveHandle(creator.closedNodes[i].Position + offset + horizontalOffset, Quaternion.identity,
                                                            handleSize, Vector3.zero, Handles.SphereHandleCap);
                        if (down)
                        {
                            newPos = Handles.FreeMoveHandle(creator.closedNodes[i].Position - offset + horizontalOffset, Quaternion.identity,
                                                            handleSize, Vector3.zero, Handles.SphereHandleCap);
                        }
                    }
                    if (forward)
                    {
                        Vector3 horizontalOffset = new Vector3(.5f, 0, .5f);
                        Vector3 newPos = Handles.FreeMoveHandle(creator.closedNodes[i].Position + offset + horizontalOffset, Quaternion.identity,
                                                            handleSize, Vector3.zero, Handles.SphereHandleCap);
                        if (down)
                        {
                            newPos = Handles.FreeMoveHandle(creator.closedNodes[i].Position - offset + horizontalOffset, Quaternion.identity,
                                                            handleSize, Vector3.zero, Handles.SphereHandleCap);
                        }
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        creator = (BuildingCreator)target;
        if (creator.building == null)
        {
            creator.CreateBuilding();

            Debug.Log("Building Created");
            
        }
    }
}

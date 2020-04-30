using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCreator : MonoBehaviour
{
    public Building building;

    public Grid grid;

    public List<Node> availableNodes = new List<Node>();
    public List<Node> closedNodes = new List<Node>();

    [HideInInspector]
    public GameObject normalPiece;
    [HideInInspector]
    public GameObject edgePiece;
    [HideInInspector]
    public GameObject doubleEdgePiece;

    BuildingCreator()
    {
        if(availableNodes == null)
        {
            availableNodes = new List<Node>();
        }
        if (closedNodes == null)
        {
            closedNodes = new List<Node>();
        }
    }
    // Start is called before the first frame update
    public void CreateBuilding()
    {
        building = new Building();
        building.normalPiece = normalPiece;
        building.edgePiece = edgePiece;
        building.doubleEdgePiece = doubleEdgePiece;

        grid = new Grid();
        if (availableNodes == null)
        {
            availableNodes = new List<Node>();
        }
        if (closedNodes == null)
        {
            closedNodes = new List<Node>();
        }
        UpdateAvailableNodes();
    }

    public void Reset()
    {
        CreateBuilding();
    }
    
    public Node GetClosestNode(Vector3 pos, bool closestAvailableNode = true)
    {
        Node closestNode = new Node();
        if(closestAvailableNode)
        {
            for (int i = 0; i < availableNodes.Count; i++)
            {
                Node currentNode = availableNodes[i];
                if (closestNode == null)
                {
                    closestNode = currentNode;
                }
                else
                {
                    if ((currentNode.Position - pos).magnitude < (closestNode.Position - pos).magnitude)
                    {
                        closestNode = currentNode;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < closedNodes.Count; i++)
            {
                Node currentNode = closedNodes[i];
                if (closestNode == null)
                {
                    closestNode = currentNode;
                }
                else
                {
                    if ((currentNode.Position - pos).magnitude < (closestNode.Position - pos).magnitude)
                    {
                        closestNode = currentNode;
                    }
                }
            }
        }
        
        return closestNode;
    }
    public void UpdateAvailableNodes()
    {
        availableNodes.Clear();
        for (int i = 0; i < grid.Width; i++)
        {
            for (int j = 0; j < grid.Height; j++)
            {
                for (int k = 0; k < grid.Depth; k++)
                {
                    if ((grid.Nodes[i, j, k].Down == null || grid.Nodes[i, j, k].Down.type != NODETYPE.EMPTY) && grid.Nodes[i, j, k].type == NODETYPE.EMPTY)
                    {
                        availableNodes.Add(grid.Nodes[i, j, k]);
                    } 
                    else
                    {
                        if (availableNodes.Contains(grid.Nodes[i, j, k]))
                        {
                            availableNodes.Remove(grid.Nodes[i, j, k]);
                        }
                    }

                    if(grid.Nodes[i, j, k].type != NODETYPE.EMPTY)
                    {
                        closedNodes.Add(grid.Nodes[i, j, k]);
                    }
                    else
                    {
                        if (closedNodes.Contains(grid.Nodes[i, j, k]))
                        {
                            closedNodes.Remove(grid.Nodes[i, j, k]);
                        }
                    }
                }
            }
        }
    }
}

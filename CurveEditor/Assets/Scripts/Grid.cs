using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Grid
{
    public float Spacing = 1;
    public Node[,,] Nodes;

    public int Width = 10;
    public int Height = 10;
    public int Depth = 10;
    public Grid()
    {
        Nodes = new Node[Width, Height, Depth];
        PopulateGrid();
    }
    public void PopulateGrid()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                for (int k = 0; k < Depth; k++)
                {
                    Node newNode = new Node();
                    Vector3 pos = new Vector3(i + .5f, j + .5f, k + .5f);
                    newNode.Position = pos;

                    
                    if(i != 0)
                    {
                        newNode.Left = Nodes[i - 1, j, k];
                        Nodes[i - 1, j, k].Right = newNode;
                        if (i == Width - 1)
                        {
                            newNode.Right = null;
                        }
                    }
                    else
                    {
                        newNode.Left = null;
                    }
                    if (j != 0)
                    {
                        newNode.Down = Nodes[i , j - 1, k];
                        Nodes[i, j - 1, k].Up = newNode;
                        if (j == Height - 1)
                        {
                            newNode.Up = null;
                        }
                    }
                    else
                    {
                        newNode.Down = null;
                    }
                    if (k != 0)
                    {
                        newNode.Back = Nodes[i, j, k - 1];
                        Nodes[i, j, k - 1].Forward = newNode;
                        if(k == Depth - 1)
                        {
                            newNode.Forward = null;
                        }
                    }
                    else
                    {
                        newNode.Back = null;
                    }
                    newNode.type = NODETYPE.EMPTY;
                    Nodes[i, j, k] = newNode;
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

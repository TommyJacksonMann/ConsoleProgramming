using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building 
{
    [SerializeField]
    public List<GameObject> buildingPieces;

    public GameObject normalPiece;
    public GameObject edgePiece;
    public GameObject doubleEdgePiece;

    public Building()
    {
        buildingPieces = new List<GameObject>();
    }

    public void addBuildingPiece(GameObject newPiece)
    {
        buildingPieces.Add(newPiece);
    }

    public void removeBuildingPiece(GameObject piece)
    {
        buildingPieces.Remove(piece);
    }
}

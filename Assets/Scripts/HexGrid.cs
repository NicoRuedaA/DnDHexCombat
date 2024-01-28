using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    
    
    Dictionary<Vector3Int, Hex> hexTileDict = new Dictionary<Vector3Int, Hex>();
    Dictionary<Vector3Int, List<Vector3Int>> hexTileNeighboursDict = new Dictionary<Vector3Int, List<Vector3Int>>();

    private List<Vector3Int> neighbours = new List<Vector3Int>();

    private HexCoordinates _hexCoordinates;
    
    private static HexGrid _instance;
    public static HexGrid instance{
        get{
            if(_instance == null){
                Debug.Log("Hex Grid is Null!!!");
            }
            return _instance;
        }
    }
    
    
    private void Awake()
    {
        
        _instance = this;
        
        foreach (Hex hex in FindObjectsOfType<Hex>())
        {
            hexTileDict[hex.HexCoords] = hex;
        }


        _hexCoordinates = GetComponent<HexCoordinates>();


    }

    

    public Hex GetTileAt(Vector3Int hexCoordinates)
    {
        Hex result = null;
        hexTileDict.TryGetValue(hexCoordinates, out result);
        //Debug.Log(result);
        return result;
    }

    //requiere posicion en el grid
    public List<Vector3Int> GetNeighboursFor(Vector3Int hexCoordinates)
    { 
        
        if (hexTileDict.ContainsKey(hexCoordinates) == false)
            return new List<Vector3Int>();
        if (hexTileNeighboursDict.ContainsKey(hexCoordinates))
            return hexTileNeighboursDict[hexCoordinates];

        
        
        hexTileNeighboursDict.Add(hexCoordinates, new List<Vector3Int>());

        foreach (var direction in Direction.GetDirectionList(hexCoordinates.z))
        {
            if (hexTileDict.ContainsKey(hexCoordinates + direction))
            {
                hexTileNeighboursDict[hexCoordinates].Add((hexCoordinates + direction));
            }
        }
        
        return hexTileNeighboursDict[hexCoordinates];

    }

    public void DisableNeighbours(Vector3Int hexCoordinates)
    {
        foreach(var kvp in neighbours)
        //foreach(var kvp in hexTileNeighboursDict[hexCoordinates])
        {
            GetTileAt(kvp).DisableHighlight();
        }
    }


    public void EnableNeighbours(Vector3Int hexCoordinates)
    {

        //neighbours =  GetNeighboursFor(hexCoordinates);
        BFSResult bfsResult = GraphSearch.BFSGetRange(hexCoordinates, 20);
        neighbours = new List<Vector3Int>(bfsResult.GetRangePositions());


        
        foreach(var kvp in neighbours)
        //foreach(var kvp in hexTileNeighboursDict[hexCoordinates])
        {
            Debug.Log(kvp);
            GetTileAt(kvp).EnableHighlight();
        }
    }


    public  Vector3Int GetClosestHex(Vector3 worldPosition)
    {
        worldPosition.y = 0;
        //return _hexCoordinates.ConvertPositionToOffset(worldPosition);
        return HexCoordinates.ConvertPositionToOffset(worldPosition);
    }


}

public static class Direction
{
    public static List<Vector3Int> directionsOffSetOdd = new List<Vector3Int>
    {
        new Vector3Int (-1,0,1),
        new Vector3Int (0, 0, 1),
        new Vector3Int (1, 0, 0),
        new Vector3Int (0, 0, -1),
        new Vector3Int (-1, 0, -1),
        new Vector3Int (-1, 0, 0),

    };
    
    public static List<Vector3Int> directionsOffSetEven = new List<Vector3Int>
    {
        new Vector3Int (0,0,1),
        new Vector3Int (1, 0, 1),
        new Vector3Int (1, 0, 0),
        new Vector3Int (1, 0, -1),
        new Vector3Int (0, 0, -1),
        new Vector3Int (-1, 0, 0),

    };

    public static List<Vector3Int> GetDirectionList(int z) => z % 2 == 0 ? directionsOffSetEven : directionsOffSetOdd;

}

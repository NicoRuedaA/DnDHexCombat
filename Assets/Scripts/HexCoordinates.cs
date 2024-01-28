using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoordinates : MonoBehaviour
{
    public static float xOffset = 1.73f, yOffset = 1, zOffset = 1.5f;

    internal Vector3Int GetHexCoords() => offSetCoordinates;
        
    [Header("Offset Coordinates")]
    //[SerializeField]
    public Vector3Int offSetCoordinates;

    
    private void Awake()
    {
       offSetCoordinates = ConvertPositionToOffset(transform.position);
       transform.name = offSetCoordinates.ToString();
    }


    //coordiantes to Grid
    public static Vector3Int ConvertPositionToOffset(Vector3 position)
    {

        int x = Mathf.CeilToInt((position.x / xOffset));
        int y = Mathf.RoundToInt((position.y / yOffset));
        int z = Mathf.RoundToInt((position.z / zOffset));

        return new Vector3Int(x, y, z);

    }
}

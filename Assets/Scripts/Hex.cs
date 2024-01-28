using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//seleccionamos siempre al padre
[SelectionBase]
public class Hex : MonoBehaviour
{


    [SerializeField] private HexType _hexType;

    private HexCoordinates _hexCoordinates;
    public Vector3Int HexCoords => _hexCoordinates.GetHexCoords();

    private GlowHighlight _glowHighlight;

    
    private void Awake()
    {
        _hexCoordinates = GetComponent<HexCoordinates>();
        //_renderer = transform.GetChild(0).GetComponentInChildren<Renderer>();
        //_originalMaterial = transform.GetChild(0).GetComponentInChildren<Renderer>().material;
        _glowHighlight = GetComponent<GlowHighlight>();
    }


    /*private Borrar hexGridConverter;

     void Start()
    {
        hexGridConverter = GetComponent<Borrar>();

        Vector3 worldPosition = new Vector3(10f, 0f, 5f);
        Vector3 hexCoordinates = hexGridConverter.WorldToHex(worldPosition);

        Debug.Log("Coordenadas Hexagonales: " + hexCoordinates);
    }*/



    public int GetCost() =>
        _hexType switch
        {
            HexType.Difficult => 15,
            HexType.Default => 10,
            HexType.Road => 5,
            _ => throw new Exception($"Hex of type {_hexType} not supported")
        };

    public bool IsObstacle()
    {
        return this._hexType == HexType.Obstacle;
    }


    public Vector3Int GetHexCoords() => GetComponent<HexCoordinates>().offSetCoordinates;

    

    /// <summary>
    /// highlight
    /// </summary>

    public void Select()
    {
        Transform child = transform.GetChild(0);
        if (child != null && child.childCount > 0)
        {
            Transform grandChild = child.GetChild(0);
            if (grandChild != null)
            {
                grandChild.tag = "Selected";
            }
        }
    }

    public void Deselect()
    {
        Transform child = transform.GetChild(0);
        if (child != null && child.childCount > 0)
        {
            Transform grandChild = child.GetChild(0);
            if (grandChild != null)
            {
                grandChild.tag = "Selectable";
            }
        }
    }
    
    
    public void DisableHighlight()
    {
        _glowHighlight.ToggleGlow(false);
    }

    public void EnableHighlight()
    {

        _glowHighlight.ToggleGlow(true);
        
    }
    
    

    public void ResetHighlight()
    {
        _glowHighlight.ResetGlowHighlight();
    }

    public void HighlightPath()
    {
        _glowHighlight.HighlightValidPath();

    }
}

public enum HexType
{
    None,
    Default,
    Difficult,
    Road,
    Water,
    Obstacle
}
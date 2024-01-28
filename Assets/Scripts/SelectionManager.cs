using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SelectionManager : MonoBehaviour
{

    public UnityEvent<GameObject> _onUnitSelected;
    public UnityEvent<GameObject> _onTerrainSelected;

    public LayerMask selectionMask;
    
    

    public Camera _camera;
    
    //private Hex _hex;
    
    
            
    /*private Material originalMaterialHighlight;
    private Material originalMaterialSelection;*/

    //private GameObject _selected;
    
    
    RaycastHit hit;
    
    private static SelectionManager _instance;
    
    public static SelectionManager instance {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("SelectionManager instance is Null!!!");
                // Lanzar excepción o manejar de otra manera según tus necesidades.
            }
            return _instance;
        }
    }

    
    
    private void Awake()
    {
        _instance = this;
        _camera = Camera.main;
        if (_camera == null)
        {
            Debug.LogError("Main Camera not found on SelectionManager!");
            // Puedes lanzar una excepción o manejar de otra manera según tus necesidades.
        }
    }
    
    


    public void HandleClick(Vector3 mousePos)
    {
        /*Ray ray = Camera.main.ScreenPointToRay(mousePos);
        
        
        
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Selectable"))
            {
                HandleSelectableHit();
            }
        }
        else
        {
            HandleNoHit();
        }*/
        GameObject result;
        if (FindTarget(mousePos, out result))
        {
            if (UnitSelected(result))
            {
                print(result.name);
                _onUnitSelected?.Invoke(result);
            }
            else
            {
                
                //_selected = hit.transform.parent.gameObject;
                print(result.transform.parent.parent.name);
                _onTerrainSelected?.Invoke(result);
            }
            
        }
        else UnitManager.instance.ClearSelection();
        /*else
        {
            
            //_selectedUnit.MovementFinished += ResetTurn;
            UnitManager.instance.ClearSelection();
        }*/
        
    }

    private bool FindTarget(Vector3 mousePos, out GameObject result)
    {
        RaycastHit _hit;
        Ray ray = _camera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out _hit, selectionMask))
        {
            result = _hit.collider.gameObject;
            return true;
        }

        result = null;
        return false;
    }

    private bool UnitSelected(GameObject result)
    {
        return result.GetComponent<Unit>() != null;
    }
    
    
    
    
    
    /*void HandleSelectableHit()
    {
        if (_selected != null)
        {
            DeselectCurrent();
            DisableNeighbours(_selected.GetComponent<HexCoordinates>().GetHexCoords());
        }

        _selected = hit.transform.parent.parent.gameObject;
        
        _selected.GetComponent<Hex>().Select();
        EnableNeighbours(_selected.GetComponent<HexCoordinates>().GetHexCoords());
    }

    void HandleNoHit()
    {
        if (_selected != null)
        {
            DeselectCurrent();
            DisableNeighbours(_selected.GetComponent<HexCoordinates>().GetHexCoords());
            _selected = null;
        }
    }

    void DeselectCurrent()
    {
        _selected.GetComponent<Hex>().Deselect();
    }
    

    void DisableNeighbours(Vector3Int coordinates)
    {
        HexGrid.instance.DisableNeighbours(coordinates);
    }

    void EnableNeighbours(Vector3Int coordinates)
    {
        HexGrid.instance.EnableNeighbours(coordinates);
    }*/
    
    


}

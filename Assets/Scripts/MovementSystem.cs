    using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    
    
    private static MovementSystem _instance;
    public static MovementSystem instance{
        get{
            if(_instance == null){
                Debug.Log("Mov System is Null!!!");
            }
            return _instance;
        }
    }


    private void Start()
    {

        _instance = this;
    }


    private BFSResult movementRange = new BFSResult();
    private List<Vector3Int> currentPath = new List<Vector3Int>();

    public void HideRange()
    {
        foreach (Vector3Int hexPosition in movementRange.GetRangePositions())
        {
            HexGrid.instance.GetTileAt(hexPosition).DisableHighlight();
        }

        movementRange = new BFSResult();
    }

    public void ShowRange(Unit selectedUnit)
    {
        CalculateRange(selectedUnit);

        Vector3Int unitPos = HexGrid.instance.GetClosestHex(selectedUnit.transform.position);

        foreach (Vector3Int hexPosition in movementRange.GetRangePositions())
        {
            if(unitPos == hexPosition) continue;
            HexGrid.instance.GetTileAt(hexPosition).EnableHighlight();
        }
        
    }

    public void CalculateRange(Unit selectedUnit)
    {
        movementRange = GraphSearch.BFSGetRange(HexGrid.instance.GetClosestHex(selectedUnit.transform.position),
            selectedUnit.MovementPoints);
    }


    public void ShowPath(Vector3Int selectedHexPosition)
    {
        if (movementRange.GetRangePositions().Contains(selectedHexPosition))
        {
            foreach (Vector3Int hexPosition in currentPath)
            {
                HexGrid.instance.GetTileAt(hexPosition).ResetHighlight();    
            }

            currentPath = movementRange.GetPathTo(selectedHexPosition);
            
            foreach (Vector3Int hexPosition in currentPath)
            {
                HexGrid.instance.GetTileAt(hexPosition).HighlightPath();
            }

        }
    }

    public void MoveUnit(Unit selectedUnit)
    {
        Debug.Log("movind unit " + selectedUnit.name);
        selectedUnit.MoveThroughPath(currentPath.Select(pos=> HexGrid.instance.GetTileAt(pos).transform.position).ToList());
    }

    public bool IsHexInRange(Vector3Int hexPosition)
    {
        return movementRange.isHexPositionInRange(hexPosition);
    }
    
    
    
}

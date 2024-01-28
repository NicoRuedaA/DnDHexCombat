using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    //public bool PlayersTurn { get; private set; } = true;
         //= IsMyTurn();

    [SerializeField] private Unit _selectedUnit;
    
    private Hex _previouslySelectedHex;

        
    private static UnitManager _instance;
    public static UnitManager instance{
        get{
            if(_instance == null){
                Debug.Log("Unit Manager is Null!!!");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private static bool IsMyTurn(GameObject unit)
    {
        return (GameManager.instance.TurnOwner == turnOwner.player && unit.GetComponent<Unit>().IsMyTurn());
    }

    public void HandleUnitSelected(GameObject unit)
    {
        //if(!PlayersTurn) return;
        Unit unitReference = unit.GetComponent<Unit>();
        if ((!IsMyTurn(unit)) || (CheckIfTheSameUnitSelected(unitReference))) return;
        
        if (!CanvasManager.instance.ButtonsAreEnabled())
        {
            
            CanvasManager.instance.EnableButtons();
            //_selectedUnit = unitReference;
            //unitReference.Select();
        }
        else
        {
            if (unitReference.IsAttacking()) PrepareForAttack(unitReference);
            else PrepareUnitForMovement(unitReference);    
        }
    }
    
    public void HandleTerrainSelect(GameObject hexGo)
    {
        //if(_selectedUnit == null || PlayersTurn == false) return;
        if(_selectedUnit == null || GameManager.instance.TurnOwner == turnOwner.IA) return;

        //print(hexGo.transform.parent.parent.name);
        Hex selectedHex = hexGo.GetComponentInParent<Hex>();
        
        
        if (HandleHexOutOfRange(selectedHex.HexCoords) || HandleSelectedHexIsUnitHex(selectedHex.HexCoords))
            //if (HandleSelectedHexIsUnitHex(selectedHex.GetHexCoords()))
            //if (HandleSelectedHexIsUnitHex(selectedHex.HexCoords))
        {
            return;
        }

        HandleTargetHexSelected(selectedHex);

    }
    
    

    private bool CheckIfTheSameUnitSelected(Unit unitReference)
    {

        //bool res = false;
        
        if (this._selectedUnit == unitReference)
        {
            ClearOldSelection();
            return true;
        }

        return false;
    }





    private void PrepareForAttack(Unit unitReference)
    {
        _selectedUnit.Attack(unitReference);
        if(_selectedUnit!=null) _selectedUnit.MovementFinished += ResetTurn;
        ClearOldSelection();
    }


    private void PrepareUnitForMovement(Unit unitReference)
    {
        if (this._selectedUnit != null)
        {
            ClearOldSelection();
        }
        //revisar si se vuelve a seleccionar
        this._selectedUnit = unitReference;
        this._selectedUnit.Select();
        MovementSystem.instance.ShowRange(this._selectedUnit);

    }

    public void ClearSelection()
    {
        if(_selectedUnit!=null) _selectedUnit.MovementFinished += ResetTurn;
        ClearOldSelection();
    }

    private void ClearOldSelection()
    {
        _previouslySelectedHex = null;
        Debug.Log("pre error");

        if (_selectedUnit != null)
        {
            _selectedUnit.Deselect();
            MovementSystem.instance.HideRange();
            _selectedUnit = null;
        }
        CanvasManager.instance.DesableButtons();
        
    }
    
    
    //movement
    private void HandleTargetHexSelected(Hex selectedHex)
    {   
        //si no hemos clickado anteriormente
        if (_previouslySelectedHex == null || _previouslySelectedHex != selectedHex)
        {
            //mostramos el camino resaltado
            _previouslySelectedHex = selectedHex;
            MovementSystem.instance.ShowPath(selectedHex.HexCoords);
        }
        else
        {
            MovementSystem.instance.MoveUnit(_selectedUnit);
            GameManager.instance.TurnOwner = turnOwner.IA;
            if(_selectedUnit!=null) _selectedUnit.MovementFinished += ResetTurn;
            ClearOldSelection();
        }
    }
    
    
    private bool HandleSelectedHexIsUnitHex(Vector3Int hexPosition)
    {
        //Solo comprobar si es la Hex del personaje
        /*if (hexPosition == HexGrid.instance.GetClosestHex(_selectedUnit.transform.position))
        {
            _selectedUnit.Deselect();
            ClearOldSelection();
            return true;
        }
        return false;*/

        foreach (var character in CombatManager.instance.CharactersList)
        {
            if (hexPosition == HexGrid.instance.GetClosestHex(character.transform.position))
            {
                _selectedUnit.Deselect();
                ClearOldSelection();
                return true;
                //hacer animacion roja en la casilla seleccionada
            }
        }
        return false;
    }
    
    private  bool HandleHexOutOfRange(Vector3Int hexPosition)
    {
        if (MovementSystem.instance.IsHexInRange(hexPosition) == false)
        {
            Debug.Log("Hex Out of Range!");
            CanvasManager.instance.DesableButtons();
            return true;
        }

        return false;
    }

    private void ResetTurn(Unit selectedUnit)
    {
        if(_selectedUnit!=null) _selectedUnit.MovementFinished += ResetTurn;
        GameManager.instance.TurnOwner = turnOwner.player;
    }
}

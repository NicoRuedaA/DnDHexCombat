using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    //private Dictionary<GameObject, int> _charactersInCombat = new Dictionary<GameObject, int>();
    //private GameObject[] _charactersInCombat;
    private List<Unit> _charactersList = new List<Unit>();
    private List<Unit> _turnOrderedList = new List<Unit>();

    private Unit _turnCharacter;
    private int _turnNumber=0;

    public List<Unit> CharactersList => _charactersList;

    public List<Unit> TurnList => _turnOrderedList;

    public int TurnNumber()
    {
        return _turnNumber;
    }
    public Unit CharacterTurn => _turnCharacter;

    
    private static CombatManager _instance;
    public static CombatManager instance{
        get{
            if(_instance == null){
                Debug.Log("Game Manager is Null!!!");
            }
            return _instance;
        }
    }
    
    void Start()
    {
        _instance = this;
        _turnNumber = 0;

        //CREO LA LISTA
        foreach (GameObject character in GameObject.FindGameObjectsWithTag("character"))
        {
            _charactersList.Add(character.GetComponent<Unit>());
        }
        
        //TIRO DADOS DE CADA ELEMENTO EN LA LISTA
        foreach (var character in _charactersList)
        {
            _turnOrderedList.Add(character);
            character.RollDices(playerStats.initative);
            /*se puede cambiar por pasar el valor de al variable como GameObject.variable y simplificar el codigo llamado
            initiativeResult = character.GetComponent<Unit>().RollDices(playerStats.initative);
            _charactersInCombat.Add(character, initiativeResult);
            _turnOrder.Add(character);*/
        }
        
        
        _turnOrderedList.Sort((a, b) => b.InitiativeResult.CompareTo(a.InitiativeResult));
        _turnCharacter = _turnOrderedList[_turnNumber];
        Debug.Log($"New turn {_turnNumber} : {_turnCharacter}");
        
        //solo para debug
        foreach (var VARIABLE in _turnOrderedList)
        {
            print(VARIABLE.transform.name);
        }
        
        CanvasManager.instance.DesableButtons();

    }
    
    
    public void NextTurn()
    {
        Debug.Log($"Actual turn {_turnNumber} : {_turnCharacter}");
        _turnNumber++;
        if (_turnNumber >= _turnOrderedList.Count) _turnNumber = 0;
        _turnCharacter = _turnOrderedList[_turnNumber];
        Debug.Log($"New turn {_turnNumber} : {_turnCharacter}");


    }


    
}


//Indica si puedes mover el player o está en medio de una animacion
public enum playerStats
{
    initative,
}


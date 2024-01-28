using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public turnOwner TurnOwner;
    private static GameManager _instance;
    public static GameManager instance{
        get{
            if(_instance == null){
                Debug.Log("Game Manager is Null!!!");
            }
            return _instance;
        }
    }
    
    private void Awake()
    {
        _instance = this;
        TurnOwner = turnOwner.player;


    }
    
}

public enum turnOwner
{
    player,
    IA,
}

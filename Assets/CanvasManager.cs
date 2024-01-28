using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    private GameObject _buttonParent;
    private List<GameObject> _buttonsList = new List<GameObject>();
    
    private static CanvasManager _instance;
    public static CanvasManager instance{
        get{
            if(_instance == null){
                Debug.Log("Canvas Manager is Null!!!");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        _buttonParent = transform.GetChild(0).gameObject;
        foreach (Transform child in _buttonParent.transform)
        {
            _buttonsList.Add(child.gameObject);
        }
        
        

    }

    private void Start()
    {
        buttonsState = true;
        DesableButtons();
    }

    private bool buttonsState = false;
    
    public void EnableButtons()
    {
        if (!buttonsState)
        {
            buttonsState = true;
            foreach (var buttons in _buttonsList)
            {
                buttons.SetActive(true);
            }
        }
    }
    public void DesableButtons()
    {
        if (buttonsState)
        {
            buttonsState = false;
            foreach (var buttons in _buttonsList)
            {
                buttons.SetActive(false);
            }
        }
    }

    public bool ButtonsAreEnabled() => buttonsState;
}

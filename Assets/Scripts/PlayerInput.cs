using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
        public UnityEvent<Vector3> PointerClick;
        private static PlayerInput _instance;
        public static PlayerInput instance{
                get{
                        if(_instance == null){
                                Debug.Log("Input Player is Null!!!");
                        }
                        return _instance;
                }
        } 


        private void Awake()
        {
                _instance = this;

        }

        void Update()
        {
                if (Input.GetMouseButtonDown(0))
                {
                        //SelectionManager.instance.Prueba();
                        LeftClick();
                        //SelectionManager.instance.SelectObject(Input.mousePosition);
                }
                
                if (Input.GetMouseButtonDown(1))
                {
                        //SelectionManager.instance.Prueba();
                        RightClick();
                        //SelectionManager.instance.SelectObject(Input.mousePosition);
                }
                
                //DetectMouseClick();
        }


        private void LeftClick()
        {
                Vector3 mousePos = Input.mousePosition;
                PointerClick?.Invoke(mousePos);
        }


        private void RightClick()
        {
                print("click derecho");
        }

}

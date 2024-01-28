using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    //public Transform Camera;
    private Camera _mainCamera;
    
    private void Start()
    {
        _mainCamera = UnityEngine.Camera.main;
        
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _mainCamera.transform.forward);
    }
}

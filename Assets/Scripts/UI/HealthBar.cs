using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Unit _unit;
    public Slider healthSlider;
    public int MAX_HEALTH;
    public int health;

    private void Awake()
    {
        _unit = GetComponentInParent<Unit>();
    }

    private void Start()
    {
        MAX_HEALTH = _unit.MAX_HEALTH;
        health = MAX_HEALTH;
    }
    

    public void UpdateHealth()
    {
        healthSlider.value = _unit.Health;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;

public class WallAlertUI : MonoBehaviour
{
    public GameObject alertWall;
    private TextMeshProUGUI alertWallText;

    void OnEnable()
    {
        EventManager.Wall.OnDisplayText += setActiveAlertWall;
    }

    void OnDisable()
    {
        EventManager.Wall.OnDisplayText -= setActiveAlertWall;
    }


    public void Start()
    {
        alertWallText = alertWall.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void setActiveAlertWall(int module, bool state)
    {
        SelectionManager.instance.isInteracting = state;
        Debug.Log("Current wall state is " + state);
        alertWall.SetActive(state);
        alertWallText.text = "No puedes pasar sin antes completar todas las tareas del módulo " + module;
    }  
}

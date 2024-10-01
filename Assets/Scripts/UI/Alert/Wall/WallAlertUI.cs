using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;

public class WallAlertUI : MonoBehaviour
{
    public GameObject alertWall;
    private TextMeshProUGUI alertWallText;
    public static event Action<int, bool> OnDisplayText;

    public static void DisplayText(int module, bool state)
    {
        if (OnDisplayText != null)
        {
            OnDisplayText.Invoke(module, state);
        }
    }

    void OnEnable()
    {
        OnDisplayText += setActiveAlertWall;
    }

    void OnDisable()
    {
        OnDisplayText -= setActiveAlertWall;
    }


    public void Start()
    {
        alertWallText = alertWall.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void setActiveAlertWall(int module, bool state)
    {
        alertWallText.text = "No puedes pasar sin antes completar todas las tareas del módulo " + module;
        alertWall.SetActive(state);
    }  
}

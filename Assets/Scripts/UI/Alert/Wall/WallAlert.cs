using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;


public class WallAlert : MonoBehaviour
{
    [SerializeField]
    private GameObject moduleWalls;
    [SerializeField]
    private WallAlertUI wallAlertUI;
    private List<GameObject> walls;


    public void OnEnable()
    {
        EventManager.Wall.OnDisabeWall += HandleDisableWall;
    }

    public void OnDisable()
    {
       EventManager.Wall.OnDisabeWall -= HandleDisableWall;
    }

    void Start()
    {
        walls = new List<GameObject>();
        if (moduleWalls != null)
        {
            for (int i = 0; i < moduleWalls.transform.childCount; i++)
            {
                walls.Add(moduleWalls.transform.GetChild(i).gameObject);
                moduleWalls.transform.GetChild(i).gameObject.AddComponent<Wall>();
                moduleWalls.transform.GetChild(i).AddComponent<BoxCollider>();
                moduleWalls.transform.GetChild(i).GetComponent<BoxCollider>().isTrigger = true;
                moduleWalls.transform.GetChild(i).GetComponent<BoxCollider>().size = new Vector3(20, 1, 20);
                moduleWalls.transform.GetChild(i).gameObject.GetComponent<Wall>().module = i+1;
            }
        }
    }

    public void HandleDisableWall(int module)
    {
        if(module <= walls.Count)
        {
            walls[module - 1].gameObject.SetActive(false);
            // Hiding message text after wall has been disabled
            EventManager.Wall.OnDisplayText(0, false);

        }
    }

}
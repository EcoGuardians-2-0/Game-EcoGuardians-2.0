using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public int module;

    public Wall(int module)
    {
        this.module = module;
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Contacting character");
        if (other.CompareTag("Player"))
            EventManager.Wall.OnDisplayText(module, true);
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("No longer Contacting character");
        if (other.CompareTag("Player"))
            EventManager.Wall.OnDisplayText(module, false);
    }

}

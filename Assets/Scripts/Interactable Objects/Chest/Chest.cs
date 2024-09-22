using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableObject
{
    override
    public void Interact()
    {
        Debug.Log("Interactuando con " + itemName);
    }
}

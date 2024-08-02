using UnityEngine;

public class Door : InteractableObject
{
    override
    public void Interact()
    {
        Debug.Log("Interactuando con " + itemName);
    }
}

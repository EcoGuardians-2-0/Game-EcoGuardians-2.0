using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{

    [SerializeField]
    protected string selectionPrompt;

    [SerializeField]
    protected string itemName;

    public bool playerInRange;

    public string GetSelectionPrompt()
    {
        return selectionPrompt;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public abstract void Interact();
    
}
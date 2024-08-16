using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{

    [SerializeField]
    protected string selectionPrompt;

    [SerializeField]
    protected string itemName;

    [SerializeField]
    protected Color customColor = Color.yellow;

    protected Outline outline;

    protected bool interacting;

    public bool playerInRange;
    
    protected virtual void Start()
    {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.enabled = false;
        setOutline(customColor, 10f);
    }

    public void setOutline(Color color, float width)
    {
        outline.OutlineColor = color;
        outline.OutlineWidth = width;
    }

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

    public virtual void Interact()
    {
        interacting = !interacting;
        SelectionManager.instance.isInteracting = interacting;
    }

}
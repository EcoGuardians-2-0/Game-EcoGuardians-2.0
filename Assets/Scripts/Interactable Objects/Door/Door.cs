using UnityEngine;

public class Door : InteractableObject
{
    private Animator doorAnimation;
    private bool active;

    public void Start()
    {
        doorAnimation = GetComponent<Animator>();
        selectionPrompt = "Abrir puerta";
        itemName = "Puerta";
    }

    override public void Interact()
    {
        active = !active;
        doorAnimation.SetBool("door", active);

        if (active)
            selectionPrompt = "Cerrar puerta";
        else
            selectionPrompt = "Abrir puerta";
    }
}

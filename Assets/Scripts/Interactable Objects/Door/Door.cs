using UnityEngine;

public class Door : InteractableObject
{
    private Animator doorAnimation;
    private bool active;

    public new void Start()
    {
        base.Start(); // Do not remove - child calls parent method
        doorAnimation = GetComponent<Animator>();
        selectionPrompt = "Abrir puerta";
        itemName = "Puerta";
    }

    public override void Interact()
    {
        active = !active;

        if (active)
        {
            doorAnimation.Play("DoorOpen", 0, 0.0f);
            selectionPrompt = "Cerrar puerta";
        }
        else
        {
            doorAnimation.Play("DoorClose", 0, 0.0f);
            selectionPrompt = "Abrir puerta";
        }

    }
}

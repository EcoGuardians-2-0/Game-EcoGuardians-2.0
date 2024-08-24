using UnityEngine;

public class Door : InteractableObject
{
    private Animator doorAnimation;
    private bool active = false;

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
            doorAnimation.SetBool("door", active);
            selectionPrompt = "Cerrar puerta";
        }
        else
        {
            doorAnimation.SetBool("door", active);
            selectionPrompt = "Abrir puerta";
        }

    }
}

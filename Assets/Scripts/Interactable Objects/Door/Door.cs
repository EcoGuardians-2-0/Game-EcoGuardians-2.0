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
        base.Interact(); // Do not remove - child calls parent method

        active = !active;
        doorAnimation.SetBool("door", active);

        if (active)
            selectionPrompt = "Cerrar puerta";
        else
            selectionPrompt = "Abrir puerta";
    }
}

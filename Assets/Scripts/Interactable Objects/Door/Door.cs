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

            // Play door sound depending on the door type based on the tag
            
            // Tag: DoorGlass
            if (gameObject.CompareTag("DoorGlass"))
            {
                AudioManager.Instance.PlaySound(SoundType.doorClosedGlass);
            }
            // Tag: DoorWood
            else if (gameObject.CompareTag("DoorWood"))
            {
                AudioManager.Instance.PlaySound(SoundType.doorClosedWood);
            }
            else
            {
                AudioManager.Instance.PlaySound(SoundType.doorClosedWood);
            }
        }
        else
        {
            doorAnimation.SetBool("door", active);
            selectionPrompt = "Abrir puerta";

            // Play door sound depending on the door type based on the tag

            // Tag: DoorGlass
            if (gameObject.CompareTag("DoorGlass"))
            {
                AudioManager.Instance.PlaySound(SoundType.doorOpenGlass);
            }
            // Tag: DoorWood
            else if (gameObject.CompareTag("DoorWood"))
            {
                AudioManager.Instance.PlaySound(SoundType.doorOpenWood);
            }
            else
            {
                AudioManager.Instance.PlaySound(SoundType.doorOpenWood);
            }
        }
    }
}

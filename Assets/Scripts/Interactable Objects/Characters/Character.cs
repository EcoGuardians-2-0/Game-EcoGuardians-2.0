using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : InteractableObject
{
    [Header("INK JSON")]
    [SerializeField]
    private TextAsset inkJSON;

    Animator animator;
    public bool ikActive = false;
    public Transform objTarget;

    new void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }
    public override void Interact()
    {

        if (!DialogueManager.instance.isTalking)
        {
            SelectionManager.instance.isInteracting = true;
            DisableObjects.Instance.disableCharacterController();
            DisableObjects.Instance.disableCameras();
            DisableObjects.Instance.disableSwitchCamera();
            DialogueManager.instance.StartConversation(inkJSON);
        }

    }

    protected override void handleCollision(Collider other)
    {
        string name = other.gameObject.name;
        Debug.Log("Game Object: " + name);
    }

    private void OnAnimatorIK()
    {
        if (animator)
        {
            if (ikActive)
            {
                if (objTarget != null && playerInRange)
                {
                    Debug.Log("Hello there");
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(objTarget.position);
                }
            }
        }
        else
        {
            animator.SetLookAtWeight(0);
        }
    }

}

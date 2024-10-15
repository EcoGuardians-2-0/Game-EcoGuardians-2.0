using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : InteractableObject
{
    [Header("INK JSON")]
    [SerializeField]
    private TextAsset inkJSON;

    private Animator animator;
    private Transform objTarget;
    private Quaternion originalRotation;
    public bool ikActive = false;
    private float lookWeight;

    new void Start()
    {
        base.Start();
        originalRotation = transform.rotation;
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (playerInRange)
        {
            lookWeight = Mathf.Lerp(lookWeight, 1, Time.deltaTime * 2.5f);

            if (objTarget != null)
            {
                Vector3 direction = objTarget.position - transform.position;
                direction.y = 0f;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 2f);
            }
        }
        else
        {
            lookWeight = Mathf.Lerp(lookWeight, 0, Time.deltaTime * 1.5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, Time.deltaTime * 1.5f);
        }
    }

    public override void Interact()
    {

        if (!DialogueManager.instance.isTalking)
        {
            DialogueManager.instance.StartConversation(inkJSON);

            //CameraUtilityManager.Instance.SetCameraOnCharacter(transform);
        }

    }

    protected override void handleCollision(Collider other)
    {
        string name = other.gameObject.name;
        objTarget= other.GetComponentsInChildren<Transform>()[1];
    }

    private void OnAnimatorIK()
    {
        if (animator)
        {
            if (ikActive)
            {
                if (objTarget != null)
                {
                    animator.SetLookAtWeight(lookWeight);
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

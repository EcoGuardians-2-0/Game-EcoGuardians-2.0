using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLook : MonoBehaviour
{
    Animator animator;
    public bool ikActive = false;
    public Transform objTarget;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAnimatorIK()
    {
        if (animator)
        {
            if (ikActive)
            {
                if(objTarget != null)
                {
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

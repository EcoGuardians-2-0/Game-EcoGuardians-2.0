using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKManager : MonoBehaviour
{
    Animator animator;
    public bool ikActive = false;
    public Transform objTarget;
    public bool isInRange;

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
                if(objTarget != null && isInRange)
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

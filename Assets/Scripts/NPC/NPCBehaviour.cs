using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private float _timeUntilBored;

    [SerializeField]
    private int _numberOfBoredAnimations;

    private bool isTalking;
    private float _dampingTime = 0.6f;
    private int _boredAnimation;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isTalking)
        {
            string name = animator.GetComponentInParent<Character>().getItemName();
            if (DialogueManager.instance.canPlay && name == DialogueManager.instance.currentSpeaker)
            {
                isTalking = true;
                _boredAnimation = DialogueManager.instance.currentAnimation;
                animator.SetFloat("Blend", _boredAnimation - 1);
            }
        }
        else if(DialogueManager.instance.hasFinished)
        {
            if (DialogueManager.instance.hasSkipped || stateInfo.normalizedTime % 1 > 0.98)
            {
                ResetIdle();
            }
        }

        animator.SetFloat("Blend", _boredAnimation, _dampingTime, Time.deltaTime);
    }

    private void ResetIdle()
    {
        if (isTalking)
        {
           _boredAnimation--;
        }

        isTalking = false;
    }
}

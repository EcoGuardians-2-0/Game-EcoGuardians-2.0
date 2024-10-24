using UnityEngine;

public class StepCall : MonoBehaviour
{
    // Cooldown for walking steps
    private float stepCooldown = 0.1f;

    // Cooldown for running steps
    private float stepRunCooldown = 0.08f; 

    
    private float lastStepTime;
    private float lastStepRunTime;

    // Walking animation event Step
    public void Step()
    {
        if (Time.time - lastStepTime >= stepCooldown)
        {
            //Debug.Log("Step1");
            AudioManager.Instance.PlayFootstepSound(false);
            lastStepTime = Time.time; 
        }
    }

    public void StepRun()
    {
        if (Time.time - lastStepRunTime >= stepRunCooldown)
        {
            AudioManager.Instance.PlayFootstepSound(true);
            lastStepRunTime = Time.time;
        }
    }
}

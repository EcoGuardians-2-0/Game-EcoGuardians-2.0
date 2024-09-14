using UnityEngine;

public class StepCall : MonoBehaviour
{
    private float stepCooldown = 0.2f; // Cooldown for walking steps
    private float stepRunCooldown = 0.15f; // Cooldown for running steps
    private float lastStepTime;
    private float lastStepRunTime;

    // Walking animation event Step
    public void Step()
    {
        if (Time.time - lastStepTime >= stepCooldown)
        {
            Debug.Log("Step1");
            AudioManager.Instance.PlayFootstepSound(false);
            lastStepTime = Time.time; // Update the last step time for walking
        }
    }

    public void StepRun()
    {
        if (Time.time - lastStepRunTime >= stepRunCooldown)
        {
            Debug.Log("StepRun1");
            AudioManager.Instance.PlayFootstepSound(true);
            lastStepRunTime = Time.time; // Update the last step time for running
        }
    }
}

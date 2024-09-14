using UnityEngine;

public class StepCall : MonoBehaviour
{
    private float lastStepTime = 0f;
    private float stepCooldown = 0.2f; // Adjust the cooldown duration as needed

    // Walking animation event Step
    public void Step()
    {
        if (Time.time - lastStepTime >= stepCooldown)
        {
            lastStepTime = Time.time;
            Debug.Log("Step1");
            AudioManager.Instance.PlayFootstepSound(false);
        }
    }

    public void StepRun()
    {
        if (Time.time - lastStepTime >= stepCooldown)
        {
            lastStepTime = Time.time;
            Debug.Log("StepRun1");
            AudioManager.Instance.PlayFootstepSound(true);
        }
    }
}
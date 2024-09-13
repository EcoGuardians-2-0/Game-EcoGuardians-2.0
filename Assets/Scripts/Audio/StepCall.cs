using UnityEngine;

public class StepCall : MonoBehaviour
{
    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Walking animation event Step
    public void Step()
    {
        bool isRunning = playerController.IsRunning;
        AudioManager.Instance.PlayFootstepSound(isRunning);
    }

    public void StepRun()
    {
        AudioManager.Instance.PlayFootstepSound(true);
    }
}
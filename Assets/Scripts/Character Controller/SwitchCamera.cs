using UnityEngine;
using Cinemachine;

public class SwitchCamera : MonoBehaviour
{
    [Header("Camera to assign")]
    public CinemachineVirtualCamera FirstCam;
    public CinemachineVirtualCamera ThirdCam;

    [Header("Sensitivity Settings")]
    private float minSensitivity = 0f; // Adjusted min sensitivity
    private float maxSensitivity = 1000f; // Adjusted max sensitivity
    private float defaultSensitivity = 300f; // Default sensitivity

    private bool thirdActive = true;
    private float lastAppliedSensitivity;

    private void Start()
    {
        // Set the default sens when the game starts
        float currentSavedSensitivity = PlayerPrefs.GetFloat("masterSen", defaultSensitivity);
        PlayerPrefs.SetFloat("masterSen", currentSavedSensitivity);

        UpdateCameraSensitivity();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            thirdActive = !thirdActive;
        }

        UpdateActiveCamera();

        float currentSavedSensitivity = PlayerPrefs.GetFloat("masterSen", defaultSensitivity);
        if (currentSavedSensitivity != lastAppliedSensitivity)
        {
            Debug.Log($"Sensitivity changed from {lastAppliedSensitivity} to {currentSavedSensitivity}");
            UpdateCameraSensitivity();
        }
    }

    void UpdateActiveCamera()
    {
        if (thirdActive)
        {
            FirstCam.Priority = 10;
            ThirdCam.Priority = 11;
            UpdateRecentering(ThirdCam);
        }
        else
        {
            ThirdCam.Priority = 10;
            FirstCam.Priority = 11;
            UpdateRecentering(FirstCam);
        }
    }

    void UpdateRecentering(CinemachineVirtualCamera camera)
    {
        bool recentering = Input.GetAxis("CameraRecentre") == 1;
        var pov = camera.GetCinemachineComponent<CinemachinePOV>();
        pov.m_VerticalRecentering.m_enabled = recentering;
        pov.m_HorizontalRecentering.m_enabled = recentering;
    }

    void UpdateCameraSensitivity()
    {
        float savedSensitivity = PlayerPrefs.GetFloat("masterSen", defaultSensitivity);

        float appliedSensitivity = Mathf.Clamp(savedSensitivity, minSensitivity, maxSensitivity);

        // Update First Person Camera sensitivity
        var firstPersonPOV = FirstCam.GetCinemachineComponent<CinemachinePOV>();
        firstPersonPOV.m_HorizontalAxis.m_MaxSpeed = appliedSensitivity;
        firstPersonPOV.m_VerticalAxis.m_MaxSpeed = appliedSensitivity;

        // Update Third Person Camera sensitivity
        var thirdPersonPOV = ThirdCam.GetCinemachineComponent<CinemachinePOV>();
        thirdPersonPOV.m_HorizontalAxis.m_MaxSpeed = appliedSensitivity;
        thirdPersonPOV.m_VerticalAxis.m_MaxSpeed = appliedSensitivity;

        lastAppliedSensitivity = savedSensitivity;
    }
}
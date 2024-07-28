using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class SwitchCamera : MonoBehaviour
{
    [Header("Camera to assign")]
    public CinemachineVirtualCamera FirstCam;
    public CinemachineVirtualCamera ThirdCam;

    private bool thirdActive = true;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            thirdActive = !thirdActive;
        }

        if (thirdActive)
        {
            FirstCam.Priority = 10;
            ThirdCam.Priority = 11;
            if (Input.GetAxis("CameraRecentre") == 1)
            {
                ThirdCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalRecentering.m_enabled = true;
                ThirdCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalRecentering.m_enabled = true;
            }
            else
            {
                ThirdCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalRecentering.m_enabled = false;
                ThirdCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalRecentering.m_enabled = false;
            }
        }
        else
        {
            ThirdCam.Priority = 10;
            FirstCam.Priority = 11;
            if (Input.GetAxis("CameraRecentre") == 1)
            {
                FirstCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalRecentering.m_enabled = true;
                FirstCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalRecentering.m_enabled = true;
            }
            else
            {
                FirstCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalRecentering.m_enabled = false;
                FirstCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalRecentering.m_enabled = false;
            }
        }
    }
}

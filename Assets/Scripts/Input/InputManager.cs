using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            EventManager.CameraView.OnChangeCameraView?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            EventManager.Minimap.OnDisplayMinimap?.Invoke();
        }
    }
}


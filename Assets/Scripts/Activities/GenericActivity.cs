using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericActivity : MonoBehaviour
{
    void OnEnable()
    {
        EventManager.Photograph.OnActiveCamera(false);
        EventManager.Pause.OnUnpause += HandleOnUnPause;
    }

    void OnDisable()
    {
        EventManager.Photograph.OnActiveCamera(true);
        EventManager.Pause.OnUnpause -= HandleOnUnPause;
    }

    void HandleOnUnPause()
    {
        if (gameObject.activeSelf)
        {
            DisableObjects.Instance.showCursor();
        }
    }
}

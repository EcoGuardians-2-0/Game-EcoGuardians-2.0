using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericActivity : MonoBehaviour
{
    void OnEnable()
    {
        EventManager.Pause.OnUnpause += HandleOnUnPause;
    }

    void OnDisable()
    {
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

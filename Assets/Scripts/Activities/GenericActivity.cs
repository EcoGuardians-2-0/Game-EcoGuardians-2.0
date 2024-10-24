using UnityEngine;

public class GenericActivity : MonoBehaviour
{
    void OnEnable()
    {
        EventManager.Minimap.OnLockMiniMap.Invoke();
        EventManager.Photograph.OnActiveCamera(false);
        EventManager.Pause.OnUnpause += HandleOnUnPause;
    }

    void OnDisable()
    {
        EventManager.Minimap.OnUnlockMiniMap.Invoke();
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

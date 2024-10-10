using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    [SerializeField]
    private GameObject miniMap;

    private bool isActive;
    private bool isLocked;


    void Start()
    {
        isActive = false;
        isLocked = true;
        toggleMinimap();
    }

    private void OnEnable()
    {
        EventManager.Minimap.OnDisplayMinimap += HandleOnDisplayMinimap;
        EventManager.Minimap.OnLockMiniMap += HandleOnLockMinimap;
        EventManager.Minimap.OnUnlockMiniMap += HandleOnUnlockMiniMap;
    }

    private void OnDisable()
    {
        EventManager.Minimap.OnDisplayMinimap -= HandleOnDisplayMinimap;
        EventManager.Minimap.OnLockMiniMap -= HandleOnLockMinimap;
        EventManager.Minimap.OnUnlockMiniMap -= HandleOnUnlockMiniMap;
    }

    public void HandleOnLockMinimap()
    {
        isLocked = true;
        if (isActive)
        {
            isActive = false;
            toggleMinimap();
        }
    }
    public void HandleOnUnlockMiniMap()
    {
        isLocked = false;
        HandleOnDisplayMinimap();
    }

    public void HandleOnDisplayMinimap()
    {
        if (!isLocked)
        {
            isActive = !isActive;
            toggleMinimap();
        }
    }

    public void toggleMinimap()
    {
        if(miniMap != null && miniMap.GetComponent<Canvas>() != null)
            miniMap.GetComponent<Canvas>().enabled = isActive;
    }

}

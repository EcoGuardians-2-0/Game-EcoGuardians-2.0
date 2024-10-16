using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    [SerializeField]
    private GameObject miniMap;

    private bool isActive;
    private bool isLocked;
    private bool generalLock;


    void Start()
    {
        isActive = false;
        isLocked = false;
        generalLock = true;
        toggleMinimap();
    }

    private void OnEnable()
    {
        EventManager.Minimap.OnDisplayMinimap += HandleOnDisplayMinimap;
        EventManager.Minimap.OnLockMiniMap += HandleOnLockMinimap;
        EventManager.Minimap.OnUnlockMiniMap += HandleOnUnlockMiniMap;
        EventManager.Minimap.OnGeneralUnlockMiniMap += HandleGeneralUnlockMinimap;
    }

    private void OnDisable()
    {
        EventManager.Minimap.OnDisplayMinimap -= HandleOnDisplayMinimap;
        EventManager.Minimap.OnLockMiniMap -= HandleOnLockMinimap;
        EventManager.Minimap.OnUnlockMiniMap -= HandleOnUnlockMiniMap;
        EventManager.Minimap.OnGeneralUnlockMiniMap -= HandleGeneralUnlockMinimap;

    }

    public void HandleGeneralUnlockMinimap()
    {
        generalLock = false;
        HandleOnDisplayMinimap();
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
        if (!isActive)
        {
            HandleOnDisplayMinimap();
        }
    }

    public void HandleOnDisplayMinimap()
    {
        if (!isLocked && !generalLock)
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

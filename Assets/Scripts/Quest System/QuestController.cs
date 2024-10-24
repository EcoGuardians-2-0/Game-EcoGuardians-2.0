using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    private bool isActive;
    private bool isLocked;
    private bool generalLock;
    void Start()
    {
        isActive = false;
        isLocked = false;
        generalLock = true;
    }

    private void OnEnable()
    {
        EventManager.QuestUI.OnDisplayQuestUI += HandleDisplayQuestUI;
        EventManager.QuestUI.OnUnlockQuestUI += HandleUnlockQuestUI;
        EventManager.QuestUI.OnLockQuestUI += HandleLockQuestUI;
        EventManager.QuestUI.OnGeneralUnlockQuestUI += HandleGeneralUnlock;
    }

    private void OnDisable()
    {
        EventManager.QuestUI.OnDisplayQuestUI -= HandleDisplayQuestUI;
        EventManager.QuestUI.OnUnlockQuestUI -= HandleUnlockQuestUI;
        EventManager.QuestUI.OnLockQuestUI -= HandleLockQuestUI;
        EventManager.QuestUI.OnGeneralUnlockQuestUI -= HandleGeneralUnlock;
    }

    private void HandleGeneralUnlock()
    {
        generalLock = false;
        HandleDisplayQuestUI();
    }
    private void HandleDisplayQuestUI()
    {
        if (!isLocked && !generalLock)
        {
            isActive = !isActive;
            toggleQuestUI();
        }
    }

    private void HandleUnlockQuestUI()
    {
        isLocked = false;
        if (!isActive)
        {
            HandleDisplayQuestUI();
        }
    }

    private void HandleLockQuestUI()
    {
        isLocked = true;
        if (isActive)
        {
            isActive = false;
            toggleQuestUI();
        }

    }

    private void toggleQuestUI()
    {
        if (isActive)
        {
            AnimationsQuest.Instance.ShowQuestsUI();
        }
        else
        {
            AnimationsQuest.Instance.HideQuestsUI();
        }
    }
}

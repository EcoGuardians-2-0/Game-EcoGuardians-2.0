using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsQuest : MonoBehaviour
{
    // Singleton
    public static AnimationsQuest Instance { get; private set; }

    // Variables

    [SerializeField]
    private GameObject questsUI;

    private bool questsState;

    private float firstXPositionQuests = 0f;
    private float secondXPositionQuests = 560f;

    // Getters and setters
    public bool GetQuestsState(){ return questsState; }
    public void SetQuestsState(bool state){ questsState = state; }

    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        questsState = false;
        secondXPositionQuests *= questsUI.GetComponent<RectTransform>().localScale.x;
    }

    // Show the quests UI
    public void ShowQuestsUI()
    {
        questsUI.SetActive(true);
        RectTransform rectTransform = questsUI.GetComponent<RectTransform>();

        // Cancel any ongoing animations
        if (LeanTween.isTweening(rectTransform))
            LeanTween.cancel(rectTransform);

        LeanTween.moveX(rectTransform, secondXPositionQuests, 1f).setEase(LeanTweenType.easeOutQuint);
        questsState = true;
    }

    // Hide the quests UI
    public void HideQuestsUI()
    {
        RectTransform rectTransform = questsUI.GetComponent<RectTransform>();

        // Cancel any ongoing animations
        if (LeanTween.isTweening(rectTransform))
            LeanTween.cancel(rectTransform);

        LeanTween.moveX(rectTransform, firstXPositionQuests, 1f).setEase(LeanTweenType.easeOutQuint).setOnComplete(() => questsUI.SetActive(false));
        questsState = false;
    }

    public void ShowQuestsUI(GameObject quest)
    {
        questsUI.SetActive(true);
        RectTransform rectTransform = questsUI.GetComponent<RectTransform>();

        // Cancel any ongoing animations
        if (LeanTween.isTweening(rectTransform))
            LeanTween.cancel(rectTransform);

        LeanTween.moveX(rectTransform, secondXPositionQuests, 0.35f).setEase(LeanTweenType.easeInSine).setOnComplete(() => StartQuestAnimation(quest));
        questsState = true;
    }

    public void ShowQuestAnimation(GameObject quest)
    {
        if (!questsState)
            ShowQuestsUI(quest);
        else
            StartQuestAnimation(quest);
    }

    private void StartQuestAnimation(GameObject quest)
    {
        RectTransform rectTransform = quest.GetComponent<RectTransform>();

        float xDefault = rectTransform.anchoredPosition.x;

        LeanTween.moveX(rectTransform, xDefault + 25f, 0.5f).setEase(LeanTweenType.easeOutQuint);
        LeanTween.moveX(rectTransform, xDefault, 0.5f).setDelay(0.5f).setEase(LeanTweenType.easeOutQuint);
        LeanTween.scale(quest, new Vector3(1.1f, 1.1f, 1f), 0.5f).setEase(LeanTweenType.easeOutQuint);
        LeanTween.scale(quest, new Vector3(1f, 1f, 1f), 0.5f).setDelay(0.5f).setEase(LeanTweenType.easeOutQuint);
    }

    public void DeleteQuestAnimation(GameObject quest)
    {
        LeanTween.scale(quest, new Vector3(0f, 0f, 0f), 0.3f).setEase(LeanTweenType.easeOutQuint).setOnComplete(() => Destroy(quest));
    }

    public void CompleteQuestAnimation(GameObject checkBox)
    {
        LeanTween.scale(checkBox, new Vector3(1.5f, 1.5f, 1.5f), 0.5f).setEase(LeanTweenType.easeOutQuint);
        LeanTween.scale(checkBox, new Vector3(1f, 1f, 1f), 0.5f).setDelay(0.5f).setEase(LeanTweenType.easeOutQuint);
    }
}

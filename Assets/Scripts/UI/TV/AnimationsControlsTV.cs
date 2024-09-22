using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsControlsTV : MonoBehaviour
{
    public static AnimationsControlsTV Instance { get; private set; }

    [SerializeField]
    private GameObject controlsImageTVUI;

    [SerializeField]
    private GameObject controlsVideoTVUI;

    private bool imageState;
    private bool videoState;

    private float firstXPositionImage = -754f;
    private float secondXPositionImage = 0f;
    private float firstXPositionVideo = -1164f;
    private float secondXPositionVideo = 0f;

    // Getters
    public bool GetImageState(){ return imageState; }
    public bool GetVideoState(){ return videoState; }

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    // Show the controls image TV UI
    public void ShowControlsImageTVUI()
    {
        controlsImageTVUI.SetActive(true);
        RectTransform rectTransform = controlsImageTVUI.GetComponent<RectTransform>();

        // Cancel any ongoing animations
        if (LeanTween.isTweening(rectTransform))
            LeanTween.cancel(rectTransform);

        LeanTween.moveX(rectTransform, secondXPositionImage, 1f).setEase(LeanTweenType.easeOutQuint);
        imageState = true;
    }

    // Hide the controls image TV UI
    public void HideControlsImageTVUI(bool definitive)
    {
        RectTransform rectTransform = controlsImageTVUI.GetComponent<RectTransform>();

        // Cancel any ongoing animations
        if (LeanTween.isTweening(rectTransform))
            LeanTween.cancel(rectTransform);

        LeanTween.moveX(rectTransform, firstXPositionImage, 1f).setEase(LeanTweenType.easeOutQuint).setOnComplete(() =>
        {
            if (definitive && !imageState)
                controlsImageTVUI.SetActive(false);
        });

        imageState = false;
    }

    // Show the controls video TV UI
    public void ShowControlsVideoTVUI()
    {
        controlsVideoTVUI.SetActive(true);
        RectTransform rectTransform = controlsVideoTVUI.GetComponent<RectTransform>();

        // Cancel any ongoing animations
        if (LeanTween.isTweening(rectTransform))
            LeanTween.cancel(rectTransform);

        LeanTween.moveX(rectTransform, secondXPositionVideo, 1f).setEase(LeanTweenType.easeOutQuint);

        videoState = true;
    }

    // Hide the controls video TV UI
    public void HideControlsVideoTVUI(bool definitive)
    {
        RectTransform rectTransform = controlsVideoTVUI.GetComponent<RectTransform>();

        // Cancel any ongoing animations
        if (LeanTween.isTweening(rectTransform))
            LeanTween.cancel(rectTransform);

        LeanTween.moveX(rectTransform, firstXPositionVideo, 1f).setEase(LeanTweenType.easeOutQuint).setOnComplete(() =>
        {
            if (definitive && !videoState)
                controlsVideoTVUI.SetActive(false);
        });

        videoState = false;
    }
}
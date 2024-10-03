using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterUI : MonoBehaviour
{
    [SerializeField]
    private GameObject letterUI;
    private Image letterImage;

    void OnEnable()
    {
        EventManager.Letter.OnDisplayLetterImage += HandleDisplayLetter;
    }

    void OnDisable()
    {
        EventManager.Letter.OnDisplayLetterImage -= HandleDisplayLetter;
    }

    void Start()
    {
        letterImage = letterUI.GetComponentInChildren<Image>();
    }

    private void HandleDisplayLetter(Sprite image, bool active)
    {
        letterImage.sprite = image;
        letterUI.SetActive(active);
    }

}


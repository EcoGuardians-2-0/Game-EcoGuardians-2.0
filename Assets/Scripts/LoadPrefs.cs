using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPrefs : MonoBehaviour
{
    [Header("General setting")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuController menuController;

    [Header("Volume Settings")]
    [SerializeField] private Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Gameplay Settings")]
    [SerializeField] private Text sensTextValue = null;
    [SerializeField] private Slider sensSlider = null;
    private void Awake()
    {
        if (canUse)
        {
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                volumeTextValue.text = localVolume.ToString("0.0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
            {
                menuController.ResetButton("Audio");
            }

            if (PlayerPrefs.HasKey("masterSen"))
            {
                float localSens = PlayerPrefs.GetFloat("masterSen");

                sensTextValue.text = localSens.ToString("0");
                sensSlider.value = localSens;
                menuController.mainControllerSen = Mathf.RoundToInt(localSens);
            }
        }
    }
}

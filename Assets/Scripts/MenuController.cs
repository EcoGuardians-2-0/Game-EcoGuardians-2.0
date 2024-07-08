using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class MenuController : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] private Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 0.5f;

    [Header("Gameplay Settings")]
    [SerializeField] private Text sensTextValue = null;
    [SerializeField] private Slider sensSlider = null;
    [SerializeField] private int defaultSen = 4;
    public int mainControllerSen = 4;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Levels to load")]
    public string newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGame = null;

    private Camera mainCamera;

    private void Start()
    {
        // Try to find the menu camera by name
        mainCamera = GameObject.Find("MainCameraMenu")?.GetComponent<Camera>();

        if (mainCamera == null)
        {
            // Log a warning if the camera is not found
            Debug.LogWarning("MenuCamera not found. Ensure the camera is named 'MenuCamera' in the Inspector.");
            return;
        }

        // Ensure only the menu camera is active
        foreach (Camera cam in Camera.allCameras)
        {
            cam.gameObject.SetActive(false);
        }

        mainCamera.gameObject.SetActive(true);
    }

    public void NewGameDialogYes()  
    {
        SceneManager.LoadScene(newGameLevel);
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGame.SetActive(true);
        }            
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void SetControllerSen(float sensitivity)
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);
        sensTextValue.text = sensitivity.ToString("0");
    }

    public void GameplayApply()
    {
        PlayerPrefs.SetFloat("masterSen", mainControllerSen);
        StartCoroutine (ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }

        if (MenuType == "Gameplay")
        {
            sensTextValue.text = defaultSen.ToString("0");
            sensSlider.value = defaultSen;
            mainControllerSen = defaultSen;
            GameplayApply();
        }
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.Audio;

public class ControllerScreensMenuUI : MonoBehaviour
{   

    // Get the audio mixers
    [Header("Audio Mixers")]
    [SerializeField]
    public AudioMixer mainMixer;

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text sfxVolumeValue = null;
    [SerializeField] private TMP_Text musicVolumeValue = null;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text sensTextValue = null;
    public int mainControllerSen = 4;

    private bool menuP = false;
    [Header("Screens")]
    public GameObject pauseUI;
    public GameObject menu;
    public GameObject settings;
    public GameObject audioUI;
    public GameObject controls;
    public GameObject game;
    public GameObject panelExit;
    public ControllerPauseUI controllerPauseUI;


    public void Update()
    {
        checkEsc();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void MenuTrue(bool Menu)
    {
        menuP = Menu;
    }

    public void Regresar()
    {
        if (menuP)
        {
            menu.SetActive(true);
            menuP = false;
        }
        else
        {
            settings.SetActive(false);
            pauseUI.SetActive(true);
            menuP = false;
        }
    }

    private void checkEsc()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && settings.activeSelf)
        {
            if (!menuP)
            {
                settings.SetActive(false);
                pauseUI.SetActive(true);
                controllerPauseUI.InGame(true);
            }
            else
            {
                settings.SetActive(false);
                menu.SetActive(true);
                controllerPauseUI.FromMenu(false);
            }

        }
        if (Input.GetKeyDown(KeyCode.Escape) && audioUI.activeSelf)
        {
            audioUI.SetActive(false);
            settings.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && game.activeSelf)
        {
            game.SetActive(false);
            settings.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && controls.activeSelf)
        {
            controls.SetActive(false);
            settings.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && panelExit.activeSelf)
            panelExit.gameObject.SetActive(false);
    }

    public void ExitButton()
    {
        Time.timeScale = 1f;
    }

    public void setSFXVolume(float sfxVolume)
    {
        // Get the related audio mixers SFX volume and set it
        mainMixer.SetFloat("sfxVolume", sfxVolume);
        sfxVolumeValue.text = sfxVolume.ToString("0.0");
    }

    public void setMusicVolume(float musicVolume)
    {
        // Get the related audio mixers Music volume and set it
        mainMixer.SetFloat("musicVolume", musicVolume);
        musicVolumeValue.text = musicVolume.ToString("0.0");
    }

    public void SetControllerSen(float sensitivity)
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);
        sensTextValue.text = sensitivity.ToString("0");
    }

    public void GameplayApply()
    {
        PlayerPrefs.SetFloat("masterSen", mainControllerSen);
    }
}

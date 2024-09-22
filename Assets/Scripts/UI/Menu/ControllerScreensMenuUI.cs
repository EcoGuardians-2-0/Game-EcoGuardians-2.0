using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;

public class ControllerScreensMenuUI : MonoBehaviour
{

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;

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
        checkP();
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

    private void checkP()
    {
        if (Input.GetKeyDown(KeyCode.P) && settings.activeSelf)
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
        if (Input.GetKeyDown(KeyCode.P) && audioUI.activeSelf)
        {
            audioUI.SetActive(false);
            settings.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.P) && game.activeSelf)
        {
            game.SetActive(false);
            settings.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.P) && controls.activeSelf)
        {
            controls.SetActive(false);
            settings.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.P) && panelExit.activeSelf)
            panelExit.gameObject.SetActive(false);
    }

    public void ExitButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");

    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
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

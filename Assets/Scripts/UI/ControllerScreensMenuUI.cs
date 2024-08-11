using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ControllerScreensMenuUI : MonoBehaviour
{

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text sensTextValue = null;
    public int mainControllerSen = 4;

    [Header("Levels To Load")]
    public string newGameLevel;
    public string levelToLoad;
    [SerializeField] private GameObject noSavedGame = null;

    private Camera mainCamera;
    private bool menuP = false;
    public GameObject menuPUI;
    public GameObject ajustesUI;
    public GameObject pausaUI;

    private void Start()
    {
        showCursor();
    }

    public void MenuTrue(bool Menu)
    {
        if (Menu == true)
        {
            menuP = true;
        }
    }

    public void NewGameDialogYes()
    {
        hideCursor();
        SceneManager.LoadScene(newGameLevel);

    }
    public void Regresar()
    {
        if (menuP == true)
        {
            menuPUI.SetActive(true);
            menuP = false;
        }
        else
        {
            ajustesUI.SetActive(false);
            pausaUI.SetActive(true);
            menuP = false;
        }

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

    public void ExitButton()
    {
        Time.timeScale = 1f;
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

    public void showCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void hideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

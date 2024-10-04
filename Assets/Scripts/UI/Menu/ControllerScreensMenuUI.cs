using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.Audio;
using UnityEngine.Events;

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
    public float mainControllerSen = 5f;

    [Header("Events")]
    public UnityEvent<string> onGameStarted;

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

    private static ControllerScreensMenuUI _Instance;
    public static ControllerScreensMenuUI Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<ControllerScreensMenuUI>();
                // name it for easy recognition
                _Instance.name = _Instance.GetType().ToString();
                // mark root as DontDestroyOnLoad();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }

    void Start()
    {
        // Get the related audio mixers SFX volume and set it
        setSFXVolume(80.0f);
        mainMixer.SetFloat("musicVolume", -40.0f);
    }

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Destroy(this);
        }
        else
        {
            _Instance = this;
        }
    }

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

    public void setSFXVolume(float sfxVolume)
    {
        // Set the SFX volume
        sfxVolume = Mathf.Clamp(sfxVolume, 0f, 100f);
        float dbSFXVolume = Mathf.Lerp(-80f, 0f, sfxVolume / 100f);
        mainMixer.SetFloat("sfxVolume", dbSFXVolume);
        sfxVolumeValue.text = sfxVolume.ToString("0");
    }

    public void setMusicVolume(float musicVolume)
    {
        // Get the related audio mixers Music volume and set it
        mainMixer.SetFloat("musicVolume", musicVolume);
        musicVolumeValue.text = musicVolume.ToString("0.0");
    }

    public void SetControllerSen(float sensitivity)
    {
        mainControllerSen = sensitivity;
        sensTextValue.text = sensitivity.ToString("0.0");
    }

    public void GameplayApply()
    {
        PlayerPrefs.SetFloat("masterSen", mainControllerSen);
    }

    public void startNewGame()
    {
        onGameStarted.Invoke("NewGame");
    }
}

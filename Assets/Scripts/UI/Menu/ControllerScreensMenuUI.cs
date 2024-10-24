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
    [SerializeField] private Slider sensitivitySlider;
    private float mainControllerSen = 40f;
    

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
    public GameObject AlertChangesSaved;
    public Button returnGame;
    public Button returnAudio;
    public Button returnControls;
    public Button returnSettings;
    private float originalControllerSen;
    
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
        setMusicVolume(80.0f);
        // Establecer el valor predeterminado de sensibilidad
        float defaultSensitivity = 40f;

        // Guardar este valor como la sensibilidad original
        originalControllerSen = defaultSensitivity * 5;

        // Aplicar el valor inicial usando SetControllerSen
        SetControllerSen(defaultSensitivity);
        GameplayApply();

        // Actualizar el slider y el texto al valor inicial
        sensitivitySlider.value = defaultSensitivity;
        sensTextValue.text = defaultSensitivity.ToString("0");
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
            returnSettings.onClick.Invoke();
        if (Input.GetKeyDown(KeyCode.P) && audioUI.activeSelf)
            returnAudio.onClick.Invoke();
        if (Input.GetKeyDown(KeyCode.P) && game.activeSelf)
            returnGame.onClick.Invoke();
        if (Input.GetKeyDown(KeyCode.P) && controls.activeSelf)
            returnControls.onClick.Invoke();
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

    public void setMusicVolume(float volume)
    {
        // Set the music volume
        volume = Mathf.Clamp(volume, 0f, 100f);
        float dbVolume = Mathf.Lerp(-80f, 0f, volume / 100f);
        mainMixer.SetFloat("musicVolume", dbVolume);
        musicVolumeValue.text = volume.ToString("0");
    }

    public void SetControllerSen(float sensitivity)
    {
        float transformedSensitivity = sensitivity * 5;
        mainControllerSen = transformedSensitivity;
        sensTextValue.text = sensitivity.ToString("0");
    }

    public void GameplayApply()
    {
        // Verificar si hubo cambios antes de aplicar
        if (mainControllerSen != originalControllerSen)
        {
            PlayerPrefs.SetFloat("masterSen", mainControllerSen);
            PlayerPrefs.Save();
            StartCoroutine(ActivateAndDeactivate());

            // Actualizar la sensibilidad original al nuevo valor guardado
            originalControllerSen = mainControllerSen;
        }
    }

    // Función para cancelar los cambios y restaurar la sensibilidad original
    public void CancelChanges()
    {
        // Verificar si hubo cambios antes de cancelar
        if (mainControllerSen != originalControllerSen)
        {
            mainControllerSen = originalControllerSen;
            sensTextValue.text = (originalControllerSen / 5).ToString("0");

            // Restaurar el valor del slider
            sensitivitySlider.value = originalControllerSen / 5;
        }
    }

    public void startNewGame()
    {
        onGameStarted.Invoke("NewGame");
    }
    public IEnumerator ActivateAndDeactivate()
    {
        if (!AlertChangesSaved.activeSelf)
        {
            AlertChangesSaved.SetActive(true);

            yield return new WaitForSeconds(3f);

            AlertChangesSaved.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ControllerPauseUI : MonoBehaviour
{
    public bool gameIsPaused = false;
    private bool canPause = false;
    private bool menuP = false;
    public GameObject pauseMenuUI;
    public DisableObjects disableObjects;
    public Vector2 screenCenter;
    
    private static ControllerPauseUI _Instance;
    public static ControllerPauseUI Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<ControllerPauseUI>();
                // name it for easy recognition
                _Instance.name = _Instance.GetType().ToString();
                // mark root as DontDestroyOnLoad();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && canPause)
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void InGame(bool can)
    {
        if (!menuP)
            canPause = can;
        else
            menuP = false;
    }
    public void FromMenu(bool menu)
    {
        menuP = menu;
    }

    public void Resume()
    {
        disableObjects.hideCursor();
        pauseMenuUI.SetActive(false);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Time.timeScale = 1f; // Restaura el tiempo de juego
        gameIsPaused = false; // Actualiza el estado de pausa
    }

    public void Pause()
    {
        disableObjects.showCursor();
        pauseMenuUI.SetActive(true); // Activa el men� de pausa
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Time.timeScale = 0f; // Detiene el tiempo de juego
        gameIsPaused = true; // Actualiza el estado de pausa
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (canPause && !hasFocus)
            Pause();
    }
    public void SetIsPaused(bool isPaused)
    {
        gameIsPaused = isPaused;
    }

    
}
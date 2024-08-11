using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerPauseUI : MonoBehaviour
{
    public bool gameIsPaused = false; // Indica si el juego está pausado
    public GameObject pauseMenuUI; // UI del menú de pausa

    private void Start()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        HideCursor(); // Oculta y bloquea el cursor
        pauseMenuUI.SetActive(false); // Desactiva el menú de pausa
        Time.timeScale = 1f; // Restaura el tiempo de juego
        gameIsPaused = false; // Actualiza el estado de pausa
    }

    public void Pause()
    {
        ShowCursor(); // Muestra el cursor y lo desbloquea
        pauseMenuUI.SetActive(true); // Activa el menú de pausa
        Time.timeScale = 0f; // Detiene el tiempo de juego
        gameIsPaused = true; // Actualiza el estado de pausa
    }

    private void ShowCursor()
    {
        Cursor.visible = true; // Hace visible el cursor
        Cursor.lockState = CursorLockMode.None; // Desbloquea el cursor
    }

    private void HideCursor()
    {
        Cursor.visible = false; // Oculta el cursor
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor
    }
}

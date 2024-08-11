using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausaController1 : MonoBehaviour
{
    public static bool GameIsPaused = false; // Indica si el juego está pausado
    public GameObject pauseMenuUI; // UI del menú de pausa

    private void Start()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
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
            if (GameIsPaused)
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
        hideCursor(); // Oculta y bloquea el cursor
        pauseMenuUI.SetActive(false); // Desactiva el menú de pausa
        Time.timeScale = 1f; // Restaura el tiempo de juego
        GameIsPaused = false; // Actualiza el estado de pausa
    }

    public void Pause()
    {
        showCursor(); // Muestra el cursor y lo desbloquea
        pauseMenuUI.SetActive(true); // Activa el menú de pausa
        Time.timeScale = 0f; // Detiene el tiempo de juego
        GameIsPaused = true; // Actualiza el estado de pausa
    }

    private void showCursor()
    {
        Cursor.visible = true; // Hace visible el cursor
        Cursor.lockState = CursorLockMode.None; // Desbloquea el cursor
    }

    private void hideCursor()
    {
        Cursor.visible = false; // Oculta el cursor
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor
    }
}

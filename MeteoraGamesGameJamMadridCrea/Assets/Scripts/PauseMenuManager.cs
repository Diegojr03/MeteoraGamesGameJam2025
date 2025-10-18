using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;

    void Update()
    {
        // Detectar cuando se presiona ESC para pausar
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pausar el tiempo del juego
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Reanudar el tiempo del juego
        pauseMenu.SetActive(false);
    }

    public void GoToLevelSelection()
    {
        // Resetear vidas antes de ir a selección de niveles
        ResetLives();
        Time.timeScale = 1f; // Asegurar que el tiempo se reanude
        SceneManager.LoadScene("LevelSelection");
    }

    public void GoToMainMenu()
    {
        // Resetear vidas antes de ir al menú principal
        ResetLives();
        Time.timeScale = 1f; // Asegurar que el tiempo se reanude
        SceneManager.LoadScene("MenuInicial");
    }

    void ResetLives()
    {
        // Resetear las vidas a máximo
        PlayerPrefs.SetInt("PlayerLives", 3); // O tu maxLives
        PlayerPrefs.Save();
    }
}

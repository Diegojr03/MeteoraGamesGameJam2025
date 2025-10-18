using UnityEngine;

public class EndLevelManager : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CompleteLevel();
        }
    }

    void CompleteLevel()
    {
        Debug.Log("�Nivel completado!");

        // Guardar progreso al 100%
        LevelProgressTracker progressTracker = FindObjectOfType<LevelProgressTracker>();
        if (progressTracker != null)
        {
            progressTracker.SaveCurrentProgress();
        }

        // Resetear vidas para el pr�ximo nivel
        PlayerPrefs.SetInt("PlayerLives", 3);
        PlayerPrefs.Save();

        // Ir a selecci�n de niveles o siguiente nivel
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelection");
    }
}

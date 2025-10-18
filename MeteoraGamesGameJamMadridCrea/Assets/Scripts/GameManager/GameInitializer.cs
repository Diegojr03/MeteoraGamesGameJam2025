using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    void Start()
    {
        ResetLivesOnGameStart();
    }

    void ResetLivesOnGameStart()
    {
        // Solo resetear si estamos en la escena del menú principal
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu")
        {
            PlayerPrefs.DeleteKey("PlayerLives");
            PlayerPrefs.Save();
            Debug.Log("Vidas reseteadas al iniciar el juego");
        }
    }

    // También resetear cuando se presiona Play en el Editor
#if UNITY_EDITOR
    void OnApplicationQuit()
    {
        // Opcional: Resetear también al salir del juego en el Editor
        PlayerPrefs.DeleteKey("PlayerLives");
        PlayerPrefs.Save();
    }
#endif
}

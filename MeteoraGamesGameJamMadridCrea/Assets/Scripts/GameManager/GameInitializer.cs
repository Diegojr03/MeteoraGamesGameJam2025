using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    void Start()
    {
        ResetLivesOnGameStart();
    }

    void ResetLivesOnGameStart()
    {
        // Solo resetear si estamos en la escena del men� principal
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu")
        {
            PlayerPrefs.DeleteKey("PlayerLives");
            PlayerPrefs.Save();
            Debug.Log("Vidas reseteadas al iniciar el juego");
        }
    }

    // Tambi�n resetear cuando se presiona Play en el Editor
#if UNITY_EDITOR
    void OnApplicationQuit()
    {
        // Opcional: Resetear tambi�n al salir del juego en el Editor
        PlayerPrefs.DeleteKey("PlayerLives");
        PlayerPrefs.Save();
    }
#endif
}

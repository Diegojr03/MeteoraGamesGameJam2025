using UnityEngine;

public class LevelProgressTracker : MonoBehaviour
{
    [Header("Configuración Manual")]
    public float startPositionX = 0f;
    public float endPositionX = 50f;

    [Header("Referencias")]
    public Transform player;

    private float maxProgress = 0f;
    private int currentLevelIndex = 0;

    void Start()
    {
        // Obtener el índice del nivel actual
        currentLevelIndex = PlayerPrefs.GetInt("SelectedLevel", 0);

        // CARGAR EL PROGRESO MÁXIMO PREVIO DE ESTE NIVEL
        LoadPreviousMaxProgress();

        Debug.Log($"Nivel {currentLevelIndex} iniciado. Progreso máximo anterior: {maxProgress * 100}%");
    }

    void LoadPreviousMaxProgress()
    {
        if (GameManager.instance != null)
        {
            maxProgress = GameManager.instance.LoadLevelProgress(currentLevelIndex);
        }
        else
        {
            maxProgress = PlayerPrefs.GetFloat("LastProgress_Level_" + currentLevelIndex, 0f);
        }

        Debug.Log($"Progreso cargado para nivel {currentLevelIndex}: {maxProgress * 100}%");
    }

    void Update()
    {
        if (player == null) return;

        float currentProgress = CalculateProgress();

        if (currentProgress > maxProgress)
        {
            Debug.Log($"¡Nuevo récord en nivel {currentLevelIndex}! {currentProgress * 100}% (anterior: {maxProgress * 100}%)");
            maxProgress = currentProgress;
        }
    }

    float CalculateProgress()
    {
        float playerX = player.position.x;
        float progress = (playerX - startPositionX) / (endPositionX - startPositionX);
        return Mathf.Clamp01(progress);
    }

    public void SaveCurrentProgress()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.SaveLevelProgress(currentLevelIndex, maxProgress);
        }
        else
        {
            PlayerPrefs.SetFloat("LastProgress_Level_" + currentLevelIndex, maxProgress);
            PlayerPrefs.Save();
        }

        Debug.Log($"Progreso guardado para nivel {currentLevelIndex}: {maxProgress * 100}%");
    }

    public float GetCurrentProgress()
    {
        return maxProgress;
    }
}

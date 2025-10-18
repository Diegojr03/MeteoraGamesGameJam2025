using UnityEngine;

public class LevelProgressTracker : MonoBehaviour
{
    [Header("Configuración Manual")]
    public float startPositionX = 0f;
    public float endPositionX = 50f;

    [Header("Referencias")]
    public Transform player;

    private float maxProgress = 0f;

    void Start()
    {
        // CARGAR EL PROGRESO MÁXIMO PREVIO en lugar de resetear a 0
        LoadPreviousMaxProgress();

        Debug.Log($"Nivel iniciado. Progreso máximo anterior: {maxProgress * 100}%");
    }

    void LoadPreviousMaxProgress()
    {
        int levelIndex = PlayerPrefs.GetInt("SelectedLevel", 0);

        if (GameManager.instance != null)
        {
            maxProgress = GameManager.instance.LoadLevelProgress(levelIndex);
        }
        else
        {
            maxProgress = PlayerPrefs.GetFloat("LastProgress_Level_" + levelIndex, 0f);
        }

        Debug.Log($"Progreso cargado: {maxProgress * 100}%");
    }

    void Update()
    {
        if (player == null) return;

        float currentProgress = CalculateProgress();

        if (currentProgress > maxProgress)
        {
            Debug.Log($"¡Nuevo récord! {currentProgress * 100}% (anterior: {maxProgress * 100}%)");
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
            GameManager.instance.SaveLevelProgress(maxProgress);
        }
        else
        {
            int levelIndex = PlayerPrefs.GetInt("SelectedLevel", 0);
            PlayerPrefs.SetFloat("LastProgress_Level_" + levelIndex, maxProgress);
            PlayerPrefs.Save();
        }

        Debug.Log($"Progreso guardado definitivo: {maxProgress * 100}%");
    }

    public float GetCurrentProgress()
    {
        return maxProgress;
    }
}

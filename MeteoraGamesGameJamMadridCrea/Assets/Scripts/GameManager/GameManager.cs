using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Progreso del Nivel")]
    public float currentLevelProgress = 0f;
    public int currentLevelIndex = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveLevelProgress(int levelIndex, float progress)
    {
        progress = Mathf.Clamp01(progress);
        PlayerPrefs.SetFloat("LastProgress_Level_" + levelIndex, progress);
        PlayerPrefs.Save();

        Debug.Log($"Progreso guardado - Nivel {levelIndex}: {progress * 100}%");
    }

    public float LoadLevelProgress(int levelIndex)
    {
        return PlayerPrefs.GetFloat("LastProgress_Level_" + levelIndex, 0f);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelSelectionManager : MonoBehaviour
{
    [Header("Navegación")]
    public Button leftArrow;
    public Button rightArrow;
    public Button playButton;
    public TextMeshProUGUI levelNameText;

    [Header("Progreso Visual")]
    public Slider progressBar;
    public TextMeshProUGUI progressText;

    [Header("Configuración de Niveles")]
    public string[] levelNames = { "Nivel 1", "Nivel 2", "Nivel 3", "Nivel 4" };
    public string[] sceneNames = { "Nivel1", "Nivel2", "Nivel3", "Nivel4" };

    private int currentLevelIndex = 0;

    void Start()
    {
        leftArrow.onClick.AddListener(PreviousLevel);
        rightArrow.onClick.AddListener(NextLevel);
        playButton.onClick.AddListener(PlayCurrentLevel);

        UpdateLevelDisplay();
    }

    void PreviousLevel()
    {
        if (currentLevelIndex > 0)
        {
            currentLevelIndex--;
            UpdateLevelDisplay();
        }
    }

    void NextLevel()
    {
        if (currentLevelIndex < levelNames.Length - 1)
        {
            currentLevelIndex++;
            UpdateLevelDisplay();
        }
    }

    void UpdateLevelDisplay()
    {
        // Actualizar nombre del nivel
        levelNameText.text = levelNames[currentLevelIndex];

        // Obtener y mostrar progreso
        float progress = GetLevelProgress(currentLevelIndex);
        progressBar.value = progress;

        // Actualizar texto de porcentaje
        if (progressText != null)
        {
            progressText.text = Mathf.RoundToInt(progress * 100) + "%";
        }

        // Actualizar estado de las flechas
        leftArrow.interactable = (currentLevelIndex > 0);
        rightArrow.interactable = (currentLevelIndex < levelNames.Length - 1);
    }

    float GetLevelProgress(int levelIndex)
    {
        return PlayerPrefs.GetFloat("LastProgress_Level_" + levelIndex, 0f);
    }

    public void PlayCurrentLevel()
    {
        PlayerPrefs.SetInt("SelectedLevel", currentLevelIndex);

        // Cargar la escena específica del nivel seleccionado
        if (currentLevelIndex >= 0 && currentLevelIndex < sceneNames.Length)
        {
            SceneManager.LoadScene(sceneNames[currentLevelIndex]);
        }
        else
        {
            Debug.LogError("Índice de nivel fuera de rango: " + currentLevelIndex);
            SceneManager.LoadScene("GameScene");
        }
    }
}

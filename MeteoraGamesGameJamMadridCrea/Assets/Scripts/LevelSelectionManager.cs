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
    public Image[] coins;

    [Header("Configuración de Niveles")]
    public string[] levelNames = { "Nivel 1", "Nivel 2", "Nivel 3", "Nivel 4" };

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
        // IZQUIERDA = nivel anterior (número más bajo)
        if (currentLevelIndex > 0)
        {
            currentLevelIndex--;
            UpdateLevelDisplay();
        }
    }

    void NextLevel()
    {
        // DERECHA = nivel siguiente (número más alto)  
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

        // FLECHAS: Izquierda desactivada en nivel 1, Derecha desactivada en nivel 4
        leftArrow.interactable = (currentLevelIndex > 0);
        rightArrow.interactable = (currentLevelIndex < levelNames.Length - 1);

        // Actualizar monedas
        UpdateCoinsDisplay();
    }

    void UpdateCoinsDisplay()
    {
        for (int i = 0; i < coins.Length; i++)
        {
            int coinCollected = PlayerPrefs.GetInt("Level_" + currentLevelIndex + "_Coin_" + i, 0);
            coins[i].color = (coinCollected == 1) ? Color.yellow : Color.gray;
        }
    }

    float GetLevelProgress(int levelIndex)
    {
        return PlayerPrefs.GetFloat("LastProgress_Level_" + levelIndex, 0f);
    }

    public void PlayCurrentLevel()
    {
        PlayerPrefs.SetInt("SelectedLevel", currentLevelIndex);
        SceneManager.LoadScene("GameScene");
    }
}

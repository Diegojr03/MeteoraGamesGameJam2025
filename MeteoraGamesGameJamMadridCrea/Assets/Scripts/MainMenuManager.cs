using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button playButton;
    public Button controlsButton;
    public GameObject PanelControles;
    public GameObject BotonAtras;

    void Start()
    {
        // Asignar las funciones a los botones
        playButton.onClick.AddListener(PlayGame);
        controlsButton.onClick.AddListener(ShowControls);
    }

    void PlayGame()
    {
        // Cargar la escena de selección de niveles
        SceneManager.LoadScene("LevelSelection");
    }

    void ShowControls()
    {
        PanelControles.SetActive(true);
        BotonAtras.SetActive(true);
    }
    public void HideControls()
    {
        PanelControles.SetActive(false);
        BotonAtras.SetActive(false);
    }
}

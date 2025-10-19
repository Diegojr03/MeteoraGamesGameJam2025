using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [Header("Referencias de Corazones")]
    public Image[] hearts;

    [Header("Colores")]
    public Color fullHeartColor = Color.red;
    public Color emptyHeartColor = Color.gray;

    private PlayerController playerController;

    void Start()
    {
        // Buscar el PlayerController y esperar un frame si es necesario
        StartCoroutine(FindPlayerCoroutine());
    }

    System.Collections.IEnumerator FindPlayerCoroutine()
    {
        // Esperar un frame para asegurar que PlayerController est� listo
        yield return null;

        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("No se encontr� PlayerController en la escena");
        }
        else
        {
            // Actualizar corazones con el valor inicial
            UpdateHearts();
        }
    }

    void Update()
    {
        // Actualizar continuamente por si acaso
        UpdateHearts();
    }

    void UpdateHearts()
    {
        if (playerController == null) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
            {
                if (i < playerController.currentLives)
                {
                    hearts[i].color = fullHeartColor;
                }
                else
                {
                    hearts[i].color = emptyHeartColor;
                }
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class SimplePlayerHUD : MonoBehaviour
{
    public Image playerImage;
    public PlayerController player;
    public Sprite[] healthSprites; // [0] = 1 vida, [1] = 2 vidas, [2] = 3 vidas

    void Update()
    {
        if (player == null) return;

        int lives = player.currentLives;

        // Asegurar que el índice esté en rango
        int spriteIndex = Mathf.Clamp(lives - 1, 0, healthSprites.Length - 1);

        if (healthSprites[spriteIndex] != null && playerImage != null)
        {
            playerImage.sprite = healthSprites[spriteIndex];
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Vidas")]
    public int maxLives = 3;
    public int currentLives;


    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;
    public GameObject panelMuerte;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // CARGAR VIDAS GUARDADAS CON SEGURIDAD EXTRA
        currentLives = PlayerPrefs.GetInt("PlayerLives", maxLives);

        // FORZAR que nunca tenga más de maxLives ni menos de 1
        if (currentLives > maxLives)
        {
            currentLives = maxLives;
            PlayerPrefs.SetInt("PlayerLives", currentLives);
            PlayerPrefs.Save();
        }
        else if (currentLives < 1)
        {
            currentLives = maxLives;
            PlayerPrefs.SetInt("PlayerLives", currentLives);
            PlayerPrefs.Save();
        }

        Debug.Log("Vidas al iniciar nivel: " + currentLives);
    }

    // El resto del script se mantiene igual...
    void Update()
    {
        if (!isInvincible)
        {
            Movement();
            Jump();
        }
    }

    void Movement()
    {
        float moveX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            canDoubleJump = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump && !isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            canDoubleJump = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary") && !isInvincible)
        {
            TakeDamage();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canDoubleJump = false;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void TakeDamage()
    {
        currentLives--;

        // GUARDAR LAS VIDAS ACTUALES
        PlayerPrefs.SetInt("PlayerLives", currentLives);
        PlayerPrefs.Save();

        Debug.Log("Vidas después de daño: " + currentLives);

        // GUARDAR PROGRESO ANTES DE REINICIAR
        SaveProgressBeforeRestart();

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(InvincibilityFrames());
        }
    }

    void SaveProgressBeforeRestart()
    {
        LevelProgressTracker progressTracker = FindObjectOfType<LevelProgressTracker>();
        if (progressTracker != null)
        {
            progressTracker.SaveCurrentProgress();
        }
    }

    System.Collections.IEnumerator InvincibilityFrames()
    {
        isInvincible = true;

        float timer = 0f;
        float blinkInterval = 0.1f;

        while (timer < 1f)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
        RestartLevel();
    }

    void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void GameOver()
    {
        Debug.Log("Game Over! Reiniciando vidas...");

        // Guardar progreso final
        SaveProgressBeforeRestart();

        ResetLives();
        panelMuerte.SetActive(true);
    }

    public void ResetLives()
    {
        PlayerPrefs.SetInt("PlayerLives", maxLives);
        PlayerPrefs.Save();
        currentLives = maxLives;
    }
}

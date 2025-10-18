using UnityEngine;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentLives = maxLives;
    }

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

    void TakeDamage()
    {
        currentLives--;

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(InvincibilityFrames());
        }
    }

    System.Collections.IEnumerator InvincibilityFrames()
    {
        isInvincible = true;

        float timer = 0f;
        float blinkInterval = 0.1f;

        while (timer < 2f)
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
        Debug.Log("Game Over!");
        RestartLevel();
    }
}

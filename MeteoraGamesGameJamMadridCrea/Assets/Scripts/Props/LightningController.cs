using UnityEngine;

public class LightningController : MonoBehaviour
{
    [Header("Configuración Rayo")]
    public float speed = 10f;
    public float lifetime = 2f;
    public int damage = 1;

    private Vector3 targetPosition;
    private bool hasHit = false;

    void Start()
    {
        // Destruir automáticamente después del tiempo de vida
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (!hasHit && targetPosition != null)
        {
            // Mover hacia la posición objetivo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Rotar hacia la dirección del movimiento
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            // Si llegó al destino
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                OnReachTarget();
            }
        }
    }

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }

    void OnReachTarget()
    {
        hasHit = true;
        // Aquí puedes agregar efectos de impacto
        Debug.Log("Rayo alcanzó el objetivo");

        // Destruir después de un pequeño delay para ver el impacto
        Destroy(gameObject, 0.2f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasHit)
        {
            // Hacer daño al jugador
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage();
            }

            OnReachTarget();
        }
    }
}

using UnityEngine;

public class LightningController : MonoBehaviour
{
    [Header("Configuraci�n Rayo")]
    public float speed = 10f;
    public float lifetime = 2f;
    public int damage = 1;

    private Vector3 targetPosition;
    private bool hasHit = false;

    void Start()
    {
        // Destruir autom�ticamente despu�s del tiempo de vida
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (!hasHit && targetPosition != null)
        {
            // Mover hacia la posici�n objetivo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Rotar hacia la direcci�n del movimiento
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            // Si lleg� al destino
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
        // Aqu� puedes agregar efectos de impacto
        Debug.Log("Rayo alcanz� el objetivo");

        // Destruir despu�s de un peque�o delay para ver el impacto
        Destroy(gameObject, 0.2f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasHit)
        {
            // Hacer da�o al jugador
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage();
            }

            OnReachTarget();
        }
    }
}

using UnityEngine;

public class MovingPlatformVertical : MonoBehaviour
{
    [Header("Configuración Movimiento Vertical")]
    public float bottomPositionY = -3f; // Posición inferior
    public float topPositionY = 3f;     // Posición superior
    public float moveSpeed = 2f;
    public float waitTime = 1f;         // Tiempo de espera en cada extremo

    [Header("Gizmos")]
    public Color gizmoColor = Color.green;

    private Vector3 bottomPosition;
    private Vector3 topPosition;
    private Vector3 targetPosition;
    private bool isWaiting = false;

    void Start()
    {
        // Calcular posiciones basadas en la posición inicial
        Vector3 startPos = transform.position;
        bottomPosition = new Vector3(startPos.x, bottomPositionY, startPos.z);
        topPosition = new Vector3(startPos.x, topPositionY, startPos.z);

        // Empezar moviéndose hacia arriba
        targetPosition = topPosition;
    }

    void Update()
    {
        if (!isWaiting)
        {
            // Mover hacia la posición objetivo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Si llegó al destino, esperar y cambiar dirección
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                StartCoroutine(WaitAndSwitch());
            }
        }
    }

    System.Collections.IEnumerator WaitAndSwitch()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        // Cambiar dirección
        targetPosition = (targetPosition == topPosition) ? bottomPosition : topPosition;
        isWaiting = false;
    }

    // Dibujar Gizmos en el Editor
    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Vector3 currentPos = transform.position;
            bottomPosition = new Vector3(currentPos.x, bottomPositionY, currentPos.z);
            topPosition = new Vector3(currentPos.x, topPositionY, currentPos.z);
        }

        Gizmos.color = gizmoColor;

        // Dibujar línea del recorrido
        Gizmos.DrawLine(bottomPosition, topPosition);

        // Dibujar esferas en los puntos extremos
        Gizmos.DrawWireSphere(bottomPosition, 0.3f);
        Gizmos.DrawWireSphere(topPosition, 0.3f);

        // Dibujar cubo en la plataforma
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider2D>()?.size ?? Vector3.one);
    }

    // Para que el jugador se mueva con la plataforma
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}

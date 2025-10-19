using UnityEngine;

public class MovingPlatformHorizontal : MonoBehaviour
{
    [Header("Configuración Movimiento Horizontal")]
    public float leftPositionX = -5f;   // Posición izquierda
    public float rightPositionX = 5f;   // Posición derecha
    public float moveSpeed = 2f;
    public float waitTime = 1f;         // Tiempo de espera en cada extremo

    [Header("Gizmos")]
    public Color gizmoColor = Color.blue;

    private Vector3 leftPosition;
    private Vector3 rightPosition;
    private Vector3 targetPosition;
    private bool isWaiting = false;

    void Start()
    {
        // Calcular posiciones basadas en la posición inicial
        Vector3 startPos = transform.position;
        leftPosition = new Vector3(leftPositionX, startPos.y, startPos.z);
        rightPosition = new Vector3(rightPositionX, startPos.y, startPos.z);

        // Empezar moviéndose hacia la derecha
        targetPosition = rightPosition;
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
        targetPosition = (targetPosition == rightPosition) ? leftPosition : rightPosition;
        isWaiting = false;
    }

    // Dibujar Gizmos en el Editor
    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Vector3 currentPos = transform.position;
            leftPosition = new Vector3(leftPositionX, currentPos.y, currentPos.z);
            rightPosition = new Vector3(rightPositionX, currentPos.y, currentPos.z);
        }

        Gizmos.color = gizmoColor;

        // Dibujar línea del recorrido
        Gizmos.DrawLine(leftPosition, rightPosition);

        // Dibujar esferas en los puntos extremos
        Gizmos.DrawWireSphere(leftPosition, 0.3f);
        Gizmos.DrawWireSphere(rightPosition, 0.3f);

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

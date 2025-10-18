using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;

    [Header("Configuración")]
    public float cameraBaseSpeed = 0.5f; // Velocidad constante de la cámara
    public float followZoneMin = 0.50f; // 55% - cuando empieza a seguir al jugador

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void LateUpdate()
    {
        // Calcular dónde está el jugador en la pantalla (0 a 1)
        Vector3 playerViewportPos = Camera.main.WorldToViewportPoint(player.position);
        float playerScreenX = playerViewportPos.x;

        // DECIDIR COMPORTAMIENTO
        if (playerScreenX >= followZoneMin)
        {
            // Zona 55%-100%: La cámara sigue al jugador
            FollowPlayer();
        }
        else
        {
            // Zona 1%-54%: La cámara se mueve a velocidad constante
            MoveConstant();
        }
    }

    void FollowPlayer()
    {
        // Seguir al jugador en X
        float targetX = player.position.x;
        Vector3 targetPosition = new Vector3(targetX, initialPosition.y, initialPosition.z);
        transform.position = targetPosition;
    }

    void MoveConstant()
    {
        // Mover la cámara a velocidad constante hacia la derecha
        transform.Translate(Vector2.right * cameraBaseSpeed * Time.deltaTime);
    }
}

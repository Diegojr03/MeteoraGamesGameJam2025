using UnityEngine;

public class CameraBoundaryAdjuster : MonoBehaviour
{
    void Start()
    {
        AdjustBoundaryPosition();
    }

    void AdjustBoundaryPosition()
    {
        Camera cam = Camera.main;

        if (cam != null)
        {
            // Calcular el borde izquierdo de la cámara en unidades del mundo
            float cameraHeight = 2f * cam.orthographicSize;
            float cameraWidth = cameraHeight * cam.aspect;
            float leftEdge = cam.transform.position.x - cameraWidth / 2f;

            // Posicionar el collider justo en el borde izquierdo
            transform.position = new Vector3(leftEdge, cam.transform.position.y, 0);

            // Ajustar el tamaño para cubrir toda la altura
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.size = new Vector2(0.5f, cameraHeight);
            }
        }
    }
}

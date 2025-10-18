using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        // La c�mara siempre sigue al jugador en X
        transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
    }
}

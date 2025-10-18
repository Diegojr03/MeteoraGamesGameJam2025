using UnityEngine;

public class BoundaryController : MonoBehaviour
{
    public float moveSpeed = 2f;

    void Update()
    {
        // El boundary siempre se mueve a la derecha
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }
}

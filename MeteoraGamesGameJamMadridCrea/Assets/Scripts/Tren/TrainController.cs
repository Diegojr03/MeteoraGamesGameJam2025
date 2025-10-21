using UnityEngine;

public class TrainController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public Vector2 leftLimit = new Vector2(-10f, 0f);
    public Vector2 rightLimit = new Vector2(10f, 0f);

    private bool movingRight = false;

    [Header("Humo")]
    public ParticleSystem smokePrefab;
    public Vector2[] smokePoints; // Puntos locales desde donde emitir humo

    private float nextSmokeTime = 0f;
    public float smokeInterval = 1f; // cada cuánto generar humo

    private void Update()
    {
        MoveTrain();
        EmitSmoke();
    }

    void MoveTrain()
    {
        Vector3 target = movingRight ? rightLimit : leftLimit;
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            movingRight = !movingRight;
            FlipTrain();
        }
    }

    void FlipTrain()
    {
        // Invierte escala para que el sprite mire en la dirección correcta
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void EmitSmoke()
    {
        if (smokePrefab == null || smokePoints.Length == 0) return;

        if (Time.time >= nextSmokeTime)
        {
            foreach (Vector2 point in smokePoints)
            {
                Vector3 worldPoint = transform.TransformPoint(point);
                ParticleSystem newSmoke = Instantiate(smokePrefab, worldPoint, Quaternion.identity);
                newSmoke.Play();

                // Hacemos que se destruyan tras un tiempo para limpiar memoria
                Destroy(newSmoke.gameObject, newSmoke.main.startLifetime.constantMax + 2f);
            }

            nextSmokeTime = Time.time + smokeInterval;
        }
    }

    // Dibujar los límites del recorrido
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(leftLimit, 0.3f);
        Gizmos.DrawSphere(rightLimit, 0.3f);
        Gizmos.DrawLine(leftLimit, rightLimit);

        Gizmos.color = Color.gray;
        if (smokePoints != null)
        {
            foreach (Vector2 point in smokePoints)
            {
                Vector3 world = Application.isPlaying ? transform.TransformPoint(point) : transform.position + (Vector3)point;
                Gizmos.DrawWireSphere(world, 0.2f);
            }
        }
    }
}

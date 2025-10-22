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

    private ParticleSystem[] activeSmokeSystems; // Sistemas de humo activos

    private void Start()
    {
        // Crear sistemas de humo como hijos del tren
        if (smokePrefab != null && smokePoints.Length > 0)
        {
            activeSmokeSystems = new ParticleSystem[smokePoints.Length];

            for (int i = 0; i < smokePoints.Length; i++)
            {
                Vector3 worldPoint = transform.TransformPoint(smokePoints[i]);
                ParticleSystem newSmoke = Instantiate(smokePrefab, worldPoint, Quaternion.identity, transform);
                newSmoke.transform.localPosition = smokePoints[i]; // Posición local
                activeSmokeSystems[i] = newSmoke;
                newSmoke.Play();
            }
        }
    }

    private void Update()
    {
        MoveTrain();
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

        // Ajustar los sistemas de humo para que mantengan la dirección correcta
        if (activeSmokeSystems != null)
        {
            foreach (ParticleSystem smoke in activeSmokeSystems)
            {
                if (smoke != null)
                {
                    // Mantener la escala X positiva para que el humo siempre emita hacia arriba
                    Vector3 smokeScale = smoke.transform.localScale;
                    smokeScale.x = Mathf.Abs(smokeScale.x); // Siempre positivo
                    smoke.transform.localScale = smokeScale;

                    // Opcional: Ajustar la rotación si es necesario
                    // smoke.transform.rotation = Quaternion.identity;
                }
            }
        }
    }

    // Opcional: Detener humo cuando el tren se destruya
    private void OnDestroy()
    {
        if (activeSmokeSystems != null)
        {
            foreach (ParticleSystem smoke in activeSmokeSystems)
            {
                if (smoke != null)
                {
                    smoke.Stop();
                }
            }
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

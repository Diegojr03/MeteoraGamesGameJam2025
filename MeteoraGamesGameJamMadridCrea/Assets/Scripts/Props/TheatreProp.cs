using UnityEngine;

public class TheatreProp : MonoBehaviour
{
    [Header("Configuración Animación")]
    public float lowerHeight = 2f;
    public float raiseHeight = 8f;
    public float moveDuration = 2f;
    public float waitTime = 4f;

    [Header("Configuración Rayos")]
    public bool canShootLightning = false;  // Activar/desactivar rayos
    public GameObject lightningPrefab;
    public int numberOfShots = 2;
    public float minShotInterval = 0.5f;
    public float maxShotInterval = 1.5f;

    [Header("Referencias")]
    public LineRenderer ropeRenderer;
    public Transform ropeTopPoint;
    public Transform player;

    private Vector3 originalPosition;
    private bool isAnimating = false;
    private bool isLowered = false;

    void Start()
    {
        originalPosition = transform.position;

        // Buscar al jugador si no está asignado
        if (player == null && canShootLightning)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        StartAnimation();
    }

    void Update()
    {
        UpdateRope();
    }

    public void StartAnimation()
    {
        if (!isAnimating)
        {
            isAnimating = true;
            StartCoroutine(AnimationCycle());
        }
    }

    public void StopAnimation()
    {
        isAnimating = false;
        StopAllCoroutines();
    }

    System.Collections.IEnumerator AnimationCycle()
    {
        while (isAnimating)
        {
            // BAJAR
            yield return StartCoroutine(MoveToHeight(lowerHeight, moveDuration));
            isLowered = true;

            // DISPARAR RAYOS si está activado
            if (canShootLightning)
            {
                StartCoroutine(ShootLightningSequence());
            }

            // ESPERAR abajo
            yield return new WaitForSeconds(waitTime);

            isLowered = false;

            // SUBIR
            yield return StartCoroutine(MoveToHeight(raiseHeight, moveDuration));
        }
    }

    System.Collections.IEnumerator MoveToHeight(float targetHeight, float duration)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(originalPosition.x, targetHeight, originalPosition.z);

        float timer = 0f;
        while (timer < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }

    System.Collections.IEnumerator ShootLightningSequence()
    {
        for (int i = 0; i < numberOfShots; i++)
        {
            if (isLowered && player != null && canShootLightning)
            {
                ShootLightning();

                float interval = Random.Range(minShotInterval, maxShotInterval);
                yield return new WaitForSeconds(interval);
            }
        }
    }

    void ShootLightning()
    {
        if (lightningPrefab != null && player != null)
        {
            GameObject lightning = Instantiate(lightningPrefab, transform.position, Quaternion.identity);

            LightningController lightningController = lightning.GetComponent<LightningController>();
            if (lightningController != null)
            {
                lightningController.SetTarget(player.position);
            }

            Debug.Log("¡Rayo disparado!");
        }
    }

    void UpdateRope()
    {
        if (ropeRenderer != null && ropeTopPoint != null)
        {
            ropeRenderer.positionCount = 2;
            ropeRenderer.SetPosition(0, ropeTopPoint.position);
            ropeRenderer.SetPosition(1, transform.position);
        }
    }
}

using UnityEngine;

public class SmokeDamageZOne : MonoBehaviour
{
    [Header("Da�o")]
    public int damagePerHit = 1;         // da�o por "impacto"
    public float damageInterval = 1f;    // segundos entre da�os mientras el jugador est� dentro

    // Para asegurar que no se spammee da�o
    private float nextDamageTime = 0f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        TryDamage(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Aplicar da�o seg�n intervalo
        if (Time.time >= nextDamageTime)
        {
            TryDamage(other);
        }
    }

    void TryDamage(Collider2D other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc == null) return;

        // Llamamos al TakeDamage existente en tu PlayerController
        // (ya gestiona invencibilidad, vidas y reinicio).
        for (int i = 0; i < damagePerHit; i++)
        {
            pc.TakeDamage();
        }

        nextDamageTime = Time.time + damageInterval;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Al salir, dejamos listo para volver a da�ar inmediatamente si entra otra vez
        nextDamageTime = Time.time + 0f;
    }
}

using UnityEngine;

public class SmokeDamageZOne : MonoBehaviour
{
    [Header("Daño")]
    public int damagePerHit = 1;         // daño por "impacto"
    public float damageInterval = 1f;    // segundos entre daños mientras el jugador está dentro

    // Para asegurar que no se spammee daño
    private float nextDamageTime = 0f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        TryDamage(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Aplicar daño según intervalo
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

        // Al salir, dejamos listo para volver a dañar inmediatamente si entra otra vez
        nextDamageTime = Time.time + 0f;
    }
}

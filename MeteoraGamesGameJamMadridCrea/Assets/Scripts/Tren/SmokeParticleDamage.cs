using UnityEngine;

public class SmokeParticleDamage : MonoBehaviour
{
    public int damage = 1;                // Daño por colisión
    public float damageCooldown = 3f;     // Segundos entre daños
    private float nextDamageTime = 0f;

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player") && Time.time >= nextDamageTime)
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.TakeDamage();  // o pc.TakeDamage(damage) si tu método lo admite
            }

            nextDamageTime = Time.time + damageCooldown;
        }
    }
}

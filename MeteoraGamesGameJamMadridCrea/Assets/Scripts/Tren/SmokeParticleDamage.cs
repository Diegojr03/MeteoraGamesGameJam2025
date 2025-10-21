using UnityEngine;

public class SmokeParticleDamage : MonoBehaviour
{
    public int damage = 1;                // Da�o por colisi�n
    public float damageCooldown = 3f;     // Segundos entre da�os
    private float nextDamageTime = 0f;

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player") && Time.time >= nextDamageTime)
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.TakeDamage();  // o pc.TakeDamage(damage) si tu m�todo lo admite
            }

            nextDamageTime = Time.time + damageCooldown;
        }
    }
}

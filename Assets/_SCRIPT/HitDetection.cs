using UnityEngine;
using Lean.Pool;

public class HitDetection : MonoBehaviour
{
    public ParticleSystem hitParticle; // Drag and drop your particle system prefab here

    
    private void OnCollisionEnter(Collision other)
    {
        // Check if the object we collided with has a specific tag (you can customize this)
        if (other.collider.CompareTag("Wall"))
        {
            Debug.Log("Hit wall");
            // Play the particle effect at the collision point
            PlayHitParticleEffect(transform.position + Vector3.forward);

            // You can add more logic here, like dealing damage, destroying the enemy, etc.


        }
    }

    private void PlayHitParticleEffect(Vector3 position)
    {
        // Instantiate the particle effect at the specified position
        ParticleSystem particleInstance = LeanPool.Spawn(hitParticle, position, Quaternion.identity);

        // Destroy the particle system after its duration
        LeanPool.Despawn(particleInstance.gameObject, particleInstance.main.duration);
    }
}
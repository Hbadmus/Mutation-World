using UnityEngine;

public class DestroyProjectile : MonoBehaviour
{
    public float destroyTime = 3f; // Corrected typo

    void Start()
    {
        // Destroy the projectile after a specified time
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the Eagle Eye ability is not active
        if (!CharacterAbilites.isEagleEyeActive)
        {
            Destroy(gameObject);
        }
    }
}

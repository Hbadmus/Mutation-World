using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    // Explosion Properties
    [Header("Explosion Settings")]
    [SerializeField] private GameObject cratePieces;     // Prefab for crate pieces when broken
    [SerializeField] private float explosionForce = 250; // Force applied to pieces during explosion
    [SerializeField] private float explosionRadius = 10f; // Radius within which pieces are affected by explosion

    // Ultimate Ability Settings
    [Header("Jack's Ultimate Ability")]
    [SerializeField] private bool jackUlt = false;       // Determines if Jack's ultimate ability is active

    // Audio Sources
    [Header("Audio Sources")]
    [SerializeField] private AudioSource regular;        // Audio source for regular bullets
    [SerializeField] private AudioSource shrapnel;       // Audio source for shrapnel bullets

    private void Start()
    {
        // Play regular bullet audio initially
        regular.Play();

        // Check if Jack's shrapnel ability is active and play corresponding audio
        if (CharacterAbilites.isJackShrapnelActive)
        {
            shrapnel.Play();
        }
    }

    private void Update()
    {
        // Update jackUlt flag based on the status of Jack's shrapnel ability
        jackUlt = CharacterAbilites.isJackShrapnelActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if collided object is an Enemy and Jack's ultimate is active
        if (other.gameObject.CompareTag("Enemy") && jackUlt)
        {
            Debug.Log("Ultimate ability activated");

            Transform currentCrate = gameObject.transform;

            // Instantiate crate pieces and apply explosion force to simulate breaking
            GameObject pieces = Instantiate(cratePieces, currentCrate.position, currentCrate.rotation);
            Rigidbody[] rbpieces = pieces.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rb in rbpieces)
            {
                rb.AddExplosionForce(explosionForce, currentCrate.position, explosionRadius);
            }

            Destroy(gameObject); // Destroy the original crate object after breaking
        }
    }
}

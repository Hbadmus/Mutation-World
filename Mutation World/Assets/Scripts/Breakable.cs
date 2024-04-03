using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameObject cratePieces;
    public float explosionForce = 250;
    public float explosionRadius = 10f;
    public bool jackUlt = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mob") || other.gameObject.CompareTag("Enemy") && jackUlt)
        {
        Transform currentCrate = gameObject.transform;

        GameObject pieces = Instantiate(cratePieces, currentCrate.position, currentCrate.rotation);

        Rigidbody[] rbpieces = pieces.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rbpieces)
        {
            rb.AddExplosionForce(explosionForce, currentCrate.position, explosionRadius);
        }

        Destroy(gameObject);
        }
    }
}

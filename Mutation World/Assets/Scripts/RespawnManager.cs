using UnityEngine;
using System.Collections;

public class RespawnManager : MonoBehaviour
{
    // Singleton instance
    public static RespawnManager Instance;

    private void Awake()
    {
        // Ensure there is only one instance of this manager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep alive for the game's lifetime
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to start the respawn coroutine from another script
    public void StartRespawn(GameObject pickupObject, float delay)
    {
        StartCoroutine(RespawnPickup(pickupObject, delay));
    }

    private IEnumerator RespawnPickup(GameObject pickupObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        pickupObject.SetActive(true);
    }
}

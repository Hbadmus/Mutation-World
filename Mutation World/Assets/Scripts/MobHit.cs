using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider) {
        if(collider.CompareTag("Projectile")) {
            Destroy(gameObject);
        }
    }
}
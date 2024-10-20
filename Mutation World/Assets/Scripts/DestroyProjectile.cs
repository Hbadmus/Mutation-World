using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyProjectile : MonoBehaviour
{

    public float destoryTime = 3;
        private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        Destroy(gameObject, destoryTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        private void OnTriggerEnter(Collider other)
    {
        if(!CharacterAbilites.isEagleEyeActive ) {
        Destroy(gameObject);  
        }
    }
}
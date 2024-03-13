using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilites : MonoBehaviour
{
    public float eagleEyeDuration = 5f;
    public float stormOfArrowsCooldown = 30f;
    public float stormOfArrowsRadius = 5f;
    public float stormOfArrowsDamage = 50f;

    public GameObject enemy;
    public GameObject ammoPrefab;
    Rigidbody rb;
    
    private Material originalMat;
    public Material highlightMat;

    private bool isEagleEyeReady = false;
    public static bool isEagleEyeActive = false;
    private bool isStormOfArrowsReady = true;
    public static bool isStormOfArrowsActive = false;

    // Start is called before the first frame update
    void Start()
    {
        originalMat = enemy.GetComponent<SkinnedMeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) )
        {
            ActivateEagleEye();
        }
        if (Input.GetKeyDown(KeyCode.E) && isStormOfArrowsReady)
        {
            UseStormOfArrows();
        }
    }

    void ActivateEagleEye()
    {
        if (!isEagleEyeActive)
        {
          isEagleEyeActive = true;
        enemy.GetComponent<SkinnedMeshRenderer>().material = highlightMat;
        Invoke("DeactivateEagleEye", eagleEyeDuration);
        }
    }

    void DeactivateEagleEye()
    {
        isEagleEyeActive = false;
        enemy.GetComponent<SkinnedMeshRenderer>().material = originalMat;
    }

        void UseStormOfArrows()
    {
        isStormOfArrowsActive = true;
    }

    void ResetStormOfArrowsCooldown()
    {
        isStormOfArrowsReady = true;
        Debug.Log("Storm of Arrows ready to use.");
    }
}

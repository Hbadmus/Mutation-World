using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilites : MonoBehaviour
{
    public float eagleEyeDuration = 5f;
    public float eagleEyeCool = 10f;
    public float stormOfArrowsCooldown = 30f;
    public float stormOfArrowsRadius = 5f;
    public float stormOfArrowsDamage = 50f;
    public float jackAutoMainDuration = 10f;
    public float jackAutoMainCool = 15f;
    public float jackUltShrapnelDuration = 5f;
    public float jackUltShrapnelCool = 20f;

    public GameObject enemy;
    public GameObject ammoPrefab;
    Rigidbody rb;
    
    private Material originalMat;
    public Material highlightMat;

    private bool isEagleEyeReady = false;
    public static bool isEagleEyeActive = false;
    private bool isStormOfArrowsReady = true;
    public static bool isStormOfArrowsActive = false;
    public static bool isJackAutoActive = false;
    public static bool isJackShrapnelActive = false;

    // Start is called before the first frame update
    void Start()
    {
        if (enemy != null)
        {
            originalMat = enemy.GetComponent<SkinnedMeshRenderer>().material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateCooldown();

        if (gameObject.name == "Fiona")
        {
            if (Input.GetKeyDown(KeyCode.Q) && eagleEyeCool <= 0)
            {
                ActivateEagleEye();
            }
            if (Input.GetKeyDown(KeyCode.E) && stormOfArrowsCooldown <= 0)
            {
                UseStormOfArrows();
            }
        }
        if (gameObject.name == "Jack")
        {
            if (Input.GetKeyDown(KeyCode.Q) && jackAutoMainCool <= 0)
            {
                ActivateJackAuto();
            }
            if (Input.GetKeyDown(KeyCode.E) && jackUltShrapnelCool <=0) 
            {
                ActivateJackShrapnel();
            }
        }
    }

    void ActivateEagleEye()
    {
        if (!isEagleEyeActive)
        {
            isEagleEyeActive = true;
            eagleEyeCool = 10f;
            enemy.GetComponent<SkinnedMeshRenderer>().material = highlightMat;
            Invoke("DeactivateEagleEye", eagleEyeDuration);
        }
    }

    void DeactivateEagleEye()
    {
        isEagleEyeActive = false;
        enemy.GetComponent<SkinnedMeshRenderer>().material = originalMat;
    }

    void ActivateJackAuto()
    {
        if (!isJackAutoActive)
        {
            isJackAutoActive = true;
            jackAutoMainCool = 15f;
            Invoke("DeactivateJackAuto", jackAutoMainDuration);
        }
    }

    void DeactivateJackAuto()
    {
        isEagleEyeActive = false;
    }

    void ActivateJackShrapnel()
    {
        if (!isJackShrapnelActive)
        {
            isJackShrapnelActive = true;
            jackUltShrapnelCool = 20f;
            Invoke("DeactivateJackShrapnel", jackUltShrapnelDuration);
        }
    }

    void DeactivateJackShrapnel()
    {
        isJackShrapnelActive = false;
    }

    void UseStormOfArrows()
    {
        isStormOfArrowsActive = true;
        stormOfArrowsCooldown = 30f;
    }

    void ResetStormOfArrowsCooldown()
    {
        isStormOfArrowsReady = true;
        Debug.Log("Storm of Arrows ready to use.");
    }

    void updateCooldown()
    {
        if (!isEagleEyeActive && gameObject.name == "Fiona")
        {
            eagleEyeCool--;
        }
        if (!isStormOfArrowsActive && gameObject.name == "Fiona")
        {
            stormOfArrowsCooldown--;
        }
        if (!isJackAutoActive && gameObject.name == "Jack")
        {
            jackAutoMainCool--;
        }
        if (!isJackShrapnelActive && gameObject.name == "Jack")
        {
            jackUltShrapnelCool--;
        }
    }
}

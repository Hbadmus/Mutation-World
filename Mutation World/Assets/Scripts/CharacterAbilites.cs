using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    
    public Image abilityQ;
    public Image abilityE;
    

    // Start is called before the first frame update
    void Start()
    {

        if (enemy != null)
        {
            originalMat = enemy.GetComponent<SkinnedMeshRenderer>().material;
        }
        
        abilityQ = GameObject.Find("Ability Canvas").transform.Find("AbilityQ").GetComponent<Image>();
        abilityE = GameObject.Find("Ability Canvas").transform.Find("AbilityE").GetComponent<Image>();
        abilityQ.fillAmount = 0;
        abilityE.fillAmount = 0;
        
    }

    // Update is called once per frame
    void Update()
    {

        updateCooldown();
        
        if (gameObject.name.Contains("Fiona"))
        {
            if (Input.GetKeyDown(KeyCode.Q) && !isEagleEyeActive)
            {
                ActivateEagleEye();
            }
            if (Input.GetKeyDown(KeyCode.E) && !isStormOfArrowsActive)
            {
                UseStormOfArrows();
            }

            // Cooldown timer for Q
            if (isEagleEyeActive)
            {
                abilityQ.fillAmount -= 1 / eagleEyeDuration * Time.deltaTime;
                if (abilityQ.fillAmount < 0)
                {
                    DeactivateEagleEye();
                    abilityQ.fillAmount = 0;
                    
                }
            }

            // Cooldown timer for E
            if (isStormOfArrowsActive)
            {
                abilityE.fillAmount -= 1 / stormOfArrowsCooldown * Time.deltaTime;
                if (abilityE.fillAmount < 0)
                {
                    abilityE.fillAmount = 0;
                    isStormOfArrowsActive = false;
                }
            }
            
        }
        if (gameObject.name.Contains("Jack"))
        {
            if (Input.GetKeyDown(KeyCode.Q) && jackAutoMainCool <= 0)
            {
                ActivateJackAuto();
            }
            if (Input.GetKeyDown(KeyCode.E) && jackUltShrapnelCool <=0) 
            {
                ActivateJackShrapnel();
            }

            // Cooldown timer for Q
            if (isJackAutoActive)
            {
                abilityQ.fillAmount -= 1 / jackAutoMainCool * Time.deltaTime;
                if (abilityQ.fillAmount < 0)
                {
                    DeactivateJackAuto();
                    abilityQ.fillAmount = 0;
                }
            }

            // Cooldown timer for E
            if (isStormOfArrowsActive)
            {
                abilityE.fillAmount -= 1 / jackUltShrapnelCool * Time.deltaTime;
                if (abilityE.fillAmount < 0)
                {
                    abilityE.fillAmount = 0;
                    isJackShrapnelActive = false;
                }
            }
        }
    }

    // Fiona's ability
    void ActivateEagleEye()
    {

        isEagleEyeActive = true;
        abilityQ.fillAmount = 1;
        if (enemy)
        {
            enemy.GetComponent<SkinnedMeshRenderer>().material = highlightMat;
        }

        Invoke("DeactivateEagleEye", eagleEyeDuration);

    }

    void DeactivateEagleEye()
    {
        isEagleEyeActive = false;
        if (enemy)
        {
            enemy.GetComponent<SkinnedMeshRenderer>().material = originalMat;
        }

    }

    void UseStormOfArrows()
    {
        abilityE.fillAmount = 1;
        isStormOfArrowsActive = true;
    }

    void ResetStormOfArrowsCooldown()
    {
        isStormOfArrowsReady = true;
        Debug.Log("Storm of Arrows ready to use.");
    }

    // Jack's ability
    void ActivateJackAuto()
    {
        if (!isJackAutoActive)
        {
            abilityQ.fillAmount = 1;
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
            abilityE.fillAmount = 1;
            isJackShrapnelActive = true;
            jackUltShrapnelCool = 20f;
            Invoke("DeactivateJackShrapnel", jackUltShrapnelDuration);
        }
    }

    void DeactivateJackShrapnel()
    {
        isJackShrapnelActive = false;
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

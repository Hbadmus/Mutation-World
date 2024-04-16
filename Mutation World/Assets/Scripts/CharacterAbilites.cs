using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAbilites : MonoBehaviour
{
    public float eagleEyeDuration = 5f;
    public float eagleEyeCool = 10f;
    public float stormOfArrowsDuration = 5f;
    public float stormOfArrowsCooldown = 30f;
    public float stormOfArrowsRadius = 5f;
    public float stormOfArrowsDamage = 50f;
    public float jackAutoMainDuration = 7f;
    public float jackAutoMainCool = 15f;
    public float jackUltShrapnelDuration = 7f;
    public float jackUltShrapnelCool = 15f;
    public float aceQablity = 8f;
    public float aceQcd = 15f;
    public float aceEablity = 5f;
    public float aceEcd = 20f;

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
        abilityQ.fillAmount = 1;
        abilityE.fillAmount = 1;
        
    }

    // Update is called once per frame
    void Update()
    {

        updateCooldown();

        if (gameObject.name.Contains("Fiona"))
        {
            if (Input.GetKeyDown(KeyCode.Q) && eagleEyeCool <= 0)
            {
                ActivateEagleEye();
            }
            if (Input.GetKeyDown(KeyCode.E) && stormOfArrowsCooldown <= 0)
            {
                UseStormOfArrows();
            }

            // Cooldown timer for Q
            if (abilityQ.fillAmount > 0 || isEagleEyeActive)
            {
                abilityQ.fillAmount = eagleEyeCool / 10f;
                if (abilityQ.fillAmount <= 0)
                {
                    DeactivateEagleEye();
                    abilityQ.fillAmount = 0;

                }
            }

            // Cooldown timer for E
            if (abilityE.fillAmount > 0 || isStormOfArrowsActive)
            {
                abilityE.fillAmount = stormOfArrowsCooldown / 30f;
                if (abilityE.fillAmount <= 0)
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

            if (abilityQ.fillAmount > 0 || isJackAutoActive)
            {
                // Cooldown timer for Q
                abilityQ.fillAmount = jackAutoMainCool / 15f;
                if (abilityQ.fillAmount <= 0)
                {
                    DeactivateJackAuto();
                    abilityQ.fillAmount = 0;
                }
            }
            if (abilityE.fillAmount > 0 || isJackShrapnelActive)
            {
                // Cooldown timer for E
                abilityE.fillAmount = jackUltShrapnelCool / 15f;
                if (abilityE.fillAmount <= 0)
                {
                    abilityE.fillAmount = 0;
                    isJackShrapnelActive = false;
                }
            }
        }

        if (gameObject.name.Contains("Ace"))
        {

        }
    }

    // Fiona's ability
    void ActivateEagleEye()
    {

        isEagleEyeActive = true;
        abilityQ.fillAmount = 1;
        eagleEyeCool = 10f;
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
        stormOfArrowsCooldown = 30f;
        isStormOfArrowsActive = true;
        Invoke("DeactivateStorm", jackAutoMainDuration);
    }

    void DeactivateStorm()
    {
        isStormOfArrowsActive = false;
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
        isJackAutoActive = false;
    }

    void ActivateJackShrapnel()
    {
        if (!isJackShrapnelActive)
        {
            abilityE.fillAmount = 1;
            isJackShrapnelActive = true;
            jackUltShrapnelCool = 15f;
            Invoke("DeactivateJackShrapnel", jackUltShrapnelDuration);
        }
    }

    void DeactivateJackShrapnel()
    {
        isJackShrapnelActive = false;
    }

    void updateCooldown()
    {
        if (!isEagleEyeActive && gameObject.name.Contains("Fiona"))
        {
            eagleEyeCool -= Time.deltaTime;
        }
        if (!isStormOfArrowsActive && gameObject.name.Contains("Fiona"))
        {
            stormOfArrowsCooldown -= Time.deltaTime;
        }
        if (!isJackAutoActive && gameObject.name.Contains("Jack"))
        {
            jackAutoMainCool -= Time.deltaTime;
        }
        if (!isJackShrapnelActive && gameObject.name.Contains("Jack"))
        {
            jackUltShrapnelCool -= Time.deltaTime;
        }
    }
}

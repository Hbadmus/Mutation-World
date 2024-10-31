using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAbilites : MonoBehaviour
{
    // Ability durations and cooldowns
    [Header("Fiona's Settings")]
    public float eagleEyeDuration = 5f;
    public float eagleEyeCool = 10f;
    public float stormOfArrowsDuration = 5f;
    public float stormOfArrowsCooldown = 30f;

    [Header("Jack's Settings")]
    public float jackAutoMainDuration = 7f;
    public float jackAutoMainCool = 15f;
    public float jackUltShrapnelDuration = 7f;
    public float jackUltShrapnelCool = 15f;

    // Ability states
    public static bool isEagleEyeActive = false;
    public static bool isStormOfArrowsActive = false;
    public static bool isJackAutoActive = false;
    public static bool isJackShrapnelActive = false;

    // UI elements for abilities
    public Image abilityQ; // Storm of Arrows
    public Image abilityE; // Eagle Eye

    // Start is called before the first frame update
    void Start()
    {
        // Initialize ability UI elements
        abilityQ = GameObject.Find("Ability Canvas").transform.Find("AbilityQ").GetComponent<Image>();
        abilityE = GameObject.Find("Ability Canvas").transform.Find("AbilityE").GetComponent<Image>();
        
        SetAbilityUIColor(abilityE, Color.gray);
        SetAbilityUIColor(abilityQ, Color.gray);

        abilityQ.fillAmount = 1;
        abilityE.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Set the button text for abilities
        UpdateAbilityText();

        // Update cooldowns
        UpdateCooldown();

        // Check for ability inputs based on character name
        if (gameObject.name.Contains("Fiona"))
        {
            HandleFionaAbilities();
        }
        else if (gameObject.name.Contains("Jack"))
        {
            HandleJackAbilities();
        }
    }

    private void HandleFionaAbilities()
    {
        if (Input.GetKeyDown(KeyCode.E) && eagleEyeCool <= 0)
        {
            ActivateEagleEye();
        }
        if (Input.GetKeyDown(KeyCode.Q) && stormOfArrowsCooldown <= 0)
        {
            UseStormOfArrows();
        }

        UpdateAbilityCooldown(abilityE, ref eagleEyeCool, isEagleEyeActive, 10f);
        UpdateAbilityCooldown(abilityQ, ref stormOfArrowsCooldown, isStormOfArrowsActive, 30f);
    }

    private void HandleJackAbilities()
    {
        if (Input.GetKeyDown(KeyCode.E) && jackAutoMainCool <= 0)
        {
            ActivateJackAuto();
        }
        if (Input.GetKeyDown(KeyCode.Q) && jackUltShrapnelCool <= 0)
        {
            ActivateJackShrapnel();
        }

        UpdateAbilityCooldown(abilityE, ref jackAutoMainCool, isJackAutoActive, 15f);
        UpdateAbilityCooldown(abilityQ, ref jackUltShrapnelCool, isJackShrapnelActive, 25f);
    }

    private void UpdateAbilityCooldown(Image abilityImage, ref float cooldown, bool isActive, float maxCooldown)
    {
        if (abilityImage.fillAmount > 0 || isActive)
        {
            abilityImage.fillAmount = cooldown / maxCooldown;

            if (abilityImage.fillAmount <= 0)
            {
                cooldown = 0;
            }
        }
    }

    private void UpdateAbilityText()
    {
        // Update the ability button text
        GameObject.Find("Ability Canvas").transform.Find("AbilityQ")
            .transform.Find("Q").GetComponent<Text>().text = "Q";
        GameObject.Find("Ability Canvas").transform.Find("AbilityE")
            .transform.Find("E").GetComponent<Text>().text = "E";
    }

    private void SetAbilityUIColor(Image abilityImage, Color color)
    {
        abilityImage.color = color;
        abilityImage.transform.Find("bcktxt").GetComponent<Image>().color = color;
    }

    // Fiona's ability methods
    void ActivateEagleEye()
    {
        isEagleEyeActive = true;
        abilityE.fillAmount = 1;
        eagleEyeCool = 10f;
        Invoke("DeactivateEagleEye", eagleEyeDuration);
    }

    void DeactivateEagleEye()
    {
        isEagleEyeActive = false;
    }

    void UseStormOfArrows()
    {
        abilityQ.fillAmount = 1;
        stormOfArrowsCooldown = 30f;
        isStormOfArrowsActive = true;
        Invoke("DeactivateStorm", stormOfArrowsDuration);
    }

    void DeactivateStorm()
    {
        isStormOfArrowsActive = false;
    }

    // Jack's ability methods
    void ActivateJackAuto()
    {
        if (!isJackAutoActive)
        {
            abilityE.fillAmount = 1;
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
            abilityQ.fillAmount = 1;
            isJackShrapnelActive = true;
            jackUltShrapnelCool = 25f;
            Invoke("DeactivateJackShrapnel", jackUltShrapnelDuration);
        }
    }

    void DeactivateJackShrapnel()
    {
        isJackShrapnelActive = false;
    }

    private void UpdateCooldown()
    {
        // Update cooldowns for all abilities
        if (!isEagleEyeActive && gameObject.name.Contains("Fiona"))
        {
            eagleEyeCool -= Time.deltaTime;
            if (eagleEyeCool <= 0)
            {
                SetAbilityUIColor(abilityE, Color.red);
            }
            else
            {
                SetAbilityUIColor(abilityE, Color.gray);
            }
        }

        if (!isStormOfArrowsActive && gameObject.name.Contains("Fiona"))
        {
            stormOfArrowsCooldown -= Time.deltaTime;
            if (stormOfArrowsCooldown <= 0)
            {
                SetAbilityUIColor(abilityQ, Color.red);
            }
            else
            {
                SetAbilityUIColor(abilityQ, Color.gray);
            }
        }

        if (!isJackAutoActive && gameObject.name.Contains("Jack"))
        {
            jackAutoMainCool -= Time.deltaTime;
            if (jackAutoMainCool <= 0)
            {
                SetAbilityUIColor(abilityE, Color.red);
            }
            else
            {
                SetAbilityUIColor(abilityE, Color.gray);
            }
        }

        if (!isJackShrapnelActive && gameObject.name.Contains("Jack"))
        {
            jackUltShrapnelCool -= Time.deltaTime;
            if (jackUltShrapnelCool <= 0)
            {
                SetAbilityUIColor(abilityQ, Color.red);
            }
            else
            {
                SetAbilityUIColor(abilityQ, Color.gray);
            }
        }
    }
}

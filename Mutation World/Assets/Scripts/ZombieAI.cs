using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    private Animator anim;
    private AudioSource audioSource; // Reference to the AudioSource component

    [SerializeField] 
    private AudioClip[] gruntSounds; // Array of audio clips for zombie grunts
    [SerializeField] 
    private AudioClip[] deathSounds; // Array of audio clips for zombie death sounds

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
    }

    public void SetMovementAnimationTrigger()
    {
        anim.SetTrigger("movementAnimationTrigger");
        PlayGruntSound(); // Play grunt sound when moving
    }

    public void SetAttackAnimation(bool isAttacking)
    {
        anim.SetBool("attackPlayer", isAttacking);
        PlayGruntSound(); // Play grunt sound when attacking
    }

    public void SetIdleAnimation(bool cantSee)
    {
        Debug.Log(anim.GetBool("CantSee"));
        anim.SetBool("CantSee", cantSee);
        PlayGruntSound(); 
        Debug.Log(anim.GetBool("CantSee"));
    }

    public void SetZeroHealthAnimation()
    {
        anim.SetBool("0Health", true);
        PlayDeathSound(); // Play death sound when dying
    }

    private void PlayGruntSound()
    {
        // Generate a random number between 0 and 99
        int chance = Random.Range(0, 100);
        
        // Play a grunt sound 10% of the time (when chance is less than 10)
        if (chance < 6 && gruntSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, gruntSounds.Length);
            audioSource.PlayOneShot(gruntSounds[randomIndex]);
        }
    }

    private void PlayDeathSound()
    {
        // Generate a random number between 0 and 99
        int chance = Random.Range(0, 100);

        // Play one of the two death sounds 80% of the time (when chance is less than 80)
        if (chance < 80 && deathSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, deathSounds.Length);
            audioSource.PlayOneShot(deathSounds[randomIndex]);
        }
    }
}

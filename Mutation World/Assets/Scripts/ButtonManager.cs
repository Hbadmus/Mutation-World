using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    // Audio Clip for Button Interaction
    [Header("Button Audio Settings")]
    [SerializeField] private AudioClip button;       // Audio clip to play when the button is pressed
    private AudioSource audioSource;                 // Reference to the AudioSource component

    void Start()
    {
        // Get or add an AudioSource component to the GameObject
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource if it doesn't exist
        }
    }

    // Method to play the button audio clip
    public void PlayClip()
    {
        // Check if button audio clip and audio source are assigned
        if (button != null && audioSource != null)
        {
            audioSource.PlayOneShot(button); // Play the button audio clip
        }
    }
}

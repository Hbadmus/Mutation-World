using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;   // Static instance to prevent multiple instances
    private AudioSource audioSource;        // Reference to the AudioSource component

    public AudioClip beginningAudio;        // Clip for the beginning audio
    public AudioClip mainAudio;             // Clip for the main audio
    public AudioClip bossAudio;
    private bool isBeginningAudioPlaying = true; // Flag to check if beginning audio is playing
    private bool isMainAudioPlaying = false;
    public EnemySpawner spawner;
    void Awake()
    {
        // Check if an instance of AudioManager already exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);            // Destroy duplicate instances
        }
        else
        {
            instance = this;                // Set this instance as the static instance
            DontDestroyOnLoad(gameObject);  // Keep this AudioManager across scenes
            audioSource = GetComponent<AudioSource>(); // Get AudioSource component

            // Play the beginning audio
            audioSource.clip = beginningAudio;
            audioSource.Play();
        }
    }

    void Start() {

    }

    void Update()
    {
                if (SceneManager.GetActiveScene().buildIndex == 2) {
            GameObject spawnerObject = GameObject.Find("Spawner");
            if (spawnerObject != null)
            {
             spawner = spawnerObject.GetComponent<EnemySpawner>(); 
            }
        }
        // Check if the beginning audio has finished playing
        if (isBeginningAudioPlaying && !audioSource.isPlaying)
        {
            // Switch to main audio
            audioSource.clip = mainAudio;
            audioSource.loop = true;              // Enable looping for main audio
            audioSource.Play();
            isBeginningAudioPlaying = false;      // Update the flag
            isMainAudioPlaying = true;       
        } else if (isMainAudioPlaying && SceneManager.GetActiveScene().buildIndex == 2 && spawner.bossHere) {
            audioSource.Stop();
            audioSource.clip = bossAudio;
            audioSource.loop = true;              // Enable looping for main audio
            audioSource.Play();
            isMainAudioPlaying = false;           
        }
    }
}

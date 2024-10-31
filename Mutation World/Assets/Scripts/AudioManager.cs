using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance; // Static instance to ensure only one AudioManager exists across scenes

    // Audio Source and Clips
    private AudioSource audioSource;      // Reference to the AudioSource component
    [Header("Audio Clips")]
    [SerializeField] private AudioClip beginningAudio; // Clip for the beginning audio
    [SerializeField] private AudioClip mainAudio;      // Clip for the main background audio
    [SerializeField] private AudioClip bossAudio;      // Clip for boss battle audio

    // Flags for Audio Control
    private bool isBeginningAudioPlaying = true; // Tracks if beginning audio is currently playing
    private bool isMainAudioPlaying = false;     // Tracks if main audio is currently playing

    // Spawner Reference
    private EnemySpawner spawner; // Reference to EnemySpawner

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances if one already exists
        }
        else
        {
            instance = this;                // Set this instance as the main AudioManager
            DontDestroyOnLoad(gameObject);  // Ensure persistence across scenes

            audioSource = GetComponent<AudioSource>(); // Cache AudioSource component

            // Begin playback with the initial audio clip
            audioSource.clip = beginningAudio;
            audioSource.Play();
        }
    }

    void Update()
    {
        // Check if in scene 2 and locate the EnemySpawner if not already assigned
        if (SceneManager.GetActiveScene().buildIndex == 2 && spawner == null)
        {
            GameObject spawnerObject = GameObject.Find("Spawner");
            if (spawnerObject != null)
            {
                spawner = spawnerObject.GetComponent<EnemySpawner>();
            }
        }

        // Transition from beginning audio to main audio once the beginning audio finishes
        if (isBeginningAudioPlaying && !audioSource.isPlaying)
        {
            audioSource.clip = mainAudio;
            audioSource.loop = true;       // Enable looping for continuous main audio
            audioSource.Play();
            isBeginningAudioPlaying = false;
            isMainAudioPlaying = true;
        }
        // Switch to boss audio if in scene 2 and boss is present
        else if (isMainAudioPlaying && SceneManager.GetActiveScene().buildIndex == 2 && spawner.bossHere)
        {
            audioSource.Stop();
            audioSource.clip = bossAudio;
            audioSource.loop = true;       // Enable looping for boss audio
            audioSource.Play();
            isMainAudioPlaying = false;
        }
    }
}

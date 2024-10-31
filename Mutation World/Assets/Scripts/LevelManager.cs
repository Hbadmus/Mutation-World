using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance; // Singleton instance of LevelManager

    [SerializeField] private GameObject bossPopUp; // Prompt for boss encounter
    [SerializeField] private GameObject winUI; // UI displayed upon winning
    [SerializeField] private GameObject lossUI; // UI displayed upon losing
    [SerializeField] private AudioClip lose; // Audio clip to play on losing
    [SerializeField] private AudioClip win; // Audio clip to play on winning

    public bool isGameOver; // Flag to check if the game is over
    private int sceneIndex; // Current active scene index
    private GameObject bossObject; // Reference to the boss object
    public GameObject reloadPopUp; // Prompt for player to reload

    private float timer = 0; // Timer to control UI visibility

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scene loads
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void Start()
    {
        // Initialize UI and game state
        winUI.SetActive(false);
        lossUI.SetActive(false);
        isGameOver = false;

        // Find UI elements if not assigned in Inspector
        if (reloadPopUp == null)
            reloadPopUp = GameObject.FindGameObjectWithTag("Reload");

        if (bossPopUp == null)
            bossPopUp = GameObject.FindGameObjectWithTag("BossPop");
    }

    void Update()
    {
        // Check if the game is over, exit if true
        if (isGameOver) return;

        // Get the current scene index
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Find the boss object in the scene
        bossObject = GameObject.FindWithTag("Boss");

        // If in the boss battle scene
        if (sceneIndex == 2 && bossObject != null)
        {
            timer += Time.deltaTime; // Increment the timer
            bossPopUp.SetActive(true); // Show the boss prompt

            // Check if the boss's health is below or equal to zero
            EnemyHealth enemyHealth = bossObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null && enemyHealth.currentHealth <= 0)
            {
                PlayerWon(); // Call win method if the boss is defeated
            }
        }

        // Hide the boss prompt after 3 seconds
        if (timer > 3)
        {
            bossPopUp.SetActive(false);
        }
    }

    public void PlayerDied()
    {
        // Handle player death
        isGameOver = true; // Set game over flag
        lossUI.SetActive(true); // Show loss UI
        AudioSource.PlayClipAtPoint(lose, transform.position); // Play lose sound
        DisablePlayerAndEnemies(); // Disable player and enemies
        EnemyBehavior.ResetScore(); // Reset enemy scores
        Invoke("MainMenu", 2); // Return to main menu after 2 seconds
    }

    public void PlayerWon()
    {
        // Handle player victory
        isGameOver = true; // Set game over flag
        winUI.SetActive(true); // Show win UI
        AudioSource.PlayClipAtPoint(win, transform.position); // Play win sound
        DisablePlayerAndEnemies(); // Disable player and enemies

        Application.Quit(); // Exit the application (for builds)
    }

    void DisablePlayerAndEnemies()
    {
        // Disable player and enemy components
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.GetComponent<MouseLook>().enabled = false; // Disable camera look
        camera.GetComponent<ShootWeapon>().enabled = false; // Disable shooting

        if (player != null)
        {
            player.GetComponent<PlayerController>().enabled = false; // Disable player controls
        }

        // Disable all enemy game objects
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            enemy.SetActive(false);

        // Disable all boss game objects
        foreach (GameObject boss in GameObject.FindGameObjectsWithTag("Boss"))
            boss.SetActive(false);
    }

    public void MainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene(0);
    }
}

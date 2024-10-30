using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject reloadPopUp; // tells the player to reload
    public GameObject bossPopUp;
    public GameObject winUI;
    public GameObject lossUI;
    public AudioClip lose;
    public AudioClip win;
    public bool isGameOver;
    int sceneIndex;
    GameObject bossObject;

    float timer = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        winUI.SetActive(false);
        lossUI.SetActive(false);
        isGameOver = false;

        if (reloadPopUp == null)
        {
            reloadPopUp = GameObject.FindGameObjectWithTag("Reload");
        }

        if (bossPopUp == null)
        {
            bossPopUp = GameObject.FindGameObjectWithTag("BossPop");
        }
    }

    void Update()
    {
        if (!isGameOver)
        {
            sceneIndex = SceneManager.GetActiveScene().buildIndex;
            bossObject = GameObject.FindWithTag("Boss");

            if (sceneIndex == 2)
            {
                if (bossObject != null)
                {
                    timer += Time.deltaTime;

                    bossPopUp.gameObject.SetActive(true);
                    EnemyHealth enemyHealth = bossObject.GetComponent<EnemyHealth>();

                    if (enemyHealth != null && enemyHealth.currentHealth <= 0)
                    {
                        PlayerWon();
                    }
                }
            }
        }

        if (timer > 3)
        {
            bossPopUp.gameObject.SetActive(false);
        }

        Debug.Log(timer);
    }

    public void PlayerDied()
    {
        isGameOver = true;
        lossUI.SetActive(true);
        AudioSource.PlayClipAtPoint(lose, transform.position);
        DisablePlayerAndEnemies();
        EnemyBehavior.ResetScore();
        Debug.Log("hi");
        Invoke("EndGame", 2);
    }

    public void PlayerWon()
    {
        isGameOver = true;
        winUI.SetActive(true);
        AudioSource.PlayClipAtPoint(win, transform.position);
        DisablePlayerAndEnemies();

        // Freeze the player's movement
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Stop the player's Rigidbody movement
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.velocity = Vector3.zero; // Set velocity to zero
                playerRb.isKinematic = true; // Make the Rigidbody kinematic to prevent further physics interactions
            }

            // Disable player movement script
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false; // Disable movement controls
            }
        }

        // Call EndGame after a delay or immediately
        Application.Quit();
    }

    void DisablePlayerAndEnemies()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.GetComponent<MouseLook>().enabled = false;
        camera.GetComponent<ShootWeapon>().enabled = false; 
        if (player != null) {
            player.GetComponent<PlayerController>().enabled = false; 
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }

        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        foreach (GameObject boss in bosses)
        {
            boss.SetActive(false);
        }
    }

    public void EndGame()
    {
        Application.Quit();
    }
}

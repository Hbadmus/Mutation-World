using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    
    public GameObject reloadPopUp; // tells the player to reload
    public GameObject winUI;
    public GameObject lossUI;
    public AudioClip lose;
    public AudioClip win;
    public bool isGameOver;
    public float currentTime = 0.0f; // The current time, not the time needed to win
    public float winTime = 60f; // This should be altered for the final, but this is low for testing


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
        if(reloadPopUp == null)
        {
            reloadPopUp = GameObject.FindGameObjectWithTag("Reload");
        }
        winUI.SetActive(false);
        lossUI.SetActive(false);
        isGameOver = false;
        
    }

    void Update()
    {
        if(!isGameOver) {
        currentTime += Time.deltaTime;
        }
        if (currentTime > winTime) 
        {
           PlayerWon();
        }


    }

    public void PlayerDied()
    {
        isGameOver = true;
        lossUI.SetActive(true);
        AudioSource.PlayClipAtPoint(lose, transform.position);
        DisablePlayerAndEnemies();
        Invoke("RestartGame", 2);
    }

    public void PlayerWon()
    {
        isGameOver = true;
        winUI.SetActive(true);
        AudioSource.PlayClipAtPoint(win, transform.position);
        DisablePlayerAndEnemies();
        if (SceneManager.GetActiveScene().buildIndex < 3)
        {
            Invoke("NextLevel", 2);
        }
    }

    void DisablePlayerAndEnemies()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.GetComponent<MouseLook>().enabled = false;
        camera.GetComponent<ShootWeapon>().enabled = false; 
        player.GetComponent<PlayerController>().enabled = false; 
        
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

    // Use this method to restart the game or load another scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

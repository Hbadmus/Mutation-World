using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject winUI;
    public GameObject lossUI;
    public AudioClip lose;
    public AudioClip win;
    public bool isGameOver;
    public float timer = 0;


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
        
    }

    void Update()
    {
        if(!isGameOver) {
        timer += Time.deltaTime;
        }


    }

    public void PlayerDied()
    {
        isGameOver = true;
        lossUI.SetActive(true);
        AudioSource.PlayClipAtPoint(lose, transform.position);
        DisablePlayerAndEnemies();
    }

    public void PlayerWon()
    {
        isGameOver = true;
        winUI.SetActive(true);
        AudioSource.PlayClipAtPoint(win, transform.position);
        DisablePlayerAndEnemies();
    }

    void DisablePlayerAndEnemies()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.GetComponent<MouseLook>().enabled = false;
        camera.GetComponent<ShootWeapon>().enabled = false; // Assuming your script is called ShootWeapon
        player.GetComponent<PlayerController>().enabled = false; // Disable player movement script here
        
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
}

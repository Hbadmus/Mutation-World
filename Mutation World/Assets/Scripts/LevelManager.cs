using TMPro;
using Unity.VisualScripting;
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
    public float currentTime = 0.0f; 
    public float winTime = 60f; 
     public Text killCountTxt;
     public Text timer;
     int killCount;
     int sceneIndex;
     GameObject bossObject;
     float countdown = 300.00f;
    


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
        //killCountTxt.gameObject.SetActive(true);
        //timer.gameObject.SetActive(true);
        SetTimerText();

        SetScoreText();

        if(reloadPopUp == null)
        {
            reloadPopUp = GameObject.FindGameObjectWithTag("Reload");
        }


        

        
    }

void Update()
{




    if (!isGameOver)
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(sceneIndex == 2) {
            SetScoreText();
    }
         bossObject = GameObject.FindWithTag("Boss");
        if (sceneIndex == 3)
        {
       if(countdown > 0) {
        countdown -= Time.deltaTime;
        } else {
            countdown = 0.0f;

            PlayerWon();
        }
        SetTimerText();
        SetScoreText();
        }
        else if (sceneIndex == 2)
        {
            SetScoreText();

            if (killCount >= 50)
            {
                PlayerWon();
            }
        }
        else if (sceneIndex == 1)
        {
        
            if (bossObject != null)
            {
                EnemyHealth enemyHealth = bossObject.GetComponent<EnemyHealth>();

                if (enemyHealth != null && enemyHealth.currentHealth <= 0)
                {
                    PlayerWon();
                }
            }
        }
    }
}


        void SetScoreText() {
        killCount = EnemyBehavior.GetScore();
        Debug.Log("killcount:" + killCount);
        killCountTxt.text = "Kill Count: " + killCount.ToString();
        Debug.Log(killCountTxt.text);
    }

        void SetTimerText() {
        timer.text = countdown.ToString("f2");
    }

    public void PlayerDied()
    {
        isGameOver = true;
        lossUI.SetActive(true);
        AudioSource.PlayClipAtPoint(lose, transform.position);
        DisablePlayerAndEnemies();
        EnemyBehavior.ResetScore();
        Debug.Log("hi");
        Invoke("RestartGame", 2);
    }

    public void PlayerWon()
    {
        isGameOver = true;
        winUI.SetActive(true);
        AudioSource.PlayClipAtPoint(win, transform.position);
        DisablePlayerAndEnemies();

            Application.Quit();
        
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

    // Use this method to restart the game 
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

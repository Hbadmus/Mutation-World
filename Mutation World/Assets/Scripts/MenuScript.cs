using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject objectToDisable;

    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void DisableAfterDelay() {
        StartCoroutine(SetInactiveAfterDelay());
    }

    private IEnumerator SetInactiveAfterDelay() {
        yield return new WaitForSeconds(0.04f); // Wait for 0.5 seconds
        objectToDisable.SetActive(false); // Set the object to inactive
    }
}

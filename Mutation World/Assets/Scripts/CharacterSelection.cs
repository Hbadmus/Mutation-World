using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters; // Array of character GameObjects
    public GameObject[] weapons;     // Array of weapon GameObjects

    public int selectedCharacter = 0; // Index of the currently selected character

    public void NextCharacter()
    {
        // Deactivate current character and its weapon
        characters[selectedCharacter].SetActive(false);
        weapons[selectedCharacter].SetActive(false); // Use selectedCharacter index for weapon

        // Increment selected character index and loop back if needed
        selectedCharacter = (selectedCharacter + 1) % characters.Length;

        // Activate new character and its weapon
        characters[selectedCharacter].SetActive(true);
        weapons[selectedCharacter].SetActive(true); // Use selectedCharacter index for weapon
    }

    public void PreviousCharacter()
    {
        // Deactivate current character and its weapon
        characters[selectedCharacter].SetActive(false);
        weapons[selectedCharacter].SetActive(false); // Use selectedCharacter index for weapon

        // Decrement selected character index and loop back if needed
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }

        // Activate new character and its weapon
        characters[selectedCharacter].SetActive(true);
        weapons[selectedCharacter].SetActive(true); // Use selectedCharacter index for weapon
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters; // Array of character GameObjects
    public GameObject[] weapons;     // Array of weapon GameObjects
    public Text characterNameText;   // UI Text to show the selected character's name

    private int selectedCharacter = 0; // Index of the currently selected character

    private void Start()
    {
        // Ensure only the first character and weapon are active at start
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(i == selectedCharacter);
            weapons[i].SetActive(i == selectedCharacter);
        }
        UpdateCharacterName();
    }

    public void NextCharacter()
    {
        ChangeCharacter(1);
    }

    public void PreviousCharacter()
    {
        ChangeCharacter(-1);
    }

    private void ChangeCharacter(int direction)
    {
        // Deactivate current character and its weapon
        characters[selectedCharacter].SetActive(false);
        weapons[selectedCharacter].SetActive(false);

        // Update selected character index
        selectedCharacter = (selectedCharacter + direction + characters.Length) % characters.Length;

        // Activate new character and its weapon
        characters[selectedCharacter].SetActive(true);
        weapons[selectedCharacter].SetActive(true);

        UpdateCharacterName();
    }

    private void UpdateCharacterName()
    {
        if (characterNameText != null)
        {
            characterNameText.text = characters[selectedCharacter].name; // Assuming character name is in GameObject name
        }
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }
}

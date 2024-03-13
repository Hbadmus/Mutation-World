using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characterMeshes;

    private GameObject selectedCharacter;

    public void SelectCharacter(int index)
    {
        if (selectedCharacter != null)
        {
            selectedCharacter.SetActive(false);
        }

        selectedCharacter = characterMeshes[index];
        selectedCharacter.SetActive(true);
    }
}

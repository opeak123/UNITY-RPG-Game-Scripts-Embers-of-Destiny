using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameCharacter : MonoBehaviour
{
    public PlayerState character;
    public InputField inputField;
    public Text nameButtonText;

    public void NameMyCharacter()
    {
        character.CharacterName = inputField.text;

        if(character.CharacterName == "")
        {
            character.CharacterName = "Nameless";
        }

        nameButtonText.text = character.CharacterName;
    }

}

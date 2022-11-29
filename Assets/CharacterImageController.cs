using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterImageController : MonoBehaviour {
    [SerializeField] private Sprite milfRegular;
    [SerializeField] private Image image;
    
    
    public void Disable() {
        image.enabled = false;
    }

    public void SetCharacter(RomanceCharacters character, Expression expression) {
        image.enabled = true;
        if (character == RomanceCharacters.MILF) {
            image.sprite = milfRegular;
        } else if (character == RomanceCharacters.BAD_BOY) {
            image.sprite = null;
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterImageController : MonoBehaviour {
    [SerializeField] private Sprite milfNeutral;
    [SerializeField] private Sprite milfAngry;
    [SerializeField] private Sprite milfSmile;
    [SerializeField] private Sprite milfSmileWink;
    [SerializeField] private Sprite milfSurpriseSlight;
    
    [SerializeField] private Sprite badBoyNeutral;
    [SerializeField] private Sprite badBoyAnnoyed;
    [SerializeField] private Sprite badBoyEmbarassed;
    [SerializeField] private Sprite badBoySmirk;
    
    [SerializeField] private Image image;
    
    
    public void Disable() {
        image.enabled = false;
    }

    public void SetCharacter(RomanceCharacters character, Expression expression) {
        image.enabled = true;
        
        if (character == RomanceCharacters.MILF) {
            switch (expression) {
                case Expression.Angry:
                    image.sprite = milfAngry;
                    break;
                case Expression.Smile:
                    image.sprite = milfSmile;
                    break;
                case Expression.SmileWink:
                    image.sprite = milfSmileWink;
                    break;
                case Expression.Surprise:
                    image.sprite = milfSurpriseSlight;
                    break;
                default:
                    image.sprite = milfNeutral;
                    break;
            }
        } else if (character == RomanceCharacters.BAD_BOY) {
            switch (expression) {
                case Expression.Smirk:
                    image.sprite = badBoySmirk;
                    break;
                case Expression.Embarassed:
                    image.sprite = badBoyEmbarassed;
                    break;
                case Expression.Annoyed:
                    image.sprite = badBoyAnnoyed;
                    break;
                default:
                    image.sprite = badBoyNeutral;
                    break;
            }
        } else {
            image.enabled = false;
        }
    }

}

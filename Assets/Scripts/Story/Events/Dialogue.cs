using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue : StoryEvent
{
    public RomanceCharacters character;
    public string dialogue;
    public Expression expression;
    public static GUIStyle DIALOGUE_BOX_STYLE = new();
    private Coroutine ongoingAnimation;

    public static void SetupDialogue()
    {
        DIALOGUE_BOX_STYLE.alignment = TextAnchor.MiddleCenter;
        DIALOGUE_BOX_STYLE.normal.background = MakeTextureForNodeTitles(new Color(0.5f, 0.5f, 0.8f));
    }

    public override SavableEvent CreateSavableEvent()
    {
        SavableDialogue savableDialogue = new SavableDialogue(this);
        return savableDialogue;
    }

    public override void RenderNodeContent(Rect rect)
    {
        Rect characterText = new Rect(rect.x, rect.y + TITLE_HEIGHT, rect.width, HEIGHT - TITLE_HEIGHT);
        GUI.Box(characterText, "Character: " + character, CONTENT_STYLE);

        Rect internalText = new Rect(rect.x, rect.y + TITLE_HEIGHT + CHARACTER_FIELD_HEIGHT + CHARACTER_FIELD_HEIGHT, rect.width,
            HEIGHT - TITLE_HEIGHT - CHARACTER_FIELD_HEIGHT - CHARACTER_FIELD_HEIGHT);
        GUI.Box(internalText, dialogue, CONTENT_STYLE);
        
        Rect expressionRect = new Rect(rect.x, rect.y + TITLE_HEIGHT + CHARACTER_FIELD_HEIGHT, rect.width, HEIGHT - TITLE_HEIGHT - CHARACTER_FIELD_HEIGHT);
        GUI.Box(expressionRect, "Expression: " + expression, CONTENT_STYLE);
    }

    public override GUIStyle GetStyle()
    {
        return DIALOGUE_BOX_STYLE;
    }
}

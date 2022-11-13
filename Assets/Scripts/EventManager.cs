using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventManager : MonoBehaviour {
    
    
    private StoryManager manager;
    
    [SerializeField] private GameObject dialoguePrefab;
    [SerializeField] private GameObject choicePrefab;
    
    private GameObject dialogueObj;
    private Coroutine dialogueAnimation;

    [NonSerialized] public Keybinds keybinds;

    private bool canRun = false;

    private void Start() {
        keybinds = new Keybinds();
        keybinds.Enable();
        keybinds.Player.Click.started += ClickUpdate;
        manager = GameObject.FindGameObjectWithTag("StoryManager").GetComponent<StoryManager>();
        StartDialogue(manager.currentEvent[0] as Dialogue);
    }

    private void ClickUpdate(InputAction.CallbackContext context)
    {
        int id = -1;
        bool ranThroughEntireCurrentEvent = UpdateCurrentEvent();
        if (!ranThroughEntireCurrentEvent)
        {
            return;
        }

        manager.TickToNextEvent(id);
        StoryEvent storyEvent = manager.currentEvent[0];
        if (storyEvent is Dialogue dialogue)
        {
            StartDialogue(dialogue);
        } else if (storyEvent is Cutscene cutscene)
        {
            
        } else if (storyEvent is SceneStart sceneStart)
        {
            
        }else if (storyEvent is Option)
        {
            
        }
    }

    private void StartDialogue(Dialogue dialogue)
    {
        dialogueObj = Instantiate(dialoguePrefab, transform);
        StartCoroutine(DialogueAnimation(dialogue));
    }

    //if current event is at end then return true;
    private bool UpdateCurrentEvent()
    {
        StoryEvent storyEvent = manager.currentEvent[0];
        if (storyEvent is Dialogue dialogue)
        {
            UpdateDialogue(dialogue);
        } else if (storyEvent is Cutscene cutscene)
        {
            
        } else if (storyEvent is SceneStart sceneStart)
        {
            
        }else if (storyEvent is Option)
        {
            
        }
        
        return true;
    }

    private void UpdateDialogue(Dialogue dialogue)
    {
        StopCoroutine(DialogueAnimation(dialogue));
        TextMeshProUGUI tmp = dialogueObj.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = fullDialogue;
    }


    private string fullDialogue;
    [SerializeField] private float speed = 0.02f;
    private IEnumerator DialogueAnimation(Dialogue dialogue) {
        int index = 0;
        fullDialogue = dialogue.dialogue;
        TextMeshProUGUI tmp = dialogueObj.GetComponentInChildren<TextMeshProUGUI>();

        while (index < dialogue.dialogue.Length) {
            tmp.text = dialogue.dialogue.Substring(0, index + 1);
            index++;
            yield return new WaitForSeconds(speed);
        }

    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventManager : MonoBehaviour {
    
    
    private StoryEvent _currentStoryEvent;
    
    [SerializeField] private GameObject dialoguePrefab;
    [SerializeField] private GameObject choicePrefab;
    
    private GameObject dialogueObjPrefab;
    
    [NonSerialized] public Keybinds keybinds;

    private Coroutine ongoingAnimationCoroutine;
    
    private void Awake() {
        keybinds = new Keybinds();
        keybinds.Enable();
    }

    private void Start() {
    }

    private void Update() {
        // if (!_currentStoryEvent.GoToNextEvent()) return;
        // _currentStoryEvent = _currentStoryEvent.ChooseNextEvent();
    }

    public bool CreateDialogue(Dialogue dialogue) {
        if (ongoingAnimationCoroutine != null) {
            // EndAnimationEarly();
            return false;
        }
        
        if (dialogueObjPrefab == null) {
            dialogueObjPrefab = Instantiate(dialoguePrefab, transform);
        }

        ongoingAnimationCoroutine = StartCoroutine(DialogueAnimation(dialogue));
        return true;
    }

    private string fullDialogue;
    [SerializeField] private float speed = 0.02f;
    private IEnumerator DialogueAnimation(Dialogue dialogue) {
        int index = 0;
        fullDialogue = dialogue.dialogue;
        TextMeshProUGUI tmp = dialogueObjPrefab.GetComponentInChildren<TextMeshProUGUI>();

        while (index < dialogue.dialogue.Length) {
            tmp.text = dialogue.dialogue.Substring(0, index + 1);
            index++;
            yield return new WaitForSeconds(speed);
        }

        ongoingAnimationCoroutine = null;
    }

    private void EndDialogueAnimationEarly() {
        StopCoroutine(ongoingAnimationCoroutine);
        ongoingAnimationCoroutine = null;
        TextMeshProUGUI tmp = dialogueObjPrefab.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = fullDialogue;
    }
}
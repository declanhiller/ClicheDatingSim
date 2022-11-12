using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventManager : MonoBehaviour {
    
    
    private StoryEvent _currentStoryEvent;

    [SerializeField] private Story story;

    public Story Story => story;

    [SerializeField] private GameObject dialoguePrefab;
    [SerializeField] private GameObject choicePrefab;

    private bool animationCurrentlyPlaying = false;
    
    private GameObject dialogueObj;
    
    [NonSerialized] public Keybinds keybinds;

    private Coroutine ongoingAnimationCoroutine;
    private Type ongoingType;
    
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
        if (animationCurrentlyPlaying) {
            EndAnimationEarly();
            EndDialogueAnimationEarly();
            animationCurrentlyPlaying = false;
            return false;
        }
        
        if (dialogueObj == null) {
            dialogueObj = Instantiate(dialoguePrefab, transform);
        }

        ongoingAnimationCoroutine = StartCoroutine(DialogueAnimation(dialogue));
        ongoingType = typeof(Dialogue);
        return true;
    }

    private string fullDialogue;
    [SerializeField] private float speed = 0.02f;
    private IEnumerator DialogueAnimation(Dialogue dialogue) {
        int index = 0;
        animationCurrentlyPlaying = true;
        fullDialogue = dialogue.dialogue;
        TextMeshProUGUI tmp = dialogueObj.GetComponentInChildren<TextMeshProUGUI>();

        while (index < dialogue.dialogue.Length) {
            tmp.text = dialogue.dialogue.Substring(0, index + 1);
            index++;
            yield return new WaitForSeconds(speed);
        }

        animationCurrentlyPlaying = false;
    }

    private void EndDialogueAnimationEarly() {
        StopCoroutine(ongoingAnimationCoroutine);
        ongoingAnimationCoroutine = null;
        TextMeshProUGUI tmp = dialogueObj.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = fullDialogue;
        animationCurrentlyPlaying = false;
    }

    private void EndAnimationEarly()
    {
        if (ongoingType == typeof(Dialogue))
        {
            EndDialogueAnimationEarly();
        }
    }

    public bool CreateCutscene(Cutscene currentEventChildEvent)
    {
        if (animationCurrentlyPlaying)
        {
            
        }

        return false;
    }
}
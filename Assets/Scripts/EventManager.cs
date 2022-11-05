using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    
    
    private StoryEvent _currentStoryEvent;

    [SerializeField] private Story story;

    public Story Story => story;

    [SerializeField] private GameObject dialoguePrefab;
    [SerializeField] private GameObject choicePrefab;

    public GameObject DialoguePrefab => dialoguePrefab;

    public GameObject ChoicePrefab => choicePrefab;

    [NonSerialized] public Keybinds keybinds;
    
    private void Awake() {
        keybinds = new Keybinds();
        keybinds.Enable();
    }

    private void Start() {
        TestCreation();
    }

    private void Update() {
        // if (!_currentStoryEvent.GoToNextEvent()) return;
        // _currentStoryEvent = _currentStoryEvent.ChooseNextEvent();
    }

    private void TestCreation() {
        // GameObject gameObj = Instantiate(dialoguePrefab, transform);
        // DialogueChunk dialogueChunk = gameObj.GetComponent<DialogueChunk>();
        // currentEvent = dialogueChunk;
        // dialogueChunk.dialogue.Add("Hello, Stranger");
        // dialogueChunk.dialogue.Add("Do you want to continue");
        // dialogueChunk.choices.Add("Yes!!!!");
        // dialogueChunk.choices.Add("No :(");
        // dialogueChunk.choices.Add("Maybe...");
    }
    
}
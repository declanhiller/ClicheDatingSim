﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class StoryManager : MonoBehaviour {
    public Story story;
    public StoryEvent currentEvent;
    [SerializeField] private string managerName;

    private Keybinds keybinds;

    [SerializeField] private EventManager eventManager;
    


    private void Start() {
        LoadSave();
        keybinds = new Keybinds();
        keybinds.Enable();
        keybinds.Player.Click.started += ClickUpdate;
        currentEvent = story.start;
        eventManager.CreateDialogue(currentEvent as Dialogue);
    }

    private bool currentlyPlayingAnimation = false;
    private void ClickUpdate(InputAction.CallbackContext context) {
        if (currentlyPlayingAnimation) {
            //end animation
            return;
        }

        if (currentEvent.childEvents.Count <= 0) {
            return;
        }

        if (currentEvent.childEvents[0].GetType() == typeof(Dialogue)) {
            //return bool of whether event was actually created or not... OR QUEUE EVENT... wait prob not cuz the queue could only be one long lmao
            bool ran = eventManager.CreateDialogue(currentEvent.childEvents[0] as Dialogue);
            if (ran) {
                currentEvent = currentEvent.childEvents[0];
            }
        } else if (currentEvent.childEvents[0].GetType() == typeof(Cutscene)) {
            
        }
        
        //attach a listener when giving the event to the event manager to listen to what option was clicked
        
        
    }
    

    public void Save(List<Vector2> positions) {
        
        string fileSavePath = GetFilePath();


        SavableStory savableStory = InitSavableStory(positions);

        string jsonString = JsonUtility.ToJson(savableStory);

        File.WriteAllText(fileSavePath, jsonString);
    }

    private SavableStory InitSavableStory(List<Vector2> positions)
    {
        SavableStory savableStory = new SavableStory();
        
        List<SavableDialogue> savableDialogueBoxes = new List<SavableDialogue>();
        for (int id = 0; id < story.allEvents.Count; id++)
        {
            StoryEvent storyEvent = story.allEvents[id];
            
            if (storyEvent == story.start) {
                savableStory.startId = id;
            }
            
            if (storyEvent is Dialogue dialogue)
            {
                SavableDialogue savableDialogue = new SavableDialogue(dialogue);
                savableDialogueBoxes.Add(savableDialogue);
                savableDialogue.id = id;
                savableDialogue.children = new int[dialogue.childEvents.Count];
                
                savableDialogue.posX = positions[id].x;
                savableDialogue.posY = positions[id].y;
                
                for (int i = 0; i < dialogue.childEvents.Count; i++)
                {
                    StoryEvent childEvent = dialogue.childEvents[i];
                    int childID = story.allEvents.FindIndex((a) => a == childEvent);
                    savableDialogue.children[i] = childID;
                }
                
            }
        }
        
        savableStory.dialogueBoxes = new SavableDialogue[savableDialogueBoxes.Count];
        savableDialogueBoxes.CopyTo(savableStory.dialogueBoxes);

        return savableStory;
    }

    public List<Vector2> LoadSave() {
        // using StreamReader reader = new StreamReader(fileSavePath);
        // string json = reader.ReadToEnd();

        if (!File.Exists(GetFilePath())) {
            story = new Story();
            FileStream fileStream = File.Create(GetFilePath());
            fileStream.Close();
            return new List<Vector2>();
        }
        
        string json = File.ReadAllText(GetFilePath());

        if (string.IsNullOrEmpty(json)) {
            story = new Story();
            return new List<Vector2>();
        }
        

        SavableStory savableStory = JsonUtility.FromJson<SavableStory>(json);

        List<Vector2> positions = new List<Vector2>();

        
        Dictionary<int, StoryEvent> tracker = new Dictionary<int, StoryEvent>();
        Dictionary<int, SavableEvent> savableTracker = new Dictionary<int, SavableEvent>();

        story = new Story();

        foreach (SavableDialogue savableDialogue in savableStory.dialogueBoxes)
        {
            Dialogue dialogue = new Dialogue();
            dialogue.character = (RomanceCharacters) savableDialogue.character;
            dialogue.dialogue = savableDialogue.dialogue;
            
            story.allEvents.Add(dialogue);
            positions.Add(new Vector2(savableDialogue.posX, savableDialogue.posY));
            
            tracker.Add(savableDialogue.id, dialogue);
            savableTracker.Add(savableDialogue.id, savableDialogue);
        }


        for (int i = 0; i < story.allEvents.Count; i++)
        {
            StoryEvent storyEvent = tracker[i];
            foreach (int id in savableTracker[i].children)
            {
                storyEvent.childEvents.Add(tracker[id]);
            }
        }

        story.start = tracker[savableStory.startId];


        return positions;

    }

    private string GetFilePath() {
        string fileSavePath = Application.persistentDataPath + "/" + managerName + ".json";
        return fileSavePath;
    }
}
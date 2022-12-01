using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class StoryManager : MonoBehaviour {
    public Story story;
    public List<StoryEvent> currentEvent;
    [SerializeField] private string managerName;
    
    [SerializeField] private EventManager eventManager;


    private void Awake() {
        LoadSave();
        currentEvent = new List<StoryEvent>();
        currentEvent.Add(story.start);
    }
    
    public void TickToNextEvent(int id)
    {
        if (id == -1)
        {
            StoryEvent storyEvent = currentEvent[0];
            currentEvent.Clear();
            currentEvent = storyEvent.childEvents;
            return;
        }

        StoryEvent optionEvent = currentEvent[id];
        currentEvent.Clear();
        currentEvent = optionEvent.childEvents;
    }
    
    public Type PeekNextEventType() {
        return currentEvent[0].childEvents[0].GetType();
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
        List<SavableSceneStart> savableSceneStartBoxes = new List<SavableSceneStart>();
        List<SavableOption> savableOptionBoxes = new List<SavableOption>();

        savableStory.startId = -1;
        
        for (int id = 0; id < story.allEvents.Count; id++)
        {
            StoryEvent storyEvent = story.allEvents[id];
            
            if (storyEvent == story.start) {
                savableStory.startId = id;
            }

            SavableEvent savableEvent = null;
            
            if (storyEvent is Dialogue dialogue)
            {
                SavableDialogue savableDialogue = new SavableDialogue(dialogue);
                savableDialogueBoxes.Add(savableDialogue);
                savableEvent = savableDialogue;
            } else if (storyEvent is SceneStart sceneStart) {
                SavableSceneStart savableSceneStart = new SavableSceneStart(sceneStart);
                savableSceneStartBoxes.Add(savableSceneStart);
                savableEvent = savableSceneStart;
            } else if (storyEvent is Option option) {
                SavableOption savableOption = new SavableOption(option);
                savableOptionBoxes.Add(savableOption);
                savableEvent = savableOption;
            }

            if (savableEvent == null) {
                throw new Exception("Story Event was of unsavable type " + storyEvent.GetType());
            }
            
            //global fields for all story events
            savableEvent.id = id;
            savableEvent.children = new int[storyEvent.childEvents.Count];
                
            savableEvent.posX = positions[id].x;
            savableEvent.posY = positions[id].y;
                
            for (int i = 0; i < storyEvent.childEvents.Count; i++)
            {
                StoryEvent childEvent = storyEvent.childEvents[i];
                int childID = story.allEvents.FindIndex((a) => a == childEvent);
                savableEvent.children[i] = childID;
            }
        }
        
        savableStory.dialogueBoxes = new SavableDialogue[savableDialogueBoxes.Count];
        savableDialogueBoxes.CopyTo(savableStory.dialogueBoxes);

        savableStory.sceneStartBoxes = new SavableSceneStart[savableSceneStartBoxes.Count];
        savableSceneStartBoxes.CopyTo(savableStory.sceneStartBoxes);

        savableStory.optionBoxes = new SavableOption[savableOptionBoxes.Count];
        savableOptionBoxes.CopyTo(savableStory.optionBoxes);

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

        TextAsset textAsset = Resources.Load<TextAsset>(managerName);
        string json = textAsset.text;
        // #if UNITY_EDITOR
        // json = File.ReadAllText(GetFilePath());
        // #endif
        if (string.IsNullOrEmpty(json)) {
            story = new Story();
            return new List<Vector2>();
        }
        

        SavableStory savableStory = JsonUtility.FromJson<SavableStory>(json);
        
        List<Vector2> positions = new List<Vector2>();


        Dictionary<int, StoryEvent> tracker = new Dictionary<int, StoryEvent>();
        Dictionary<int, SavableEvent> savableTracker = new Dictionary<int, SavableEvent>();

        story = new Story();
        
        if (savableStory.dialogueBoxes != null) {
            foreach (SavableDialogue savableDialogue in savableStory.dialogueBoxes)
            {
                Dialogue dialogue = new Dialogue();
                dialogue.character = (RomanceCharacters) savableDialogue.character;
                dialogue.dialogue = savableDialogue.dialogue;
                dialogue.expression = (Expression) savableDialogue.expression;
            
                story.allEvents.Add(dialogue);
                positions.Add(new Vector2(savableDialogue.posX, savableDialogue.posY));
            
                tracker.Add(savableDialogue.id, dialogue);
                savableTracker.Add(savableDialogue.id, savableDialogue);
            }
        }

        if (savableStory.sceneStartBoxes != null) {
            foreach (SavableSceneStart savableSceneStart in savableStory.sceneStartBoxes) {
                SceneStart sceneStart = new SceneStart();
                sceneStart.sceneName = savableSceneStart.sceneName;
                sceneStart.background = Resources.Load<Texture2D>(savableSceneStart.backgroundName);
// #if UNITY_EDITOR
//                 sceneStart.background = AssetDatabase.LoadAssetAtPath<Texture2D>(savableSceneStart.backgroundImagePath);
// #endif
                story.allEvents.Add(sceneStart);
                positions.Add(new Vector2(savableSceneStart.posX, savableSceneStart.posY));
                tracker.Add(savableSceneStart.id, sceneStart);
                savableTracker.Add(savableSceneStart.id, savableSceneStart);
            }
        }
        
        if (savableStory.optionBoxes != null) {
            foreach (SavableOption savableOption in savableStory.optionBoxes) {
                Option option = new Option();
                option.option = savableOption.optionStr;
                
                story.allEvents.Add(option);
                positions.Add(new Vector2(savableOption.posX, savableOption.posY));
                tracker.Add(savableOption.id, option);
                savableTracker.Add(savableOption.id, savableOption);
            }
        }



        for (int i = 0; i < story.allEvents.Count; i++)
        {
            StoryEvent storyEvent = tracker[i];
            foreach (int id in savableTracker[i].children)
            {
                if (id == -1) {
                    continue;
                }
                storyEvent.childEvents.Add(tracker[id]);
            }
        }

        if (savableStory.startId != -1) {
            story.start = tracker[savableStory.startId];
        }


        return positions;

    }

    private string GetFilePath() {
        string fileSavePath = Application.persistentDataPath + "/" + managerName + ".json";
        print(fileSavePath);
        return fileSavePath;
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
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

    private void ClickUpdate(InputAction.CallbackContext context) {
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
            
        } else if (currentEvent.childEvents[0].GetType() == typeof(SceneStart))
        {
            throw new NotImplementedException();
            // bool ran = eventManager.CreateDialogue(currentEvent.childEvents[0] as Dialogue);
            // if (ran) {
            //     currentEvent = currentEvent.childEvents[0];
            // }
        } else if (currentEvent.childEvents[0].GetType() == typeof(Option))
        {
            throw new NotImplementedException();
            // bool ran = eventManager.CreateDialogue(currentEvent.childEvents[0] as Dialogue);
            // if (ran) {
            //     currentEvent = currentEvent.childEvents[0];
            // }
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
        List<SavableCutscene> savableCutsceneBoxes = new List<SavableCutscene>();
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
            } else if (storyEvent is Cutscene cutscene) {
                SavableCutscene savableCutscene = new SavableCutscene(cutscene);
                savableCutsceneBoxes.Add(savableCutscene);
                savableEvent = savableCutscene;
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

        savableStory.cutsceneBoxes = new SavableCutscene[savableCutsceneBoxes.Count];
        savableCutsceneBoxes.CopyTo(savableStory.cutsceneBoxes);

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
        
        if (savableStory.dialogueBoxes != null) {
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
        }

        if (savableStory.cutsceneBoxes != null) {
            foreach (SavableCutscene savableCutscene in savableStory.cutsceneBoxes) {
                Cutscene cutscene = new Cutscene();
                cutscene.cutsceneName = savableCutscene.cutsceneName;

                cutscene.image = AssetDatabase.LoadAssetAtPath<Texture2D>(savableCutscene.path);

                story.allEvents.Add(cutscene);
                positions.Add(new Vector2(savableCutscene.posX, savableCutscene.posY));
                tracker.Add(savableCutscene.id, cutscene);
                savableTracker.Add(savableCutscene.id, savableCutscene);
            
            }
        }

        if (savableStory.sceneStartBoxes != null) {
            foreach (SavableSceneStart savableSceneStart in savableStory.sceneStartBoxes) {
                SceneStart sceneStart = new SceneStart();
                sceneStart.sceneName = savableSceneStart.sceneName;
                sceneStart.background = AssetDatabase.LoadAssetAtPath<Texture2D>(savableSceneStart.backgroundImagePath);
                
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
        return fileSavePath;
    }
}
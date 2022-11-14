using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EventManager : MonoBehaviour {
    
    
    private StoryManager manager;
    
    [SerializeField] private GameObject dialoguePrefab;
    [SerializeField] private GameObject optionPrefab;
    
    private GameObject dialogueObj;

    private List<GameObject> optionObjs = new List<GameObject>();

    [NonSerialized] public Keybinds keybinds;

    private Coroutine currentAnimation;

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
        ProcessEvent();
    }

    private void ProcessEvent()
    {
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
            Option[] options = new Option[manager.currentEvent.Count];
            for (int i = 0; i < options.Length; i++)
            {
                options[i] = manager.currentEvent[i] as Option;
            }
            
            StartOption(options);
        }
    }
    
    
    private void StartOption(Option[] options)
    {

        hasBeenClicked = false;
        
        optionObjs.Clear();
        
        float rectHeight = dialogueObj.transform.GetChild(1).GetComponent<RectTransform>().rect.height;
        float difference = rectHeight / (options.Length + 1);
        float temp =  (rectHeight / 2) - difference;
        float[] yPositions = new float[options.Length];
        for (int i = 0; i < options.Length; i++)
        {
            yPositions[i] = temp;
            temp -= difference;
        }

        for (int i = 0; i < options.Length; i++)
        {
            Option option = options[i];
            GameObject optionObj = Instantiate(optionPrefab, dialogueObj.transform.GetChild(1));
            optionObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, yPositions[i]);
            optionObj.GetComponentInChildren<TextMeshProUGUI>().text = option.option;
            var i1 = i;
            optionObj.GetComponentInChildren<Button>().onClick.AddListener(() => ChooseNextEvent(i1));
            optionObjs.Add(optionObj);
        }
        
    }

    void ChooseNextEvent(int choiceId)
    {
        hasBeenClicked = true;
        foreach (GameObject optionObj in optionObjs)
        {
            Destroy(optionObj);
        }
        manager.TickToNextEvent(choiceId);
        this.ProcessEvent();
    }
    
    

    private void StartDialogue(Dialogue dialogue)
    {
        dialogueObj = Instantiate(dialoguePrefab, transform);
        currentAnimation = StartCoroutine(DialogueAnimation(dialogue));
    }

    //if current event is at end then return true;
    private bool UpdateCurrentEvent()
    {
        StoryEvent storyEvent = manager.currentEvent[0];
        if (storyEvent is Dialogue dialogue)
        {
            return UpdateDialogue(dialogue);
        } else if (storyEvent is Cutscene cutscene)
        {
            
        } else if (storyEvent is SceneStart sceneStart)
        {
            
        }else if (storyEvent is Option)
        {
            return UpdateOption();
        }
        
        return true;
    }

    private bool hasBeenClicked = false;
    private bool UpdateOption()
    {
        return hasBeenClicked;
    }

    private bool UpdateDialogue(Dialogue dialogue)
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            currentAnimation = null;
            TextMeshProUGUI tmp = dialogueObj.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = dialogue.dialogue;
            return false;
        }

        return true;
    }


    [SerializeField] private float speed = 0.02f;
    private IEnumerator DialogueAnimation(Dialogue dialogue) {
        int index = 0;
        TextMeshProUGUI tmp = dialogueObj.GetComponentInChildren<TextMeshProUGUI>();

        while (index < dialogue.dialogue.Length) {
            tmp.text = dialogue.dialogue.Substring(0, index + 1);
            index++;
            yield return new WaitForSeconds(speed);
        }

        currentAnimation = null;
    }
}
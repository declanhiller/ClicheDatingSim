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
    [SerializeField] private GameObject cutscenePrefab;
    [SerializeField] private GameObject sceneStartPrefab;
    
    private GameObject dialogueObj;
    private GameObject cutsceneObj;
    private GameObject sceneStartObj;

    private List<GameObject> optionObjs = new List<GameObject>();

    [NonSerialized] public Keybinds keybinds;

    private Coroutine currentAnimation;

    private bool canRun = false;

    private void Start() {
        keybinds = new Keybinds();
        keybinds.Enable();
        keybinds.Player.Click.started += ClickUpdate;
        manager = GameObject.FindGameObjectWithTag("StoryManager").GetComponent<StoryManager>();
        sceneStartObj = Instantiate(sceneStartPrefab, transform);
        StartSceneStart(manager.currentEvent[0] as SceneStart);
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
            Destroy(dialogueObj);
            StartDialogue(dialogue);
        } else if (storyEvent is Cutscene cutscene)
        {
            Destroy(dialogueObj);
            StartCutscene(cutscene);
        } else if (storyEvent is SceneStart sceneStart)
        {
            StartSceneStart(sceneStart);
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

    private void StartSceneStart(SceneStart sceneStart)
    {

        if (sceneStartObj == null) {
            StartCoroutine(SceneStartFadeIn(sceneStart));
            return;
        }
        
        if (sceneStart.sceneName == "$end#") {
            StartCoroutine(SceneStartFadeOut(false, sceneStart));
            return;
        }
        
        StartCoroutine(SceneStartFadeOut(true, sceneStart));
    }

    
    private IEnumerator SceneStartFadeOut(bool shouldContinue, SceneStart sceneStart) {
        Destroy(dialogueObj);

        Image background = sceneStartObj.GetComponent<Image>();
        while (background.color.a > 0) {
            background.color = new Color(background.color.r, background.color.g, background.color.b,
                background.color.a - (fadeSpeed * Time.deltaTime));
            yield return new WaitForEndOfFrame();
        }
        if (shouldContinue) {
            StartCoroutine(SceneStartFadeIn(sceneStart));
        }
    }


    private IEnumerator SceneStartFadeIn(SceneStart sceneStart) {
        Image background = sceneStartObj.GetComponent<Image>();
        background.sprite = Sprite.Create(sceneStart.background, new Rect(0, 0 , sceneStart.background.width, sceneStart.background.height), 
            new Vector2(0.5f, 0.5f));
        Color backgroundColor = background.color;
        background.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0);
        while (background.color.a < 1) {
            background.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, background.color.a + (fadeSpeed * Time.deltaTime));
            yield return new WaitForEndOfFrame();
        }
        manager.TickToNextEvent(-1);
        ProcessEvent();
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
    
    private void StartCutscene(Cutscene cutscene)
    {
        cutsceneObj = Instantiate(cutscenePrefab, transform);
        currentAnimation = StartCoroutine(CutsceneAnimation(cutscene));
    }

    [SerializeField] private float fadeSpeed = 0.1f;
    private IEnumerator CutsceneAnimation(Cutscene cutscene)
    {
        // cutsceneObj.GetComponentInChildren<SpriteRenderer>()
        currentAnimation = null;
        yield break;
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
            TextMeshProUGUI tmp = GetDialogueBox();
            tmp.text = fullDialogue;
            return false;
        }

        // Destroy(dialogueObj);
        return true;
    }


    [SerializeField] private float speed = 0.02f;
    private string fullDialogue;
    private IEnumerator DialogueAnimation(Dialogue dialogue) {
        int index = 0;
        TextMeshProUGUI tmp = GetDialogueBox();
        fullDialogue = StringProcessor.process(dialogue.dialogue);

        GetCharacterNameBox().text = GetCharacterNameString(dialogue.character);
        
        while (index < fullDialogue.Length) {
            tmp.text = fullDialogue.Substring(0, index + 1);
            index++;
            yield return new WaitForSeconds(speed);
        }

        currentAnimation = null;
    }

    private TextMeshProUGUI GetDialogueBox()
    {
        TextMeshProUGUI[] textMeshProUguis = dialogueObj.GetComponentsInChildren<TextMeshProUGUI>();
        foreach(TextMeshProUGUI box in textMeshProUguis)
        {
            if (box.gameObject.CompareTag("Dialogue"))
            {
                return box;
            }
        }

        return null;
    }

    private TextMeshProUGUI GetCharacterNameBox()
    {
        TextMeshProUGUI[] textMeshProUguis = dialogueObj.GetComponentsInChildren<TextMeshProUGUI>();
        foreach(TextMeshProUGUI box in textMeshProUguis)
        {
            if (box.gameObject.CompareTag("Character"))
            {
                return box;
            }
        }

        return null;
    }

    public static string GetCharacterNameString(RomanceCharacters character)
    {
        switch (character)
        {
            case RomanceCharacters.MILF:
                return "Euryale";
            case RomanceCharacters.BAD_BOY:
                return "Valen";
            case RomanceCharacters.PLAYER:
                return PlayerData.playerName;
        }

        return null;
    }
}
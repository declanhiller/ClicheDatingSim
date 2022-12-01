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

    [SerializeField] private CharacterImageController imageController;

    private void Start() {
        keybinds = new Keybinds();
        keybinds.Enable();
        keybinds.Player.Click.started += ClickUpdate;
        manager = GameObject.FindGameObjectWithTag("StoryManager").GetComponent<StoryManager>();
        sceneStartObj = Instantiate(sceneStartPrefab, transform);
        sceneStartObj.transform.SetSiblingIndex(0);
        StartSceneStart(manager.currentEvent[0] as SceneStart);
        imageController.Disable();
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
        imageController.Disable();
        if (sceneStartObj == null) {
            StartCoroutine(SceneStartFadeIn(sceneStart));
            return;
        }
        
        if (sceneStart.sceneName.Contains("$end$")) {

            if (sceneStart.sceneName.Contains("1")) {
                StartCoroutine(SceneStartFadeOut(1));
            } else if (sceneStart.sceneName.Contains("2")) {
                StartCoroutine(SceneStartFadeOut(2));
            }
            
            StartCoroutine(SceneStartFadeOut(false, sceneStart));
            return;
        }
        
        StartCoroutine(SceneStartFadeOut(true, sceneStart));
    }

    private bool isSceneStartTransitioning = false;
    
    private IEnumerator SceneStartFadeOut(int num) {
        isSceneStartTransitioning = true;
        Destroy(dialogueObj);

        Image background = sceneStartObj.GetComponent<Image>();
        while (background.color.a > 0) {
            background.color = new Color(background.color.r, background.color.g, background.color.b,
                background.color.a - (fadeSpeed * Time.deltaTime));
            yield return new WaitForEndOfFrame();
        }

        SceneController sceneController = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
        if (num == 1) {
            sceneController.ChooseCharacter();
        } else if (num == 2) {
            sceneController.BadBoyScene2();
        }
        
    }
    
    private IEnumerator SceneStartFadeOut(bool shouldContinue, SceneStart sceneStart) {
        isSceneStartTransitioning = true;
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

        isSceneStartTransitioning = false;
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
            string text = StringProcessor.process(option.option);
            optionObj.GetComponentInChildren<TextMeshProUGUI>().text = text;
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

    [SerializeField] private float fadeSpeed = 0.1f;

    private void StartDialogue(Dialogue dialogue)
    {
        if (dialogueObj == null) {
            dialogueObj = Instantiate(dialoguePrefab, transform);
            dialogueObj.transform.GetChild(0).GetChild(2).GetComponent<Image>().enabled = false;
            currentAnimation = StartCoroutine(DialogueFadeInAnimation(dialogue));
            return;
        }
        
        dialogueObj.transform.GetChild(0).GetChild(2).GetComponent<Image>().enabled = false;
        currentAnimation = StartCoroutine(DialogueAnimation(dialogue));
    }

    private IEnumerator DialogueFadeInAnimation(Dialogue dialogue) {
        fadeIn = true;
        Image box = dialogueObj.GetComponentInChildren<Image>();
        Color color = box.color;
        box.color = new Color(color.r, color.g, color.b, 0);
        while (box.color.a < 0.8f) {
            box.color = new Color(color.r, color.g, color.b, box.color.a + (fadeSpeed * Time.deltaTime * 2));
            yield return new WaitForEndOfFrame();
        }
        fadeIn = false;
        currentAnimation = StartCoroutine(DialogueAnimation(dialogue));
    }

    //if current event is at end then return true;
    private bool UpdateCurrentEvent()
    {

        StoryEvent storyEvent = manager.currentEvent[0];
        if (storyEvent is Dialogue dialogue)
        {
            return UpdateDialogue(dialogue);
        }else if (storyEvent is SceneStart sceneStart) {
            return !isSceneStartTransitioning;
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

    private bool fadeIn;
    private bool UpdateDialogue(Dialogue dialogue)
    {
        if (fadeIn) {
            return false;
        }
        
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            currentAnimation = null;
            TextMeshProUGUI tmp = GetDialogueBox();
            tmp.text = fullDialogue;        
            dialogueObj.transform.GetChild(0).GetChild(2).GetComponent<Image>().enabled = true;
            if (manager.PeekNextEventType() == typeof(Option)) {
                manager.TickToNextEvent(-1);
                ProcessEvent();
            }
            return false;
        }
        
        
        
        return true;
    }


    [SerializeField] private float speed = 0.02f;
    private string fullDialogue;
    private IEnumerator DialogueAnimation(Dialogue dialogue) {
        imageController.SetCharacter(dialogue.character, dialogue.expression);
        int index = 0;
        TextMeshProUGUI tmp = GetDialogueBox();
        fullDialogue = StringProcessor.process(dialogue.dialogue);

        GetCharacterNameBox().text = GetCharacterNameString(dialogue.character);
        
        while (index < fullDialogue.Length) {
            tmp.text = fullDialogue.Substring(0, index + 1);
            index++;
            yield return new WaitForSeconds(speed);
        }

        dialogueObj.transform.GetChild(0).GetChild(2).GetComponent<Image>().enabled = true;

        currentAnimation = null;
        if (manager.PeekNextEventType() == typeof(Option)) {
            manager.TickToNextEvent(-1);
            ProcessEvent();
        }
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
            case RomanceCharacters.NARRATOR:
                return "Narrator";
            case RomanceCharacters.COWORKER_ONE:
                return "Coworker 1";
            case RomanceCharacters.COWORKER_TWO:
                return "Coworker 2";
            case RomanceCharacters.GOON:
                return "Goon";
        }

        return null;
    }
}
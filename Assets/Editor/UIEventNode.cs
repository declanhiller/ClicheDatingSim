using System.Collections.Generic;
using PlasticPipe.PlasticProtocol.Client.Proxies;
using UnityEngine;

public class UIEventNode {
    
    
    public Rect rect;
    public static GUIStyle BOX_STYLE = new();
    public static GUIStyle TITLE_BOX_STYLE = new();
    public static GUIStyle SELECTED_BOX_STYLE = new();
    public static GUIStyle CONTENT_STYLE = new();
    
    public const int HEIGHT = 100;
    public const int WIDTH = 200;
    public const int TITLE_HEIGHT = 20;
    public const int SELECTED_BORDER_SIZE = 3;
    

    public StoryEvent storyEvent;


    public List<UIEventNode> children = new List<UIEventNode>();
    public UIEventNode parent = null;

    public UIEventNode(float x, float y, StoryEvent storyEvent) {
        CreateRect(x, y);
        this.storyEvent = storyEvent;
    }

    public static void SetSpecialValues() {
        //default style
        BOX_STYLE.normal.background = EditorUtils.MakeTextureForNode(WIDTH, HEIGHT);
        //title style
        TITLE_BOX_STYLE.alignment = TextAnchor.MiddleCenter;
        TITLE_BOX_STYLE.normal.background = EditorUtils.MakeTextureForNodeTitles();
        //selected style
        SELECTED_BOX_STYLE.normal.background = EditorUtils.MakeTextureForSelectedStyle(WIDTH, HEIGHT, SELECTED_BORDER_SIZE);
        
        CONTENT_STYLE.wordWrap = true;
        CONTENT_STYLE.clipping = TextClipping.Clip;

    }

    private void CreateRect(float x, float y) {
        rect = new Rect(x, y, WIDTH, HEIGHT);
    }
    
    protected void DrawTitle(string text)
    {
        Rect title = new Rect(rect.x, rect.y, rect.width, TITLE_HEIGHT);
        GUI.Label(title, text, TITLE_BOX_STYLE);
    }

    public void Draw() {
        GUI.Box(rect, "", BOX_STYLE);
        DrawTitle(storyEvent.eventName);
        DrawContent();
    }
    
    public void DrawSelected() {
        GUI.Box(rect, "", SELECTED_BOX_STYLE);
        DrawTitle(storyEvent.eventName);
        DrawContent();
    }

    private void DrawContent()
    {
        if (storyEvent is Dialogue dialogue)
        {
            DrawDialogueContent(dialogue);
        }else if (storyEvent is Cutscene cutscene)
        {
            DrawCutsceneContent(cutscene);
        }
    }

    private void DrawDialogueContent(Dialogue dialogue)
    {
        Rect internalText = new Rect(rect.x, rect.y + TITLE_HEIGHT, rect.width, HEIGHT-TITLE_HEIGHT);
        GUI.Box(internalText, dialogue.dialogue, CONTENT_STYLE);
    }
    
    private void DrawCutsceneContent(Cutscene cutscene)
    {
        throw new System.NotImplementedException();
    }


}
using System.Collections.Generic;
using PlasticPipe.PlasticProtocol.Client.Proxies;
using UnityEngine;

public class UIEventNode {
    
    
    public Rect rect;
    public static GUIStyle boxStyle = new();
    public static GUIStyle titleBoxStyle = new();
    public static GUIStyle selectedBoxStyle = new();
    public const int HEIGHT = 100;
    public const int WIDTH = 200;
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
        boxStyle.normal.background = EditorUtils.MakeTextureForNode(WIDTH, HEIGHT);
        //title style
        titleBoxStyle.alignment = TextAnchor.MiddleCenter;
        //selected style
        selectedBoxStyle.normal.background = EditorUtils.MakeTextureForSelectedStyle(WIDTH, HEIGHT, SELECTED_BORDER_SIZE);

    }

    private void CreateRect(float x, float y) {
        rect = new Rect(x, y, WIDTH, HEIGHT);
    }
    
    protected void DrawTitle(string text)
    {
        Rect title = new Rect(rect.x, rect.y, rect.width, 20);
        GUI.Label(title, text, titleBoxStyle);
    }

    public void Draw() {
        GUI.Box(rect, "", boxStyle);
        DrawTitle(storyEvent.eventName);
    }
    
    public void DrawSelected() {
        GUI.Box(rect, "", selectedBoxStyle);
        DrawTitle(storyEvent.eventName);
    }

}
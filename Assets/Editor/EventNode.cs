using System.Collections.Generic;
using PlasticPipe.PlasticProtocol.Client.Proxies;
using UnityEngine;

public class EventNode {
    
    
    public Rect rect;
    public static GUIStyle boxStyle = new();
    public static GUIStyle titleBoxStyle = new();
    public const int HEIGHT = 100;
    public const int WIDTH = 200;
    

    public StoryEvent storyEvent;


    public List<EventNode> children = new List<EventNode>();

    public EventNode(float x, float y, StoryEvent storyEvent) {
        CreateRect(x, y);
        this.storyEvent = storyEvent;
    }

    public static void SetSpecialValues() {
        boxStyle.normal.background = EditorUtils.MakeTextureForNode(WIDTH, HEIGHT);

        titleBoxStyle.alignment = TextAnchor.MiddleCenter;
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

}
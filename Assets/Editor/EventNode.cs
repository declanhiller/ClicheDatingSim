using PlasticPipe.PlasticProtocol.Client.Proxies;
using UnityEngine;

public class EventNode {
    
    
    public Rect rect;
    public static GUIStyle defaultStyle;
    public const int HEIGHT = 100;
    public const int WIDTH = 200;

    private StoryEvent storyEvent;

    public EventNode(float x, float y, StoryEvent storyEvent) {
        CreateRect(x, y);
        this.storyEvent = storyEvent;
    }

    public static void SetSpecialValues() {
        GUIStyle guiStyle = new GUIStyle();
        defaultStyle = guiStyle;
        defaultStyle.normal.background = EditorUtils.MakeTextureForNode(WIDTH, HEIGHT);
    }

    private void CreateRect(float x, float y) {
        rect = new Rect(x, y, WIDTH, HEIGHT);
    }

    public void Draw() {
        GUI.Box(rect, storyEvent.eventName, defaultStyle);
    }

}
using UnityEngine;

public class Option : StoryEvent {
    public string option;
    public static GUIStyle OPTION_BOX_STYLE = new();

    public static void SetupOption()
    {
        OPTION_BOX_STYLE.alignment = TextAnchor.MiddleCenter;
        OPTION_BOX_STYLE.normal.background = MakeTextureForNodeTitles(new Color(0.5f, 0.8f, 0.8f));
    }
    
    public override SavableEvent CreateSavableEvent()
    {
        SavableOption savableOption = new SavableOption(this);
        return savableOption;
    }

    public override void RenderNodeContent(Rect rect)
    {
        Rect cutsceneNameBox = new Rect(rect.x, rect.y + TITLE_HEIGHT, rect.width, HEIGHT - TITLE_HEIGHT);
        GUI.Box(cutsceneNameBox, "Choice: " + option, CONTENT_STYLE);
    }

    public override GUIStyle GetStyle()
    {
        return OPTION_BOX_STYLE;
    }
}
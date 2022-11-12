using UnityEngine;
using UnityEngine.UI;

public class Cutscene : StoryEvent {
    public string cutsceneName;
    public Texture2D image;
    public static GUIStyle CUTSCENE_BOX_STYLE = new();

    public static void SetupCutscene()
    {
        CUTSCENE_BOX_STYLE.alignment = TextAnchor.MiddleCenter;
        CUTSCENE_BOX_STYLE.normal.background = MakeTextureForNodeTitles(new Color(0.8f, 0.5f, 0.5f));
    }
    
    
    public override SavableEvent CreateSavableEvent()
    {
        SavableCutscene savableCutscene = new SavableCutscene(this);
        return savableCutscene;
    }

    public override void RenderNodeContent(Rect rect)
    {
        Rect cutsceneNameBox = new Rect(rect.x, rect.y + TITLE_HEIGHT, rect.width, HEIGHT - TITLE_HEIGHT);
        GUI.Box(cutsceneNameBox, "Cutscene Name: " + cutsceneName, CONTENT_STYLE);

        if (image != null) {
            Rect imageNameTextBox = new Rect(rect.x, rect.y + TITLE_HEIGHT + CHARACTER_FIELD_HEIGHT, rect.width,
                HEIGHT - TITLE_HEIGHT - CHARACTER_FIELD_HEIGHT);
            GUI.Box(imageNameTextBox, image.name, CONTENT_STYLE);
        }
    }

    public override GUIStyle GetStyle()
    {
        return CUTSCENE_BOX_STYLE;
    }
}
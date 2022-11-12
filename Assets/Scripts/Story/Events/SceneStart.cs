using UnityEngine;

public class SceneStart : StoryEvent {
    public string sceneName;
    public Texture2D background;
    public static GUIStyle SCENE_START_BOX_STYLE = new();

    public static void SetupSceneStart()
    {
        SCENE_START_BOX_STYLE.alignment = TextAnchor.MiddleCenter;
        SCENE_START_BOX_STYLE.normal.background = MakeTextureForNodeTitles(new Color(0.5f, 0.8f, 0.5f));
    }
    
    public override SavableEvent CreateSavableEvent()
    {
        SavableSceneStart savableSceneStart = new SavableSceneStart(this);
        return savableSceneStart;
    }

    public override void RenderNodeContent(Rect rect)
    {
        Rect cutsceneNameBox = new Rect(rect.x, rect.y + TITLE_HEIGHT, rect.width, HEIGHT - TITLE_HEIGHT);
        GUI.Box(cutsceneNameBox, "Scene Name: " + sceneName, CONTENT_STYLE);

        if (background != null) {
            Rect imageNameTextBox = new Rect(rect.x, rect.y + TITLE_HEIGHT + CHARACTER_FIELD_HEIGHT, rect.width,
                HEIGHT - TITLE_HEIGHT - CHARACTER_FIELD_HEIGHT);
            GUI.Box(imageNameTextBox, background.name, CONTENT_STYLE);
        }
    }

    public override GUIStyle GetStyle()
    {
        return SCENE_START_BOX_STYLE;
    }
}
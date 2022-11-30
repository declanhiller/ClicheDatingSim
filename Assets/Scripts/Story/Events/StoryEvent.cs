using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public abstract class StoryEvent{
    public List<StoryEvent> childEvents = new List<StoryEvent>();
    
    public const int HEIGHT = 100;
    public const int WIDTH = 200;
    public const int TITLE_HEIGHT = 20;
    public const int CHARACTER_FIELD_HEIGHT = 20;

    public static GUIStyle CONTENT_STYLE = new();

    public static void Setup()
    {
        CONTENT_STYLE.wordWrap = true;
        CONTENT_STYLE.clipping = TextClipping.Clip;
        Dialogue.SetupDialogue();
        Option.SetupOption();
        SceneStart.SetupSceneStart();
    }
    
    public abstract SavableEvent CreateSavableEvent();

    public abstract void RenderNodeContent(Rect rect);

    public abstract GUIStyle GetStyle();

    public static Texture2D MakeTextureForNodeTitles(Color color)
    {
        Texture2D t2d = new Texture2D(1, 1);
        t2d.SetPixel(0, 0, color);
        t2d.Apply();
        return t2d;
    }

}

using UnityEditor;
using UnityEngine;

public class StoryInspector
{

    private const int inspector = 40;
    private Rect rect;

    private static GUIStyle INSPECTOR_STYLE;
    

    public void Render(StoryEvent storyEvent, EditorWindow window)
    {
        rect = new Rect(window.position.width - inspector, 0, inspector, window.position.height);

    }

    public static void SetStyles()
    {
        if (INSPECTOR_STYLE == null)
        {
            INSPECTOR_STYLE = new GUIStyle();
        }
        
        INSPECTOR_STYLE.normal.background = Texture2D.grayTexture;
        
    }
}
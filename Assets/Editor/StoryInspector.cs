using UnityEditor;
using UnityEngine;

public class StoryInspector
{

    private const int inspector = 300;
    private Rect rect;

    private static GUIStyle INSPECTOR_STYLE;
    private static GUIStyle INSPECTOR_TITLE_STYLE;
    

    public void Render(StoryEvent storyEvent, EditorWindow window)
    {

        if (storyEvent == null)
        {
            return;
        }
        
        if (INSPECTOR_STYLE == null)
        {
            SetStyles();
        }

        rect = new Rect(window.position.width - inspector, 0, inspector, window.position.height);
        GUILayout.BeginArea(rect, INSPECTOR_STYLE);
        GUILayout.BeginVertical();

        GUILayout.Label(storyEvent.eventName, INSPECTOR_TITLE_STYLE);
        
        
        GUILayout.EndVertical();
        GUILayout.EndArea();

    }

    public static void SetStyles()
    {
        if (INSPECTOR_STYLE == null)
        {
            INSPECTOR_STYLE = new GUIStyle();
        }
        
        INSPECTOR_STYLE.normal.background = EditorUtils.MakeTextureForInspector();
        
        INSPECTOR_TITLE_STYLE = new GUIStyle();
        INSPECTOR_STYLE.alignment = TextAnchor.MiddleCenter;
        INSPECTOR_TITLE_STYLE.normal.textColor = Color.white;
        INSPECTOR_TITLE_STYLE.wordWrap = true;
    }
}
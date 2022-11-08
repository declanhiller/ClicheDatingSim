using UnityEditor;
using UnityEngine;

public class StoryInspector
{

    private const int inspectorWidth = 300;
    private Rect rect;

    private static GUIStyle INSPECTOR_STYLE;
    private static GUIStyle INSPECTOR_TITLE_STYLE;
    private static GUIStyle FIELD_STYLE;

    private static int VERTICAL_SPACING = 20;
    

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

        rect = new Rect(window.position.width - inspectorWidth, 0, inspectorWidth, window.position.height);
        GUILayout.BeginArea(rect, INSPECTOR_STYLE);
        GUILayout.BeginVertical();
        
        GUILayout.Label(storyEvent.eventName, INSPECTOR_TITLE_STYLE);

        GUILayout.Space(VERTICAL_SPACING);

        if (storyEvent is Dialogue dialogue)
        {
            RenderDialogueInspector(dialogue);
        }else if (storyEvent is Cutscene cutscene)
        {
            RenderCutsceneInspector(cutscene);
        }
        
        GUILayout.EndVertical();
        GUILayout.EndArea();

    }

    private void RenderCutsceneInspector(Cutscene cutscene)
    {
        
    }

    private void RenderDialogueInspector(Dialogue dialogue)
    {

        float labelWidth = inspectorWidth * 0.4f;
        float fieldWidth = inspectorWidth * 0.6f;
        
        
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Character:", GUILayout.MinWidth(labelWidth), GUILayout.MaxWidth(labelWidth));
        dialogue.character = (RomanceCharacters) EditorGUILayout.EnumPopup(dialogue.character, GUILayout.MaxWidth(fieldWidth));
        GUILayout.EndHorizontal();
        
        GUILayout.Space(VERTICAL_SPACING);

        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Dialogue:", GUILayout.MinWidth(labelWidth), GUILayout.MaxWidth(labelWidth));
        dialogue.dialogue = EditorGUILayout.TextField(dialogue.dialogue, GUILayout.MaxWidth(fieldWidth));
        GUILayout.EndHorizontal();
        
    }

    public static void SetStyles()
    {
        if (INSPECTOR_STYLE == null)
        {
            INSPECTOR_STYLE = new GUIStyle();
        }
        
        INSPECTOR_STYLE.normal.background = EditorUtils.MakeTextureForInspector();
        
        INSPECTOR_TITLE_STYLE = new GUIStyle();
        INSPECTOR_TITLE_STYLE.alignment = TextAnchor.MiddleCenter;
        INSPECTOR_TITLE_STYLE.normal.textColor = Color.white;
        INSPECTOR_TITLE_STYLE.fontSize = 24;
        INSPECTOR_TITLE_STYLE.wordWrap = true;


        FIELD_STYLE = new GUIStyle();
        FIELD_STYLE.normal.textColor = Color.white;
        FIELD_STYLE.fontSize = 16;
    }
    
}
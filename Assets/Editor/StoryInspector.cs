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

        rect = new Rect(window.position.width - inspectorWidth, 20, inspectorWidth, window.position.height);
        GUILayout.BeginArea(rect, INSPECTOR_STYLE);
        GUILayout.BeginVertical();
        
        GUILayout.Label(storyEvent.GetType().ToString(), INSPECTOR_TITLE_STYLE);

        GUILayout.Space(VERTICAL_SPACING);

        if (storyEvent is Dialogue dialogue)
        {
            RenderDialogueInspector(dialogue);
            window.Repaint();
        }else if (storyEvent is Cutscene cutscene)
        {
            RenderCutsceneInspector(cutscene);
            window.Repaint();
        } else if (storyEvent is SceneStart sceneStart) {
            RenderSceneStartInspector(sceneStart);
            window.Repaint();
        }
        
        GUILayout.EndVertical();
        GUILayout.EndArea();

    }

    private void RenderSceneStartInspector(SceneStart sceneStart) {
        float labelWidth = inspectorWidth * 0.4f;
        float fieldWidth = inspectorWidth * 0.6f;
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Cutscene Name: ");
        sceneStart.sceneName = GUILayout.TextArea(sceneStart.sceneName);
        GUILayout.EndHorizontal();
        
        GUILayout.Space(VERTICAL_SPACING);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Background:", GUILayout.MinWidth(labelWidth), GUILayout.MaxWidth(labelWidth));
        sceneStart.background = (Texture2D) EditorGUILayout.ObjectField(sceneStart.background, typeof(Texture2D), false, GUILayout.MaxWidth(fieldWidth));
        EditorGUILayout.EndHorizontal();
    }

    private void RenderCutsceneInspector(Cutscene cutscene)
    {
        float labelWidth = inspectorWidth * 0.4f;
        float fieldWidth = inspectorWidth * 0.6f;
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Cutscene Name: ");
        cutscene.cutsceneName = GUILayout.TextArea(cutscene.cutsceneName);
        GUILayout.EndHorizontal();
        
        GUILayout.Space(VERTICAL_SPACING);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Background:", GUILayout.MinWidth(labelWidth), GUILayout.MaxWidth(labelWidth));
        cutscene.image = (Texture2D) EditorGUILayout.ObjectField(cutscene.image, typeof(Texture2D), false, GUILayout.MaxWidth(fieldWidth));
        EditorGUILayout.EndHorizontal();
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

        GUILayout.Label("Dialogue: ");
        // EditorGUILayout.LabelField("Dialogue:", GUILayout.MinWidth(labelWidth), GUILayout.MaxWidth(labelWidth));
        // dialogue.dialogue = EditorGUILayout.TextField(dialogue.dialogue, GUILayout.MaxWidth(fieldWidth));
        dialogue.dialogue = GUILayout.TextArea(dialogue.dialogue);
        GUILayout.EndHorizontal();
        
    }

    public static void SetStyles()
    {
        INSPECTOR_STYLE = new GUIStyle();
        
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
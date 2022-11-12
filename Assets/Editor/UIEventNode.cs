using System.Collections.Generic;
using UnityEngine;



public class UIEventNode
    {


        public Rect rect;
        public static GUIStyle BOX_STYLE = new();
        public static GUIStyle DIALOGUE_BOX_STYLE = new();
        public static GUIStyle CUTSCENE_BOX_STYLE = new();
        public static GUIStyle SCENE_START_BOX_STYLE = new();
        public static GUIStyle OPTION_BOX_STYLE = new();
        public static GUIStyle SELECTED_BOX_STYLE = new();
        public static GUIStyle CONTENT_STYLE = new();

        public const int HEIGHT = 100;
        public const int WIDTH = 200;
        public const int TITLE_HEIGHT = 20;
        public const int SELECTED_BORDER_SIZE = 3;


        public StoryEvent storyEvent;


        public List<UIEventNode> children = new List<UIEventNode>();
        public UIEventNode parent = null;

        public UIEventNode(float x, float y, StoryEvent storyEvent)
        {
            CreateRect(x, y);
            this.storyEvent = storyEvent;
        }

        public static void SetSpecialValues()
        {
            //default style
            BOX_STYLE.normal.background = EditorUtils.MakeTextureForNode(WIDTH, HEIGHT);
            //title style

            DIALOGUE_BOX_STYLE.alignment = TextAnchor.MiddleCenter;
            CUTSCENE_BOX_STYLE.alignment = TextAnchor.MiddleCenter;
            SCENE_START_BOX_STYLE.alignment = TextAnchor.MiddleCenter;
            OPTION_BOX_STYLE.alignment = TextAnchor.MiddleCenter;

            DIALOGUE_BOX_STYLE.normal.background = EditorUtils.MakeTextureForNodeTitles(new Color(0.5f, 0.5f, 0.8f));
            CUTSCENE_BOX_STYLE.normal.background = EditorUtils.MakeTextureForNodeTitles(new Color(0.8f, 0.5f, 0.5f));
            SCENE_START_BOX_STYLE.normal.background = EditorUtils.MakeTextureForNodeTitles(new Color(0.5f, 0.8f, 0.5f));
            OPTION_BOX_STYLE.normal.background = EditorUtils.MakeTextureForNodeTitles(new Color(0.5f, 0.8f, 0.8f));
            
            
            //selected style
            SELECTED_BOX_STYLE.normal.background =
                EditorUtils.MakeTextureForSelectedStyle(WIDTH, HEIGHT, SELECTED_BORDER_SIZE);

            CONTENT_STYLE.wordWrap = true;
            CONTENT_STYLE.clipping = TextClipping.Clip;

        }

        private void CreateRect(float x, float y)
        {
            rect = new Rect(x, y, WIDTH, HEIGHT);
        }

        protected void DrawTitle() {
            string text = storyEvent.GetType().ToString();
            Rect title = new Rect(rect.x, rect.y, rect.width, TITLE_HEIGHT);
            GUIStyle style = null;
            if (storyEvent is Dialogue) {
                style = DIALOGUE_BOX_STYLE;
            } else if (storyEvent is Cutscene) {
                style = CUTSCENE_BOX_STYLE;
            } else if (storyEvent is SceneStart) {
                style = SCENE_START_BOX_STYLE;
            } else if (storyEvent is Option) {
                style = OPTION_BOX_STYLE;
            }
            
            
            GUI.Label(title, text, style);
        }

        public void Draw()
        {
            GUI.Box(rect, "", BOX_STYLE);
            DrawTitle();
            DrawContent();
        }

        public void DrawSelected()
        {
            GUI.Box(rect, "", SELECTED_BOX_STYLE);
            DrawTitle();
            DrawContent();
        }

        private void DrawContent()
        {
            if (storyEvent is Dialogue dialogue)
            {
                DrawDialogueContent(dialogue);
            }
            else if (storyEvent is Cutscene cutscene)
            {
                DrawCutsceneContent(cutscene);
            } else if (storyEvent is SceneStart sceneStart) {
                DrawSceneStartContent(sceneStart);
            } else if (storyEvent is Option option) {
                DrawOptionContent(option);
            }
        }

        private const int CHARACTER_FIELD_HEIGHT = 20;

        private void DrawDialogueContent(Dialogue dialogue)
        {
            Rect characterText = new Rect(rect.x, rect.y + TITLE_HEIGHT, rect.width, HEIGHT - TITLE_HEIGHT);
            GUI.Box(characterText, "Character: " + dialogue.character, CONTENT_STYLE);

            Rect internalText = new Rect(rect.x, rect.y + TITLE_HEIGHT + CHARACTER_FIELD_HEIGHT, rect.width,
                HEIGHT - TITLE_HEIGHT - CHARACTER_FIELD_HEIGHT);
            GUI.Box(internalText, dialogue.dialogue, CONTENT_STYLE);
        }

        private void DrawCutsceneContent(Cutscene cutscene)
        {
            Rect cutsceneNameBox = new Rect(rect.x, rect.y + TITLE_HEIGHT, rect.width, HEIGHT - TITLE_HEIGHT);
            GUI.Box(cutsceneNameBox, "Cutscene Name: " + cutscene.cutsceneName, CONTENT_STYLE);

            if (cutscene.image != null) {
                Rect imageNameTextBox = new Rect(rect.x, rect.y + TITLE_HEIGHT + CHARACTER_FIELD_HEIGHT, rect.width,
                    HEIGHT - TITLE_HEIGHT - CHARACTER_FIELD_HEIGHT);
                GUI.Box(imageNameTextBox, cutscene.image.name, CONTENT_STYLE);
            }

        }
        
        private void DrawSceneStartContent(SceneStart sceneStart) {
            Rect cutsceneNameBox = new Rect(rect.x, rect.y + TITLE_HEIGHT, rect.width, HEIGHT - TITLE_HEIGHT);
            GUI.Box(cutsceneNameBox, "Scene Name: " + sceneStart.sceneName, CONTENT_STYLE);

            if (sceneStart.background != null) {
                Rect imageNameTextBox = new Rect(rect.x, rect.y + TITLE_HEIGHT + CHARACTER_FIELD_HEIGHT, rect.width,
                    HEIGHT - TITLE_HEIGHT - CHARACTER_FIELD_HEIGHT);
                GUI.Box(imageNameTextBox, sceneStart.background.name, CONTENT_STYLE);
            }
        }
        
        private void DrawOptionContent(Option option) {
            Rect cutsceneNameBox = new Rect(rect.x, rect.y + TITLE_HEIGHT, rect.width, HEIGHT - TITLE_HEIGHT);
            GUI.Box(cutsceneNameBox, "Choice: " + option.option, CONTENT_STYLE);
        }
        
    }
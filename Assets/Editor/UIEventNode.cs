using System.Collections.Generic;
using UnityEngine;



public class UIEventNode
    {


        public Rect rect;
        public static GUIStyle BOX_STYLE = new();
        public static GUIStyle SELECTED_BOX_STYLE = new();

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


            
            
            //selected style
            SELECTED_BOX_STYLE.normal.background =
                EditorUtils.MakeTextureForSelectedStyle(WIDTH, HEIGHT, SELECTED_BORDER_SIZE);

        }

        private void CreateRect(float x, float y)
        {
            rect = new Rect(x, y, WIDTH, HEIGHT);
        }

        protected void DrawTitle() {
            string text = storyEvent.GetType().ToString();
            Rect title = new Rect(rect.x, rect.y, rect.width, TITLE_HEIGHT);
            GUIStyle style = storyEvent.GetStyle();

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
            storyEvent.RenderNodeContent(rect);
        }
    }
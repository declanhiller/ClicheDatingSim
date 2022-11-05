using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StoryEditor : EditorWindow {

    [SerializeField] private Story story;

    private List<EventNode> allEventNodes = new List<EventNode>();
    
    private Vector2 mousePos;
    
    public const int WIDTH = 200;
    public const int HEIGHT = 200;
    
    
    [MenuItem("Window/Story Creator")]   
    static void ShowEditor() {     
        StoryEditor editor = EditorWindow.GetWindow<StoryEditor>();      
        editor.Init();     
    }

    private void OnEnable() {
        EventNodeFactory.SetupFactory();
        DetectStory();
    }

    private void OnSelectionChange() {
        
        DetectStory();
    }

    public void Init() {
        EventNodeFactory.SetupFactory();
    }
 
    void OnGUI() {

        RenderStory();

        mousePos = Event.current.mousePosition;
        
        DisplayContextMenu();
    }

    private void RenderStory() {
        for (int i = 0; i < allEventNodes.Count; i++) {
            EventNode storyEvent = allEventNodes[i];
            storyEvent.Draw();
        }
    }

    private void ClearStory() {
        story.start = null;
        story.allEvents.Clear();
    }

    void CheckSizeOfStory() {
        Debug.Log("There is " + story.allEvents.Count + " events in the story");
    }

    void CheckNameOfStory() {
        Debug.Log(story.name);
    }
    
    
    void CreateDialogueChunk()
    {
        if (story.start == null) {
            Dialogue dialogue = new Dialogue();
            dialogue.eventName = "Start";
            story.start = dialogue;
            story.allEvents.Add(dialogue);
            EventNode eventNode = EventNodeFactory.createNode(mousePos.x, mousePos.y, dialogue);
            allEventNodes.Add(eventNode);
        } else {
            Dialogue dialogue = new Dialogue();
            dialogue.eventName = "Dialogue";
            story.allEvents.Add(dialogue);
        }
        AssetDatabase.SaveAssets();
    }
    
    
    private void DetectStory()
    {
        if (Selection.activeObject is Story && EditorUtility.IsPersistent(Selection.activeObject)) {
            story = Selection.activeObject as Story;
            allEventNodes.Clear();
            foreach (StoryEvent storyEvent in story.allEvents) {
                EventNode eventNode = EventNodeFactory.createNode(200, 200, storyEvent);
                allEventNodes.Add(eventNode);
            }
        }
    }

    void DrawNodeCurve(Rect start, Rect end) {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);      
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);    
        Vector3 startTan = startPos + Vector3.right * 50;      
        Vector3 endTan = endPos + Vector3.left * 50;       
        Color shadowCol = new Color(0, 0, 0, 0.06f);       
        for (int i = 0; i < 3; i++) // Draw a shadow           
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }

    private void DisplayContextMenu() {
        Event current = Event.current;
        
        if(mouseOverWindow &&  current.type == EventType.ContextClick) {
            List<EventNode> hoveringOverEventNode = new List<EventNode>();
            foreach (EventNode eventNode in allEventNodes) {
                if (eventNode.rect.Contains(mousePos)) {
                    hoveringOverEventNode.Add(eventNode);
                }
            }


            if (hoveringOverEventNode.Count == 0) {
                DisplayRegularContextMenu(current);
            } else {
                DisplaySpecialContextMenu(current, hoveringOverEventNode[0]);
            }

        }
    }

    
    
    private void DisplaySpecialContextMenu(Event current, EventNode eventNode) {
        GenericMenu menu = new GenericMenu();
            
        menu.AddItem(new GUIContent("Delete"), false, Delete);
        
        menu.ShowAsContext();
 
        current.Use(); 
    }

    public void Delete() {
        
    }

    private void DisplayRegularContextMenu(Event current) {
        GenericMenu menu = new GenericMenu();
            
        menu.AddItem(new GUIContent("Create/Create Dialogue"), false, CreateDialogueChunk);
        menu.AddItem(new GUIContent("Create/Create Choice"), false, CreateDialogueChunk);
        menu.AddItem(new GUIContent("Create/Create Minigame"), false, CreateDialogueChunk);
        menu.AddItem(new GUIContent("Check Size"), false, CheckSizeOfStory);
        menu.AddItem(new GUIContent("Check Name"), false, CheckNameOfStory);
        menu.AddItem(new GUIContent("Clear Story"), false, ClearStory);


        menu.ShowAsContext();
 
        current.Use(); 
    }

}

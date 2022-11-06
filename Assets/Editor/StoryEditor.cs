using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StoryEditor : EditorWindow {

    [SerializeField] private Story story;

    private List<EventNode> allEventNodes = new List<EventNode>();
    
    private Vector2 mousePos;

    private EventNode hoveringOverNode;
    

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
 
    void OnGUI()
    {

        if (story != null)
        {
            RenderConnections();
            RenderCurrentConnectionLine();
            RenderStory();

            List<EventNode> mouseOverNodes = GetMouseOverNodes();

            hoveringOverNode = mouseOverNodes.Count > 0 ? mouseOverNodes[0] : null;
            
            mousePos = Event.current.mousePosition;

            ProcessEventTypes();
        }

    }

    private void RenderCurrentConnectionLine()
    {
        if (connectionNode != null)
        {
            RenderNodeCurve(connectionNode.rect.center, mousePos);
            Repaint();
        }
    }

    private void RenderConnections()
    {
        EventNode[] copiedArray = new EventNode[story.allEvents.Count];
        allEventNodes.CopyTo(copiedArray);
        List<EventNode> copiedList = new List<EventNode>(copiedArray);

        while (copiedList.Count != 0)
        {
            RenderConnectionsRecursively(copiedList, copiedList[0]);
        }
    }

    private void RenderConnectionsRecursively(List<EventNode> copiedList, EventNode parentNode)
    {
        copiedList.Remove(parentNode);
        List<EventNode> childEventNodes = parentNode.children;
        foreach (EventNode node in childEventNodes)
        {
            RenderNodeCurve(parentNode.rect, node.rect);
            RenderConnectionsRecursively(copiedList, node);
        }
    }

    private void ProcessEventTypes()
    {
        Event current = Event.current;
        switch (current.type)
        {
            case EventType.ContextClick:
                DisplayContextMenu(current);
                break;
            case EventType.MouseDrag:
                DragNode(current);
                break;
            case EventType.MouseDown:
                ConnectNode(current);
                break;
            
        }
    }

    private void ConnectNode(Event current)
    {
        if (connectionNode != null)
        {
            if (hoveringOverNode != null)
            {
                connectionNode.storyEvent.childEvents.Add(hoveringOverNode.storyEvent);
                connectionNode.children.Add(hoveringOverNode);
            }
        }

        connectionNode = null;
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
            EventNode eventNode = EventNodeFactory.createNode(mousePos.x, mousePos.y, dialogue);
            allEventNodes.Add(eventNode);
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

    void RenderNodeCurve(Rect start, Rect end) {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);      
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        RenderNodeCurve(startPos, endPos);
    }

    void RenderNodeCurve(Vector3 startPos, Vector3 endPos)
    {
        Vector3 startTan = startPos + Vector3.right * 50;      
        Vector3 endTan = endPos + Vector3.left * 50;       
        Color shadowCol = new Color(0, 0, 0, 0.06f);       
        for (int i = 0; i < 3; i++) // Draw a shadow           
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }

    private void DisplayContextMenu(Event current) {
        
        if(mouseOverWindow)
        {
            if (hoveringOverNode == null) {
                DisplayRegularContextMenu(current);
            } else {
                DisplaySpecialContextMenu(current, hoveringOverNode);
            }

        }
    }

    private List<EventNode> GetMouseOverNodes()
    {
        List<EventNode> hoveringOverEventNode = new List<EventNode>();
        foreach (EventNode eventNode in allEventNodes) {
            if (eventNode.rect.Contains(mousePos)) {
                hoveringOverEventNode.Add(eventNode);
            }
        }

        return hoveringOverEventNode;
    }


    private EventNode rightClickedNode;
    private void DisplaySpecialContextMenu(Event current, EventNode eventNode) {
        GenericMenu menu = new GenericMenu();

        rightClickedNode = eventNode;
            
        menu.AddItem(new GUIContent("Delete"), false, Delete);
        menu.AddItem(new GUIContent("CreateTransition"), false, CreateConnection);
        
        menu.ShowAsContext();
 
        current.Use(); 
    }

    private EventNode connectionNode;
    private void CreateConnection()
    {
        connectionNode = rightClickedNode;
    }

    public void Delete() {
        if (rightClickedNode != null)
        {
            if (rightClickedNode.storyEvent == story.start)
            {
                story.start = null;
            }
            story.allEvents.Remove(rightClickedNode.storyEvent);
            allEventNodes.Remove(rightClickedNode);
            rightClickedNode = null;
        }
    }

    private void DragNode(Event current)
    {
        if (hoveringOverNode != null)
        {
            hoveringOverNode.rect.position += current.delta;
            GUI.changed = true;
        }
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

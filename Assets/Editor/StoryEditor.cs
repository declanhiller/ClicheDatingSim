using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class StoryEditor : EditorWindow {

    [SerializeField] private Story story;

    private List<UIEventNode> allEventNodes = new List<UIEventNode>();
    
    private Vector2 mousePos;

    private UIEventNode hoveringOverNode;
    private UIEventNode selectedNode;

    private StoryInspector storyInspector;

    [MenuItem("Window/Story Creator")]   
    static StoryEditor ShowEditor() {     
        StoryEditor editor = EditorWindow.GetWindow<StoryEditor>();      
        editor.Init();
        return editor;
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
            RenderInspector();

            List<UIEventNode> mouseOverNodes = GetMouseOverNodes();

            hoveringOverNode = mouseOverNodes.Count > 0 ? mouseOverNodes[0] : null;
            
            mousePos = Event.current.mousePosition;

            ProcessEventTypes();
        }

    }

    private void RefreshEventNodes()
    {
        allEventNodes.Clear();
        foreach (StoryEvent storyEvent in story.allEvents)
        {
            UIEventNode uiEventNode = EventNodeFactory.createNode(200, 200, storyEvent);
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
        UIEventNode[] copiedArray = new UIEventNode[story.allEvents.Count];
        allEventNodes.CopyTo(copiedArray);
        List<UIEventNode> copiedList = new List<UIEventNode>(copiedArray);

        while (copiedList.Count != 0)
        {
            RenderConnectionsRecursively(copiedList, copiedList[0]);
        }
    }

    private void RenderConnectionsRecursively(List<UIEventNode> copiedList, UIEventNode parentNode)
    {
        copiedList.Remove(parentNode);
        List<UIEventNode> childEventNodes = parentNode.children;
        foreach (UIEventNode node in childEventNodes)
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
                Drag(current);
                break;
            case EventType.MouseDown:
                ClickNode(current);
                break;
            
        }
    }

    private void Drag(Event current)
    {
        if (hoveringOverNode != null)
        {
            DragNode(current);
        }
        else
        {
            DragOnWindow(current.delta);
        }
    }

    private void ClickNode(Event current)
    {
        if (connectionNode != null)
        {
            if (hoveringOverNode != null && connectionNode != hoveringOverNode)
            {
                connectionNode.storyEvent.childEvents.Add(hoveringOverNode.storyEvent);
                connectionNode.children.Add(hoveringOverNode);
                hoveringOverNode.parent = connectionNode;
            }
        }

        connectionNode = null;
        
        selectedNode = hoveringOverNode;
        
        Repaint();
    }

    private void RenderStory() {
        for (int i = 0; i < allEventNodes.Count; i++) {
            UIEventNode storyUIEvent = allEventNodes[i];
            if (storyUIEvent == selectedNode)
            {
                storyUIEvent.DrawSelected();
            }
            else
            {
                storyUIEvent.Draw();
            }
        }
    }

    void CheckSizeOfStory() {
        Debug.Log("There is " + story.allEvents.Count + " events in the story");
    }


    void CreateDialogueChunk()
    {
        if (story.start == null) {
            Dialogue dialogue = new Dialogue();
            dialogue.eventName = "Start";
            story.start = dialogue;
            story.allEvents.Add(dialogue);
            UIEventNode uiEventNode = EventNodeFactory.createNode(mousePos.x, mousePos.y, dialogue);
            allEventNodes.Add(uiEventNode);
            // AssetDatabase.AddObjectToAsset(dialogue, story);
        } else {
            Dialogue dialogue = new Dialogue();
            dialogue.eventName = "Dialogue";
            story.allEvents.Add(dialogue);
            UIEventNode uiEventNode = EventNodeFactory.createNode(mousePos.x, mousePos.y, dialogue);
            allEventNodes.Add(uiEventNode);
        }

    }
    
    
    private void DetectStory()
    {
        // if (Selection.activeObject is Story && EditorUtility.IsPersistent(Selection.activeObject)) {
        //     story = Selection.activeObject as Story;
        //     allEventNodes.Clear();
        //     foreach (StoryEvent storyEvent in story.allEvents) {
        //         UIEventNode uiEventNode = EventNodeFactory.createNode(200, 200, storyEvent);
        //         allEventNodes.Add(uiEventNode);
        //     }
        // }
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

    private List<UIEventNode> GetMouseOverNodes()
    {
        List<UIEventNode> hoveringOverEventNode = new List<UIEventNode>();
        foreach (UIEventNode eventNode in allEventNodes) {
            if (eventNode.rect.Contains(mousePos)) {
                hoveringOverEventNode.Add(eventNode);
            }
        }

        return hoveringOverEventNode;
    }


    private UIEventNode rightClickedNode;
    private void DisplaySpecialContextMenu(Event current, UIEventNode uiEventNode) {
        GenericMenu menu = new GenericMenu();

        rightClickedNode = uiEventNode;
            
        menu.AddItem(new GUIContent("Delete"), false, Delete);
        menu.AddItem(new GUIContent("CreateTransition"), false, CreateConnection);
        
        menu.ShowAsContext();
 
        current.Use(); 
    }

    private UIEventNode connectionNode;
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


            if (rightClickedNode.parent != null)
            {
                rightClickedNode.parent.children.Remove(rightClickedNode);
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
    
    private void DragSpecificNode(UIEventNode node, Vector2 delta)
    {
        node.rect.position += delta;
    }
    
    private void DragOnWindow(Vector2 delta)
    {
        foreach (UIEventNode eventNode in allEventNodes)
        {
            DragSpecificNode(eventNode, delta); 
        }

        GUI.changed = true;
    }

    
    
    private void DisplayRegularContextMenu(Event current) {
        GenericMenu menu = new GenericMenu();
            
        menu.AddItem(new GUIContent("Create/Create Dialogue"), false, CreateDialogueChunk);
        menu.AddItem(new GUIContent("Create/Create Choice"), false, CreateDialogueChunk);
        menu.AddItem(new GUIContent("Create/Create Minigame"), false, CreateDialogueChunk);
        menu.AddItem(new GUIContent("Check Size"), false, CheckSizeOfStory);

        menu.ShowAsContext();
        
 
        current.Use(); 
    }
    
        
    [UnityEditor.Callbacks.OnOpenAsset(1)]
    public static bool OpenDialogue(int assetInstanceID, int line)
    {
        Story story = EditorUtility.InstanceIDToObject(assetInstanceID) as Story;

        if (story != null)
        {
            StoryEditor window = ShowEditor();
            window.LoadNewAsset(story);
            return true;
        }
        return false;
    }

    private void LoadNewAsset(Story story)
    {
        this.story = story;
        
        allEventNodes.Clear();

    }

    
    private void RenderInspector()
    {
        if (storyInspector == null)
        {
            storyInspector = new StoryInspector();
        }


        if (selectedNode != null)
        {
            storyInspector.Render(selectedNode.storyEvent, this);
        }
    }

    private void Save()
    {
        if (story.fileSavePath == null)
        {
            story.fileSavePath = Application.persistentDataPath + "/" + story.name + ".json";

        }

        string jsonString = JsonUtility.ToJson(story);
        
        File.WriteAllText(story.fileSavePath, jsonString);

    }
}

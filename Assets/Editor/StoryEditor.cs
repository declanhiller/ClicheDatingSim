using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryEditor : EditorWindow {

    private StoryManager storyManager;
    [SerializeField] private Story story;

    private List<UIEventNode> allEventNodes = new List<UIEventNode>();
    
    private Vector2 mousePos;

    private UIEventNode hoveringOverNode;
    private UIEventNode selectedNode;

    private StoryInspector storyInspector;

    private Transform currentAsset;
    
    private UIEventNode rightClickedNode;

    private UIEventNode connectionNode;

    [MenuItem("Window/Story Creator")]   
    static StoryEditor ShowEditor() {     
        StoryEditor editor = EditorWindow.GetWindow<StoryEditor>();      
        editor.Init();
        return editor;
    }

    private void OnEnable() {
        EventNodeFactory.SetupFactory();
        ChangeStoryManager();
    }

    private void OnSelectionChange() {
        ChangeStoryManager();
    }

    private void OnFocus() {
        ChangeStoryManager();
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

            if (GUI.changed) {
                Repaint();
            }
        }

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
    

    private void RenderCurrentConnectionLine()
    {
        if (connectionNode != null)
        {
            EditorUtils.RenderNodeConnection(connectionNode.rect.center, mousePos);
            Repaint();
        }
    }

    private void RenderConnections()
    {
        UIEventNode[] copiedArray = new UIEventNode[allEventNodes.Count];
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
            EditorUtils.RenderNodeConnection(parentNode.rect, node.rect);
            RenderConnectionsRecursively(copiedList, node);
        }
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

    private void RefreshIDs() {
        int idCounter = 0;
        foreach (StoryEvent storyEvent in story.allEvents) {
            storyEvent.id = idCounter;
            idCounter++;
        }

        foreach (StoryEvent storyEvent in story.allEvents) {
            storyEvent.childrenID.Clear();
            foreach (StoryEvent child in storyEvent.childEvents) {
                storyEvent.childrenID.Add(child.id);
            }
        }
    }
    
    
    private void ChangeStoryManager() {
        Transform selectedAsset = Selection.activeTransform;
        if (selectedAsset == null) {
            //save game
            allEventNodes.Clear();
            storyManager = null;
            story = null;
            currentAsset = null;
            Repaint();
            return;
        }

        //don't do anything, asset hasn't changed
        if (currentAsset != null && currentAsset == selectedAsset) {
            return;
        }

        bool gotComponent = selectedAsset.TryGetComponent(out StoryManager storyManagerOut);
        if (!gotComponent) {
            allEventNodes.Clear();
            storyManager = null;
            story = null;
            currentAsset = null;
            Repaint();
            return;
        }
        
        LoadNewAsset(storyManagerOut);
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
        RefreshIDs();
    }

    [UnityEditor.Callbacks.OnOpenAsset(1)]
    public static bool OpenStory(int assetInstanceID, int line)
    {
        StoryManager storyManager = EditorUtility.InstanceIDToObject(assetInstanceID) as StoryManager;
        if (storyManager != null) {
            StoryEditor window = ShowEditor();
            window.LoadNewAsset(storyManager);
            return true;
            
        }
        
        return false;
    }

    private void LoadNewAsset(StoryManager manager) {
        
        allEventNodes.Clear();
        
        storyManager = manager;
        story = manager.story;
        
#if UNITY_EDITOR
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
#endif
        
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
    
    private void DisplayRegularContextMenu(Event current) {
        GenericMenu menu = new GenericMenu();
            
        menu.AddItem(new GUIContent("Create/Create Dialogue"), false, CreateDialogueChunk);
        menu.AddItem(new GUIContent("Create/Create Choice"), false, CreateDialogueChunk);
        menu.AddItem(new GUIContent("Create/Create Minigame"), false, CreateDialogueChunk);
        menu.AddItem(new GUIContent("Check Size"), false, CheckSizeOfStory);

        menu.ShowAsContext();
        
 
        current.Use(); 
    }
    
    private void DisplaySpecialContextMenu(Event current, UIEventNode uiEventNode) {
        GenericMenu menu = new GenericMenu();

        rightClickedNode = uiEventNode;
            
        menu.AddItem(new GUIContent("Delete"), false, Delete);
        menu.AddItem(new GUIContent("CreateTransition"), false, CreateConnection);
        
        menu.ShowAsContext();
 
        current.Use(); 
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
    
    private void DragNode(Event current)
    {
        if (hoveringOverNode != null)
        {
            hoveringOverNode.rect.position += current.delta;
            GUI.changed = true;
        }
    }
    
    private void DragOnWindow(Vector2 delta)
    {
        foreach (UIEventNode eventNode in allEventNodes)
        {
            DragSpecificNode(eventNode, delta); 
        }

        GUI.changed = true;
    }
    
    private void DragSpecificNode(UIEventNode node, Vector2 delta)
    {
        node.rect.position += delta;
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
    
    private void CreateConnection()
    {
        connectionNode = rightClickedNode;
    }
    
    void CheckSizeOfStory() {
        Debug.Log("There is " + story.allEvents.Count + " events in the story");
    }
}

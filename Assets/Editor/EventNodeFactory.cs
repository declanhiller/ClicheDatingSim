public class EventNodeFactory {
    
    public static UIEventNode createNode(float x, float y, StoryEvent storyEvent) {
        UIEventNode uiEventNode = new UIEventNode(x, y, storyEvent);
        return uiEventNode;
    }

    public static void SetupFactory() {
        UIEventNode.SetSpecialValues();
    }
    
}
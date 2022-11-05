public class EventNodeFactory {
    
    public static EventNode createNode(float x, float y, StoryEvent storyEvent) {
        EventNode eventNode = new EventNode(x, y, storyEvent);
        return eventNode;
    }

    public static void SetupFactory() {
        EventNode.SetSpecialValues();
    }
    
}
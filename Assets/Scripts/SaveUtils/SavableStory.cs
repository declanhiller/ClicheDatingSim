[System.Serializable]
public class SavableStory
{
    public string name;
    public SavableDialogue[] dialogueBoxes;
    public SavableCutscene[] cutsceneBoxes;
    public SavableSceneStart[] sceneStartBoxes;
    public int startId;
}
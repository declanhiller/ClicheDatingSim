[System.Serializable]
public class SavableStory
{
    public string name;
    public SavableDialogue[] dialogueBoxes;
    public SavableSceneStart[] sceneStartBoxes;
    public SavableOption[] optionBoxes;
    public int startId;
}
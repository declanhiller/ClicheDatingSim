[System.Serializable]
public class SavableDialogue : SavableEvent
{
    public int character;
    public string dialogue;

    public SavableDialogue(Dialogue dialogue)
    {
        this.dialogue = dialogue.dialogue;
        this.character = (int) dialogue.character;
    }
}
[System.Serializable]
public class SavableDialogue : SavableEvent
{
    public int character;
    public string dialogue;
    public int expression;

    public SavableDialogue(Dialogue dialogue)
    {
        this.dialogue = dialogue.dialogue;
        this.character = (int) dialogue.character;
        this.expression = (int) dialogue.expression;
    }
}
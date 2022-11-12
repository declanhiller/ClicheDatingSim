[System.Serializable]
public class SavableOption : SavableEvent {
    public string optionStr;

    public SavableOption(Option option) {
        optionStr = option.option;
    }
}
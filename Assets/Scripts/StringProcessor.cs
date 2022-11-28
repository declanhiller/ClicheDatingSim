using TMPro;

public class StringProcessor {
    
    public static string process(string text)
    {
        string returnText = text;
        while (returnText.Contains("@name@"))
        {
            returnText = returnText.Replace("@name@", PlayerData.playerName);
        }
        
        return returnText;
    }
}
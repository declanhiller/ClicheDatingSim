using TMPro;

public class StringProcessor {
    
    public static string process(string text)
    {
        string returnText = text;
        while (returnText.Contains("@name@"))
        {
            returnText = returnText.Replace("@name@", PlayerData.playerName);
        }
        
        while (returnText.Contains("#pronoun#"))
        {
            returnText = returnText.Replace("#pronoun#", PlayerData.pronoun1);
        }
        
        while (returnText.Contains("$pronoun$"))
        {
            returnText = returnText.Replace("$pronoun$", PlayerData.pronoun2);
        }
        
        while (returnText.Contains("%pronoun%"))
        {
            returnText = returnText.Replace("%pronoun%", PlayerData.pronoun3);
        }

        return returnText;
    }
}
using System.IO;
using UnityEngine;

public class StoryManager : MonoBehaviour {
    public Story story;
    public string managerName;
    public string fileSavePath;
    
    
    public void Save() {
        if (fileSavePath == null)
        {
            fileSavePath = Application.persistentDataPath + "/" + managerName + ".json";

        }
        
        string jsonString = JsonUtility.ToJson(story);
        
        File.WriteAllText(fileSavePath, jsonString);
    }

    public void LoadSave() {
        using StreamReader reader = new StreamReader(fileSavePath);
        string json = reader.ReadToEnd();
        // File.ReadAllText(fileSavePath);

        story = JsonUtility.FromJson<Story>(json);
    }
}
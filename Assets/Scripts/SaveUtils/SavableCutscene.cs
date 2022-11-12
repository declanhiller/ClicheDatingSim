using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SavableCutscene : SavableEvent {
    public string cutsceneName;
    public int textureFormat;
    public string path;


    public SavableCutscene(Cutscene cutscene) {
        this.cutsceneName = cutscene.cutsceneName;
        string assetPath = AssetDatabase.GetAssetPath(cutscene.image);
        path = assetPath;
    }
}
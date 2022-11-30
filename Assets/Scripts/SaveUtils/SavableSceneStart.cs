using UnityEditor;

[System.Serializable]
public class SavableSceneStart : SavableEvent {
    public string sceneName;
    public string backgroundImagePath;

    public SavableSceneStart(SceneStart sceneStart) {
        sceneName = sceneStart.sceneName;
        //use resources.LoadAll
        string assetPath = AssetDatabase.GetAssetPath(sceneStart.background);
        backgroundImagePath = assetPath;
    }
}
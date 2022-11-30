using UnityEditor;

[System.Serializable]
public class SavableSceneStart : SavableEvent {
    public string sceneName;
    public string backgroundImagePath;
    public string backgroundName;

    public SavableSceneStart(SceneStart sceneStart) {
        sceneName = sceneStart.sceneName;
        //use resources.LoadAll
        #if UNITY_EDITOR
        string assetPath = AssetDatabase.GetAssetPath(sceneStart.background);
        backgroundImagePath = assetPath;
        #endif
        if (sceneStart.background != null) {
            backgroundName = sceneStart.background.name;
        }
    }
}
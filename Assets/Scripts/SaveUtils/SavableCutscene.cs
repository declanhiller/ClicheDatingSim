using UnityEngine;

[System.Serializable]
public class SavableCutscene : SavableEvent {
    public string cutsceneName;
    public int textureFormat;
    public byte[] texBytes;
    public int width;
    public int height;
    

    public SavableCutscene(Cutscene cutscene) {
        this.cutsceneName = cutscene.cutsceneName;
        width = cutscene.image.width;
        height = cutscene.image.height;
        this.texBytes = cutscene.image.GetRawTextureData();
        textureFormat = (int) cutscene.image.format;
    }
}
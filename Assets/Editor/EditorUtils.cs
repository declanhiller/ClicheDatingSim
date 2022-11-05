using UnityEngine;

public class EditorUtils {
    public static Texture2D MakeTextureForNode(int width, int height)
    {
        Texture2D t2d = new Texture2D(width, height);
        Color color = new Color(0.5f, 0.5f, 0.5f);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                t2d.SetPixel(x, y, color);
            }
        }
        t2d.Apply();
        return t2d;
    }
}

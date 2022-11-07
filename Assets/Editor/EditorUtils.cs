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
    
    public static Texture2D MakeTextureForInspector()
    {
        Texture2D t2d = new Texture2D(10, 10);
        Color color = new Color(0.1f, 0.1f, 0.1f);
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                t2d.SetPixel(x, y, color);
            }
        }
        t2d.Apply();
        return t2d;
    }
    
    public static Texture2D MakeTextureForSelectedStyle(int width, int height, int border)
    {
        Texture2D t2d = new Texture2D(width, height);
        Color color = new Color(0.5f, 0.5f, 0.5f);
        Color selectedColor = new Color(0.8f, 0.2f, 0.2f);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x > border && x < width - border && y > border && y < height - border)
                {
                    t2d.SetPixel(x, y, color);
                }
                else
                {
                    t2d.SetPixel(x, y, selectedColor);
                }
            }
        }
        t2d.Apply();
        return t2d;
    }
}

using UnityEditor;
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
    
    public static void RenderArrowTip(Vector2 pos, Vector2 dir)
    {
        float rotAmount = 25;
        float len = 25;

        Vector2 leftLine = Quaternion.Euler(0, 0, rotAmount) * -dir;
        Vector2 leftEnd = pos + leftLine.normalized * len;

        Vector2 rightLine = Quaternion.Euler(0, 0, -rotAmount) * -dir;
        Vector2 rightEnd = pos + rightLine.normalized * len;


        Vector3[] points = new Vector3[4];
        points[0] = leftEnd;
        points[1] = pos;
        points[2] = rightEnd;
        points[3] = leftEnd;

        Color storedColor = Handles.color;
        Handles.color = new Color(0, 0.2f, 0.8f);
        Handles.DrawAAConvexPolygon(points);
        Handles.color = storedColor;

    }
    
    public static void RenderNodeConnection(Rect start, Rect end) {
        RenderNodeConnection(start.center, end.center);

    }

    public static void RenderNodeConnection(Vector3 startPos, Vector3 endPos)
    {
        Vector3 startHandle = (startPos - endPos).normalized;
        Vector3 endHandle = (endPos - startPos).normalized;
        Color shadowCol = new Color(0, 0, 0, 0.06f);
        Handles.DrawBezier(startPos, endPos, startPos + startHandle, endPos + endHandle, Color.black, null, 4);
        Vector3 dirForArrow = (endPos - startPos).normalized;
        // Vector2 midPoint = new Vector2((endPos.x + startPos.x) / 2, (endPos.y + startPos.y) / 2);
        RenderArrowTip((startPos + endPos) / 2, dirForArrow);
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
        Color color = new Color(0.6f, 0.6f, 0.6f);
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

    public static Texture2D MakeTextureForNodeTitles()
    {
        int width = 10;
        int height = 10;
        Texture2D t2d = new Texture2D(width, height);
        Color color = new Color(0.5f, 0.5f, 0.8f);
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

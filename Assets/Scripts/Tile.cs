using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour, ITile
{
    [SerializeField] Image image;
    private int x;
    private int y;
    private int width;
    private int height;

    RectTransform rt;
    private Texture2D tex;
    private Dictionary<int, Color32> pixels;
    private bool isDirty;

    public void Init(RectInt rect)
    {
        rt = GetComponent<RectTransform>();
        x = rect.x;
        y = rect.y;
        width = rect.width;
        height = rect.height;

        tex = new Texture2D(width, height);
        pixels = Enumerable.Range(0, width * height)
            .ToDictionary(x => x, x => new Color32(255, 255, 255, 255));
        tex.SetPixels32(pixels.Values.ToArray());
        tex.Apply();
        var sprite = Sprite.Create(tex, new Rect(0, 0, width, height), Vector2.zero);
        image.sprite = sprite;

    }

    public bool SetPixel(Vector2 oldPosition, Vector2 newPosition, Color32 color)
    {
        var oldPositionInt = new Vector2Int((int)oldPosition.x, (int)oldPosition.y);
        var newPositionInt = new Vector2Int((int)newPosition.x, (int)newPosition.y);
        var dist = Vector2Int.Distance(oldPositionInt, newPositionInt);
        for (int i = 0; i < dist; i++)
        {
            var position = Vector2.Lerp(oldPosition, newPosition, i / dist);
            Vector2 localPosition = transform.InverseTransformPoint(position);

            var pvx = rt.pivot.x;
            var pvy = rt.pivot.y;
            var w = rt.rect.width;
            var h = rt.rect.height;
            var v = new Vector3(localPosition.x + pvx * w, localPosition.y + pvy * h);

            var x = (int)v.x;
            var y = (int)v.y;

            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                var coord = x + y * height;
                pixels[coord] = color;
                tex.SetPixels32(pixels.Values.ToArray());
                isDirty = true;
            }
        }
        return isDirty;
    }

    public void UpdateTexture()
    {
        if (isDirty)
        {
            tex.Apply();
            isDirty = false;
        }
    }
}

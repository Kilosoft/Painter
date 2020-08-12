using UnityEngine;

public interface ITile
{
    void Init(RectInt rect);
    bool SetPixel(Vector2 oldPosition, Vector2 position, Color32 color);
    void UpdateTexture();
}
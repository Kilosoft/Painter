using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintProccessor : MonoBehaviour
{
    [SerializeField] GridLayoutGroup grid;
    [SerializeField] GameObject tilePrefab;

    [SerializeField] Vector2Int sizeImage;
    [SerializeField] Vector2Int sizeTile;
    [SerializeField] Color32 colorBrush;

    private ITile[,] tiles;

    private Vector2 oldPosition;
    private Stack<ITile> updateList;

    void Start()
    {
        updateList = new Stack<ITile>();
        grid.cellSize = sizeTile;
        grid.GetComponent<RectTransform>().sizeDelta = sizeImage;
        var maxCountX = sizeImage.x / sizeTile.x;
        var maxCountY = sizeImage.y / sizeTile.y;
        tiles = new ITile[maxCountX, maxCountY];
        for (int y = 0; y < maxCountY; y++)
        {
            for (int x = 0; x < maxCountX; x++)
            {
                var go = Instantiate(tilePrefab, grid.GetComponent<RectTransform>());
                var tile = go.GetComponent<ITile>();
                tile.Init(new RectInt(x, y, sizeTile.x, sizeTile.y));
                tiles[x, y] = tile;
            }
        }
    }

    private void Update()
    {
        Vector2 position = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            oldPosition = position;
            updateList.Clear();
        }
        if (Input.GetMouseButton(0))
        {
            var width = tiles.GetLength(0);
            var height = tiles.GetLength(1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var tile = tiles[x, y];
                    var isDirty = tile.SetPixel(oldPosition, position, colorBrush);
                    if (isDirty)
                    {
                        updateList.Push(tile);
                    }
                }
            }

            while (updateList.Count > 0)
            {
                var tileToUpdate = updateList.Pop();
                tileToUpdate.UpdateTexture();
            }
        }
        oldPosition = position;
    }
}

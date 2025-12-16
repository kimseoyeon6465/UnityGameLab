using System;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }

    public int width = 10;
    public int height = 6;
    public float cellSize = 1f;
    public GameObject tilePrefab;

    public event Action OnBoardResized;

    BoardTile[,] tiles;
    Vector2 origin;
    bool isInitialized;
    bool hasDefaultSize;
    int defaultWidth;
    int defaultHeight;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void InitializeBoard(int newWidth, int newHeight)
    {
        if (!hasDefaultSize)
        {
            defaultWidth = newWidth;
            defaultHeight = newHeight;
            hasDefaultSize = true;
        }

        if (isInitialized)
            ClearBoard();

        width = newWidth;
        height = newHeight;

        tiles = new BoardTile[width, height];

        if (tilePrefab == null)
        {
            Debug.LogError("BoardManager tilePrefab not set");
            return;
        }

        float totalWidth = (width - 1) * cellSize;
        float totalHeight = (height - 1) * cellSize;
        origin = new Vector2(-totalWidth / 2f, -totalHeight / 2f);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 pos = GetWorldPosition(new Vector2Int(x, y));
                GameObject go = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                go.name = $"Tile_{x}_{y}";

                var tile = go.GetComponent<BoardTile>();
                if (tile == null) tile = go.AddComponent<BoardTile>();

                tile.Initialize(new Vector2Int(x, y));
                tiles[x, y] = tile;
            }
        }

        isInitialized = true;
        OnBoardResized?.Invoke();
    }

    void ClearBoard()
    {
        tiles = null;
        for (int i = transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(transform.GetChild(i).gameObject);
    }

    public Vector2 GetWorldPosition(Vector2Int gridPos)
    {
        return origin + new Vector2(gridPos.x * cellSize, gridPos.y * cellSize);
    }

    public bool IsInsideBoard(Vector2Int gridPos)
    {
        return gridPos.x >= 0 && gridPos.x < width &&
               gridPos.y >= 0 && gridPos.y < height;
    }

    public BoardTile GetTile(Vector2Int gridPos)
    {
        if (!IsInsideBoard(gridPos)) return null;
        return tiles[gridPos.x, gridPos.y];
    }

    public void ResetAllTilesToNormal()
    {
        if (tiles == null) return;

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                var tile = tiles[x, y];
                if (tile != null)
                    tile.SetState(TileState.Normal);
            }
    }

    public void ResetToDefaultSize()
    {
        if (!hasDefaultSize) return;
        InitializeBoard(defaultWidth, defaultHeight);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public static Board instance;

    [Header("Refs")]
    public Tilemap tilemap;

    //유효 타일들
    public HashSet<Vector3Int> validTiles = new HashSet<Vector3Int>();

    public Vector3Int minBound;
    public Vector3Int maxBound;

    public List<int> validXs = new List<int>();
    public List<int> validYs = new List<int>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CacheBoard();
    }

    public bool IsValid(Vector3Int pos)
    {
        // 보드 범위 체크
        if (pos.x < minBound.x || pos.x > maxBound.x ||
            pos.y < minBound.y || pos.y > maxBound.y)
            return false;
        // 캐싱된 validTiles로 체크
        return validTiles.Contains(pos);
    }

    private void CacheBoard()
    {
        validTiles.Clear();
        validXs.Clear();
        validYs.Clear();

        BoundsInt bounds = tilemap.cellBounds;
        minBound = bounds.min;
        maxBound = bounds.max - Vector3Int.one;

        for(int x=minBound.x; x<=maxBound.x; x++)
        {
            for(int y=minBound.y; y<=maxBound.y;y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);

                if (tilemap.HasTile(pos))
                {
                    validTiles.Add(pos);
                    if(!validXs.Contains(x)) validXs.Add(x);
                    if(!validYs.Contains(y)) validYs.Add(y);
                }
            }
        }

        validXs.Sort();
        validYs.Sort();

        Debug.Log($"[Board] 타일 캐싱 완료 -> {validTiles.Count}개 타일");
    }
}

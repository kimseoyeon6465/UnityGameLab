using System.Collections.Generic;
using UnityEngine;

public static class BoardPatterns
{
    public static List<Vector2Int> Line(Vector2Int start, Vector2Int dir, int maxLen, BoardManager board)
    {
        var result = new List<Vector2Int>();

        Vector2Int pos = start;
        for (int i = 0; i < maxLen; i++)
        {
            if (!board.IsInsideBoard(pos))
                break;

            result.Add(pos);
            pos += dir;
        }

        return result;
    }

    public static List<Vector2Int> LineToEdge(Vector2Int start, Vector2Int dir, BoardManager board)
    {
        var result = new List<Vector2Int>();

        Vector2Int pos = start;
        while (board.IsInsideBoard(pos))
        {
            result.Add(pos);
            pos += dir;
        }

        return result;
    }

    public static List<Vector2Int> Cross(Vector2Int center, BoardManager board)
    {
        var result = new List<Vector2Int>();

        result.Add(center);

        result.AddRange(LineToEdge(center + Vector2Int.left, Vector2Int.left, board));
        result.AddRange(LineToEdge(center + Vector2Int.right, Vector2Int.right, board));
        result.AddRange(LineToEdge(center + Vector2Int.up, Vector2Int.up, board));
        result.AddRange(LineToEdge(center + Vector2Int.down, Vector2Int.down, board));

        return result;
    }

    public static List<Vector2Int> DiagonalCross(Vector2Int center, BoardManager board)
    {
        var result = new List<Vector2Int>();

        result.Add(center);

        Vector2Int[] dirs =
        {
            new Vector2Int(1, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, -1)
        };

        foreach (var dir in dirs)
        {
            result.AddRange(LineToEdge(center + dir, dir, board));
        }

        return result;
    }

    public static List<Vector2Int> PlusDiagonalCross(Vector2Int center, BoardManager board)
    {
        var result = Cross(center, board);
        result.AddRange(DiagonalCross(center, board));
        return result;
    }
}

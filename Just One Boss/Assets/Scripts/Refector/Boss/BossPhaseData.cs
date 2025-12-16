using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossAttackData
{
    public string attackId;
    public int repeat;
    public float cooldown;
}

[System.Serializable]
public class BossPhaseData
{
    public string name;
    public float minHpPercent;
    public float maxHpPercent;
    public string attackOrder;
    public BossAttackData[] attacks;

    public bool overrideBoardSize;
    public int boardWidth;
    public int boardHeight;
}

[System.Serializable]
public class BossData
{
    public string bossId;
    public int maxHp;
    public BossPhaseData[] phases;
}

public interface IBossAttack
{
    IEnumerator Execute(BossController boss);
}

public static class BoardRandomUtil
{
    public static List<Vector2Int> GetRandomDistinctCells(BoardManager board, int count)
    {
        var result = new List<Vector2Int>();
        if (board == null || count <= 0) return result;

        int maxCount = board.width * board.height;
        count = Mathf.Min(count, maxCount);

        var picked = new HashSet<Vector2Int>();
        int safety = 0;

        while (result.Count < count && safety < maxCount * 5)
        {
            safety++;
            int x = Random.Range(0, board.width);
            int y = Random.Range(0, board.height);
            var cell = new Vector2Int(x, y);
            if (picked.Add(cell))
                result.Add(cell);
        }

        return result;
    }

    public static Vector2Int GetRandomCell(BoardManager board)
    {
        int x = Random.Range(0, board.width);
        int y = Random.Range(0, board.height);
        return new Vector2Int(x, y);
    }
}

public class CrossWarningAttack : IBossAttack
{
    public float warnTime = 0.7f;
    public float damageTime = 0.1f;
    public int damage = 1;

    public IEnumerator Execute(BossController boss)
    {
        var board = BoardManager.Instance;
        var warning = BoardWarningSystem.Instance;
        if (board == null || warning == null) yield break;

        var center = BoardRandomUtil.GetRandomCell(board);
        var cells = BoardPatterns.Cross(center, board);

        yield return warning.ShowWarningThenDamage(cells, warnTime, damageTime, damage);
    }
}

public class DiagonalWarningAttack : IBossAttack
{
    public float warnTime = 0.7f;
    public float damageTime = 0.1f;
    public int damage = 1;

    public IEnumerator Execute(BossController boss)
    {
        var board = BoardManager.Instance;
        var warning = BoardWarningSystem.Instance;
        if (board == null || warning == null) yield break;

        var center = BoardRandomUtil.GetRandomCell(board);
        var cells = BoardPatterns.DiagonalCross(center, board);

        yield return warning.ShowWarningThenDamage(cells, warnTime, damageTime, damage);
    }
}

public class VerticalLineWarningAttack : IBossAttack
{
    public float warnTime = 0.7f;
    public float damageTime = 0.1f;
    public int damage = 1;

    public IEnumerator Execute(BossController boss)
    {
        var board = BoardManager.Instance;
        var warning = BoardWarningSystem.Instance;
        if (board == null || warning == null) yield break;

        int col = Random.Range(0, board.width);
        var start = new Vector2Int(col, 0);
        var cells = BoardPatterns.LineToEdge(start, Vector2Int.up, board);

        yield return warning.ShowWarningThenDamage(cells, warnTime, damageTime, damage);
    }
}

public class RandomCellsWarningAttack : IBossAttack
{
    public int cellCount = 3;
    public float warnTime = 0.7f;
    public float damageTime = 0.1f;
    public int damage = 1;

    public IEnumerator Execute(BossController boss)
    {
        var board = BoardManager.Instance;
        var warning = BoardWarningSystem.Instance;
        if (board == null || warning == null) yield break;

        var cells = BoardRandomUtil.GetRandomDistinctCells(board, cellCount);
        if (cells.Count == 0) yield break;

        yield return warning.ShowWarningThenDamage(cells, warnTime, damageTime, damage);
    }
}

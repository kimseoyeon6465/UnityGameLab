using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardWarningSystem : MonoBehaviour
{
    public static BoardWarningSystem Instance { get; private set; }

    private BoardManager board;
    private PlayerGridMoveController playerMover;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // playerHealth 제거
    public void Initialize(BoardManager boardManager, PlayerGridMoveController player)
    {
        board = boardManager;
        playerMover = player;
    }

    public IEnumerator ShowWarningThenDamage(
        List<Vector2Int> cells,
        float warnTime,
        float damageTime,
        int damage)
    {
        // 1. 경고 상태
        SetTilesState(cells, TileState.Warning);
        yield return new WaitForSeconds(warnTime);

        // 2. Danger 상태
        SetTilesState(cells, TileState.Danger);

        // 3. 플레이어가 타일 위에 있을 경우 처리 (현재는 로그만 표시)
        if (playerMover != null)
        {
            Vector2Int playerPos = playerMover.CurrentGridPos;

            foreach (var cell in cells)
            {
                if (cell == playerPos)
                {
                    Debug.Log($"Player hit by tiles, expected damage = {damage}");
                    break;
                }
            }
        }

        // 데인저 유지 시간
        if (damageTime > 0)
            yield return new WaitForSeconds(damageTime);

        // 4. 타일 원상복귀
        SetTilesState(cells, TileState.Normal);
    }

    private void SetTilesState(IEnumerable<Vector2Int> cells, TileState state)
    {
        if (board == null)
            return;

        foreach (var cell in cells)
        {
            var tile = board.GetTile(cell);
            if (tile != null)
                tile.SetState(state);
        }
    }
}

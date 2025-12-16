using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public float spawnInterval = 2f;

    private BoardManager board;

    private void Start()
    {
        board = BoardManager.Instance;
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (board == null || ItemManager.Instance == null)
                continue;

            // 보드 안에서 랜덤 위치
            int x = Random.Range(0, board.width);
            int y = Random.Range(0, board.height);
            Vector2Int gridPos = new Vector2Int(x, y);

            ItemManager.Instance.SpawnItem(gridPos);
        }
    }
}

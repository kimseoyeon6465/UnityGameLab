using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    public GameObject itemPrefab;

    Dictionary<Vector2Int, ItemOnBoard> items = new Dictionary<Vector2Int, ItemOnBoard>();
    BoardManager board;
    bool canSpawn = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        board = BoardManager.Instance;
        if (board != null)
            board.OnBoardResized += HandleBoardResized;
    }

    void OnDestroy()
    {
        if (board != null)
            board.OnBoardResized -= HandleBoardResized;
    }

    public void EnableSpawn(bool enable)
    {
        canSpawn = enable;
    }

    public void ClearAllItems()
    {
        foreach (var kv in items)
        {
            if (kv.Value != null)
                Destroy(kv.Value.gameObject);
        }
        items.Clear();
    }

    public ItemOnBoard SpawnItem(Vector2Int pos)
    {
        if (!canSpawn) return null;
        if (board == null || itemPrefab == null) return null;
        if (items.ContainsKey(pos)) return null;

        Vector3 world = board.GetWorldPosition(pos);
        GameObject go = Instantiate(itemPrefab, world, Quaternion.identity, transform);
        var item = go.GetComponent<ItemOnBoard>();
        if (item == null)
        {
            Destroy(go);
            return null;
        }

        item.Initialize(pos);
        items[pos] = item;
        return item;
    }

    public void TryPickupItem(Vector2Int pos)
    {
        if (!items.TryGetValue(pos, out var item)) return;
        items.Remove(pos);
        item.Pickup();
    }

    void HandleBoardResized()
    {
        if (board == null) return;

        var keys = items.Keys.ToList();
        foreach (var k in keys)
        {
            if (!board.IsInsideBoard(k))
            {
                if (items.TryGetValue(k, out var item))
                {
                    items.Remove(k);
                    if (item != null)
                        Destroy(item.gameObject);
                }
            }
        }

        foreach (var kv in items)
        {
            ItemOnBoard item = kv.Value;
            if (item == null) continue;
            item.UpdateWorldPosition();
        }
    }
}
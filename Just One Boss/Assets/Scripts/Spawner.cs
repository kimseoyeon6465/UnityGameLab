using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject cardPrefab;   // Card 컴포넌트가 있는 프리팹
    [SerializeField] private GameObject dashPrefab; // 스폰할 Dash 프리팹
    [SerializeField] private Player player;


    [Header("Spawn Settings")]
    [SerializeField] private float dashSpawnInterval = 4f; // 몇 초마다 생성할지
    [SerializeField] private float CardSpawnInterval = 2.5f;
    [SerializeField] private float cardMoveSpeed = 5.0f;
    [SerializeField] private float cardRotateSpeed = 100f;

    public enum Pattern { G_N_D_R_M, D_G_R_N_M }
    [SerializeField] private Pattern startPattern = Pattern.G_N_D_R_M;
    [SerializeField] private bool loop = true; // 계속 반복할지

    private float timer;
    private float cardTimer;
    [HideInInspector] public bool isDashSpawned = false; // 상태 플래그
    private Vector3Int lastSpawnedPos; // 마지막 스폰된 위치 저장

    private struct PointInfo
    {
        public Vector3Int cell;
        public Vector3 dir;
        public PointInfo(Vector3Int c, Vector3 d) { cell = c; dir = d; }
    }

    private Dictionary<string, PointInfo> points;

    private void Awake()
    {
        // 좌표/방향 지정
        points = new Dictionary<string, PointInfo>()
        {
            { "ㄱ", new PointInfo(new Vector3Int(-8,  1, 0), Vector3.right) }, // 오른쪽
            { "ㄴ", new PointInfo(new Vector3Int(-8, -1, 0), Vector3.right) }, // 오른쪽
            { "ㄷ", new PointInfo(new Vector3Int(-1,  2, 0), Vector3.left ) }, // 왼쪽
            { "ㄹ", new PointInfo(new Vector3Int(-1,  0, 0), Vector3.left ) }, // 왼쪽
            { "ㅁ", new PointInfo(new Vector3Int(-1, -2, 0), Vector3.left ) }, // 왼쪽
        };
    }

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= dashSpawnInterval)
        {
            SpawnDash();
            timer = 0f;
        }

       
    }

    public void SetPlayer(Player p)
    {
        player = p;
    }

    void SpawnDash()
    {
        if (isDashSpawned) return; // 이미 스폰되어 있으면 무시

        BoundsInt bounds = tilemap.cellBounds;

        for (int i = 0; i < 20; i++) // 최대 20번 시도
        {
            int x = UnityEngine.Random.Range(bounds.xMin, bounds.xMax);
            int y = UnityEngine.Random.Range(bounds.yMin, bounds.yMax);
            Vector3Int cellPos = new Vector3Int(x, y, 0);

            // 타일이 있고, 이전에 스폰된 위치와 다르면 OK
            if (tilemap.HasTile(cellPos) && cellPos != lastSpawnedPos)
            {
                Vector3 worldPos = tilemap.GetCellCenterWorld(cellPos);
                GameObject dash = Instantiate(dashPrefab, worldPos, Quaternion.identity);

                // Dash에게 Spawner 참조 넘기기
                dash.GetComponent<Dash>().Init(this, player);

                lastSpawnedPos = cellPos;
                isDashSpawned = true;
                return;
            }
        }
    }
    private IEnumerator SpawnLoop()
    {
        // 두 가지 순서
        string[] order1 = { "ㄱ", "ㄴ", "ㄷ", "ㄹ", "ㅁ" };
        string[] order2 = { "ㄷ", "ㄱ", "ㄹ", "ㄴ", "ㅁ" };

        do
        {
            string[] order = UnityEngine.Random.value < 0.5f ? order1 : order2;
            yield return StartCoroutine(SpawnPattern(order));
        } while (loop);
    }

    private IEnumerator SpawnPattern(IEnumerable<string> order)
    {
        foreach (var key in order)
        {
            SpawnCard(key);
            yield return new WaitForSeconds(CardSpawnInterval);
        }
    }

    private void SpawnCard(string key)
    {
        if (!points.TryGetValue(key, out var info))
        {
            Debug.LogWarning($"[Spawner] Unknown key: {key}");
            return;
        }


        // 타일 중앙 월드 좌표
        Vector3 worldPos = tilemap != null
            ? tilemap.GetCellCenterWorld(info.cell)
            : (Vector3)info.cell;

        //Debug.Log($"[Spawner] Spawn '{key}' at cell {info.cell}, worldPos = {worldPos}");
        var go = Instantiate(cardPrefab, worldPos, Quaternion.identity);

        var card = go.GetComponent<Card>();
        if (card != null)
        {
            // 타일맵/방향/속도 적용
            if (tilemap != null) card.SetDependencies(tilemap);
            card.Init(info.dir, cardMoveSpeed, cardRotateSpeed, tilemap);
        }

    }

    private void InstantiateCard()
    {
        Instantiate(cardPrefab, cardPrefab.transform.position, Quaternion.identity);
    }
}

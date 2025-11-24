using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{

    public NegativeTileManager negativeTileManager;

    [Header("Refs")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject cardPrefab;   // Card 컴포넌트가 있는 프리팹
    [SerializeField] private GameObject dashPrefab; // 스폰할 Dash 프리팹
    [SerializeField] private Player player;


    [Header("Spawn Settings")]

    public bool enableCard = true;
    public bool enableDash = true;
    public bool enableLaser = true;
    public bool enableNegativeTile = false;
    public bool enableCoin = false;
    public bool enableClone = false;
    public bool enableBoss2 = false;

    public float dashSpawnInterval = 4f; // 몇 초마다 생성할지
    public float CardSpawnInterval = 2.5f;
    public float cardMoveSpeed = 5.0f;
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

        int count = Board.instance.validTiles.Count;
        if (count == 0) return;

        Vector3Int cellPos;
        int safety = 0;
        do
        {
            int index = UnityEngine.Random.Range(0, count);
            cellPos = Board.instance.validTiles.ElementAt(index);//ElementAt은 데이터 집합에서 특정 인덱스 반환
            safety++;
        }
        while (cellPos == lastSpawnedPos && safety > 20);

        Vector3 worldPos = tilemap.GetCellCenterWorld(cellPos);


        GameObject dash = Instantiate(dashPrefab, worldPos, Quaternion.identity);

        // Dash에게 Spawner 참조 넘기기
        dash.GetComponent<Dash>().Init(this, player);

        lastSpawnedPos = cellPos;
        isDashSpawned = true;
        
    }
    private IEnumerator SpawnLoop()
    {
        // 두 가지 순서
        string[] order1 = { "ㄱ", "ㄴ", "ㄷ", "ㄹ", "ㅁ" };
        string[] order2 = { "ㄷ", "ㄱ", "ㄹ", "ㄴ", "ㅁ" };

        do
        {
            string[] order = UnityEngine.Random.value < 0.5f ? order1 : order2;
            if (enableCard)
                yield return StartCoroutine(SpawnPattern(order));
            else
                yield return null;
        } while (loop);
    }

    private IEnumerator SpawnPattern(IEnumerable<string> order)
    {
        foreach (var key in order)
        {
            if (enableCard)
                SpawnCard(key);
            yield return new WaitForSeconds(CardSpawnInterval);
        }
    }

    private void SpawnCard(string key)
    {
        if (!enableCard)
            return;

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

    public void SpawnLaser()
    {
        if (!enableLaser) return;

        List<int> xs = Board.instance.validXs;

        int playerX = player.GetX();

        List<int> candidates = new List<int>();
        foreach(int x in xs)
        {
            if(x!=playerX)
                candidates.Add(x);
        }

        if (candidates.Count == 0)
        {
            Debug.LogWarning("레이저 스폰 실패");
            return;
        }
        int randomX = candidates[UnityEngine.Random.Range(0, candidates.Count)];

        int centerY = Board.instance.validYs[Board.instance.validYs.Count/2];

        Vector3Int cellPos = new Vector3Int(randomX, centerY, 0);
        Vector3 worldPos = tilemap.GetCellCenterWorld(cellPos);

        LaserManager.instance.FireLaser(worldPos, Vector3.down, 0.15f);
        StartCoroutine(SpawnGrowLaser(worldPos, randomX));
        
    }

    private IEnumerator SpawnGrowLaser(Vector3 worldPos, int spawnX)
    {
        yield return new WaitForSeconds(0.15f);  // 예고 끝난 후 등장

        GameObject laser = Instantiate(LaserManager.instance.laserPrefab, worldPos, Quaternion.identity);

        laser.transform.up = Vector3.down;

        // Grow + 이동 방향 판단
        laser.GetComponent<LaserProjectile>().Init(tilemap, spawnX);
    }

    public void SpawnNegativeTiles()
    {
        if (!enableNegativeTile) return;
        negativeTileManager.StartNegativeTiles();
    }

    public void SpawnCoin()
    {
        if (!enableCoin) return;
        // TODO: CoinProjectile.Create(player.position)
    }

    public void SpawnClone()
    {
        if (!enableClone) return;
        // TODO: Instantiate(ClonePrefab)
    }

    public void SpawnBoss2()
    {
        if (!enableBoss2) return;
        // TODO: Instantiate(Boss2Prefab)
    }
    public void ResetDash()
    {
        Dash d = FindObjectOfType<Dash>();
        if (d != null)
        {
            Destroy(d.gameObject);
        }
        // 다시 Dash 스폰 가능하도록
        isDashSpawned = false;

        // 기본 Dash 바로 다시 스폰
        SpawnDash();
    }
}

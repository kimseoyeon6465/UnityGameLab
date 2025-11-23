using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Phase Info")]
    public int phase = 1;

    private Spawner spawner;
    private GameUI gameUI;

    private int score;
    private int bossGauge;

    private float laserTimer = 0f;
    public float laserInterval = 5f;

    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
        gameUI = FindObjectOfType<GameUI>();
    }

    private void Update()
    {
        score = gameUI.GetScore();
        bossGauge = gameUI.GetRageValue();

        TryPhaseChange();
        if (spawner.enableLaser)
        {
            laserTimer += Time.deltaTime;

            if (laserTimer >= laserInterval)
            {
                spawner.SpawnLaser();
                laserTimer = 0f;
            }
        }

    }

    void TryPhaseChange()
    {
        if (phase == 1 && score >= 9000 && bossGauge >= 10)
            SetPhase(2);

        else if (phase == 2 && score >= 14000 && bossGauge >= 10)
            SetPhase(3);

        else if (phase == 3 && score >= 18000 && bossGauge >= 10)
            SetPhase(4);

        else if (phase == 4 && score >= 22500 && bossGauge >= 10)
            EnterEnding();
    }
    void SetPhase(int newPhase)
    {
        phase = newPhase;
        Debug.Log("=== Phase → " + phase + " ===");

        ResetBossGauge();
        ApplyPhaseStartSettings(phase);

        //TODO: 연출
    }

    void EnterEnding()
    {
        Debug.Log("=== 엔딩 도달 ===");

        ResetBossGauge();

        // 엔딩에서는 전체 공격 스톱
        spawner.enableCard = false;
        spawner.enableLaser = false;
        spawner.enableCoin = false;
        spawner.enableNegativeTile = false;
        spawner.enableClone = false;
        spawner.enableBoss2 = false;
        spawner.enableDash = false;

        // TODO: 엔딩 UI/연출
    }

    void ResetBossGauge()
    {
        gameUI.AddRage(-9999);
    }

    void ApplyPhaseStartSettings(int p)
    {
        if (p == 1)
        {
            spawner.enableCard = true;
            spawner.enableDash = true;
            spawner.enableLaser = true;          // 레이저 예고 + 발사
            spawner.enableNegativeTile = true;   // 여러 Negative 발판
            spawner.enableCoin = false;
            spawner.enableClone = false;
            spawner.enableBoss2 = false;

            spawner.dashSpawnInterval = 4f;
            spawner.CardSpawnInterval = 2.5f;
        }
        else if (p == 2)
        {
            spawner.enableCoin = true;           // 코인(2번 충돌)
            spawner.enableLaser = true;
            spawner.enableNegativeTile = true;

            spawner.dashSpawnInterval = 3f;
            spawner.CardSpawnInterval = 2.0f;
        }
        else if (p == 3)
        {
            spawner.enableClone = true;          // 복제 플레이어 등장 (데미지 있음)
            spawner.enableCoin = true;

            spawner.dashSpawnInterval = 2f;
            spawner.CardSpawnInterval = 1.8f;
        }
        else if (p == 4)
        {
            spawner.enableBoss2 = true;          // Boss2 반대 공격
            spawner.enableClone = false;         // Clone 퇴장(선택)

            spawner.dashSpawnInterval = 1.5f;
            spawner.CardSpawnInterval = 1.5f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController Instance { get; private set; }

    public BossDataLoader dataLoader;
    public Transform ItemHitPivot;

    BossData data;
    int currentHp;
    int maxHp;

    BossPhaseData currentPhase;
    int currentPhaseIndex = -1;
    int sequenceAttackIndex;

    Coroutine attackRoutine;
    Dictionary<string, IBossAttack> attackRegistry = new Dictionary<string, IBossAttack>();

    bool isInitialized;
    bool isDead;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void InitializeBoss()
    {
        if (isInitialized) return;

        data = dataLoader != null ? dataLoader.Load() : null;
        if (data == null)
        {
            Debug.LogError("BossData load failed");
            enabled = false;
            return;
        }

        maxHp = data.maxHp;
        currentHp = maxHp;

        RegisterAttacks();
        UpdatePhaseByHp(force: true);

        isInitialized = true;
    }

    void RegisterAttacks()
    {
        attackRegistry["cross_warning"] = new CrossWarningAttack();
        attackRegistry["diagonal_warning"] = new DiagonalWarningAttack();
        attackRegistry["vertical_warning"] = new VerticalLineWarningAttack();
        attackRegistry["random_cells"] = new RandomCellsWarningAttack();
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        if (isDead) return;

        currentHp -= damage;
        if (currentHp <= 0)
        {
            currentHp = 0;
            Debug.Log($"Boss Die, HP {currentHp}/{maxHp}");
            OnBossDead();
            return;
        }

        Debug.Log($"Boss TakeDamage {damage}, HP {currentHp}/{maxHp}");
        UpdatePhaseByHp();
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;
        if (isDead) return;

        currentHp += amount;
        if (currentHp > maxHp) currentHp = maxHp;

        Debug.Log($"Boss Heal {amount}, HP {currentHp}/{maxHp}");
        UpdatePhaseByHp();
    }

    void UpdatePhaseByHp(bool force = false)
    {
        if (data == null || data.phases == null || data.phases.Length == 0) return;

        float hpPercent = maxHp > 0 ? (float)currentHp / maxHp : 0f;
        int newIndex = FindPhaseIndexByHp(hpPercent);
        if (newIndex == -1) return;

        if (newIndex != currentPhaseIndex || force)
        {
            currentPhaseIndex = newIndex;
            currentPhase = data.phases[currentPhaseIndex];
            OnPhaseChanged();
        }
    }

    int FindPhaseIndexByHp(float p)
    {
        for (int i = 0; i < data.phases.Length; i++)
        {
            var phase = data.phases[i];
            if (p > phase.minHpPercent && p <= phase.maxHpPercent)
                return i;
        }
        return -1;
    }

    void OnPhaseChanged()
    {
        if (currentPhase == null || isDead) return;

        Debug.Log($"Boss Phase -> {currentPhase.name}");

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        var board = BoardManager.Instance;
        if (board != null)
            board.ResetAllTilesToNormal();

        sequenceAttackIndex = 0;
        ApplyPhaseBoardSize();
        StartPhaseLoop();
    }

    void ApplyPhaseBoardSize()
    {
        if (currentPhase == null || !currentPhase.overrideBoardSize) return;

        var board = BoardManager.Instance;
        if (board == null) return;

        int w = Mathf.Max(1, currentPhase.boardWidth);
        int h = Mathf.Max(1, currentPhase.boardHeight);
        board.InitializeBoard(w, h);
    }

    void StartPhaseLoop()
    {
        if (currentPhase == null || isDead) return;

        if (attackRoutine != null)
            StopCoroutine(attackRoutine);

        attackRoutine = StartCoroutine(PhaseAttackLoop());
    }

    IEnumerator PhaseAttackLoop()
    {
        while (!isDead && currentHp > 0)
        {
            var attackData = GetNextAttackFromPhase(currentPhase);
            if (attackData == null)
            {
                yield return null;
                continue;
            }

            if (!attackRegistry.TryGetValue(attackData.attackId, out var attack))
            {
                Debug.LogWarning($"Unknown attackId {attackData.attackId}");
                if (attackData.cooldown > 0f)
                    yield return new WaitForSeconds(attackData.cooldown);
                continue;
            }

            for (int i = 0; i < attackData.repeat; i++)
            {
                yield return attack.Execute(this);
                if (attackData.cooldown > 0f)
                    yield return new WaitForSeconds(attackData.cooldown);

                if (isDead || currentHp <= 0)
                    yield break;
            }
        }
    }

    BossAttackData GetNextAttackFromPhase(BossPhaseData phase)
    {
        if (phase.attacks == null || phase.attacks.Length == 0) return null;

        if (phase.attackOrder == "random")
        {
            int idx = Random.Range(0, phase.attacks.Length);
            return phase.attacks[idx];
        }
        else
        {
            var attack = phase.attacks[sequenceAttackIndex];
            sequenceAttackIndex = (sequenceAttackIndex + 1) % phase.attacks.Length;
            return attack;
        }
    }

    void OnBossDead()
    {
        if (isDead) return;
        isDead = true;

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        var board = BoardManager.Instance;
        if (board != null)
        {
            board.ResetAllTilesToNormal();
            board.ResetToDefaultSize();
        }

        var itemMgr = ItemManager.Instance;
        if (itemMgr != null)
        {
            itemMgr.EnableSpawn(false);
            itemMgr.ClearAllItems();
        }

        // TODO game clear flow
    }
}

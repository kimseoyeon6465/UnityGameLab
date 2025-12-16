using System.Collections;
using UnityEngine;
public class ItemOnBoard : MonoBehaviour
{
    public int damage = 50;
    public float flyDuration = 0.5f;
    [SerializeField] float curveOffset = 1.5f;

    Vector2Int gridPos;
    bool picked;

    public void Initialize(Vector2Int pos)
    {
        gridPos = pos;
    }

    public void UpdateWorldPosition()
    {
        var board = BoardManager.Instance;
        if (board == null) return;
        transform.position = board.GetWorldPosition(gridPos);
    }

    public void Pickup()
    {
        if (picked) return;
        picked = true;
        StartCoroutine(FlyToBoss());
    }

    IEnumerator FlyToBoss()
    {
        var boss = BossController.Instance;
        if (boss == null)
        {
            Destroy(gameObject);
            yield break;
        }

        Transform target = boss.ItemHitPivot != null ? boss.ItemHitPivot : boss.transform;

        Vector3 p0 = transform.position;
        Vector3 p2 = target.position;
        Vector3 p1 = CalcControlPoint(p0, p2);

        float elapsed = 0f;
        while (elapsed < flyDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / flyDuration);
            transform.position = Bezier(p0, p1, p2, t);
            yield return null;
        }

        transform.position = p2;
        boss.TakeDamage(damage);
        Destroy(gameObject);
    }

    Vector3 CalcControlPoint(Vector3 a, Vector3 b)
    {
        Vector3 mid = (a + b) * 0.5f;
        Vector3 dir = (b - a).normalized;
        Vector3 perp = new Vector3(-dir.y, dir.x, 0f);
        float side = Random.value < 0.5f ? -1f : 1f;
        return mid + perp * curveOffset * side;
    }

    Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1f - t;
        return u * u * p0 + 2f * u * t * p1 + t * t * p2;
    }
}
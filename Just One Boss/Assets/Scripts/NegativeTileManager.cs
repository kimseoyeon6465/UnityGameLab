using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NegativeTileManager : MonoBehaviour//NegativeTileManager는 타일 색을 바꾸기만 함
{
    public Tilemap tilemap;

    public Color dangerColor = Color.red;
    public Color warningColor = Color.green;
    public float warningDuration = 1.5f;
    public float dangerDuration = 2.0f;
    private Color originalColor;

    private HashSet<Vector3Int> selectedTiles = new HashSet<Vector3Int>();
    private Dictionary<Vector3Int, Color> originalColors = new Dictionary<Vector3Int, Color>();

    public void StartNegativeTiles(float percent = 0.6f)
    {
        StopAllCoroutines();
        StartCoroutine(NegativeTilesRoutine(percent));
    }

    IEnumerator NegativeTilesRoutine(float percent)
    {
        selectedTiles.Clear();
        originalColors.Clear();

        BoundsInt bounds = tilemap.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;
            if (Random.value <= percent)
            {
                selectedTiles.Add(pos);

                //타일 컬러 변경위해 플래그 해제
                tilemap.SetTileFlags(pos, TileFlags.None);

                originalColors[pos] = tilemap.GetColor(pos);
                tilemap.SetColor(pos, dangerColor);
            }
        }
        yield return new WaitForSeconds(warningDuration);
        // 2) 초록 위험 단계 (충돌 O → Hazard 생성)

        foreach (var pos in selectedTiles)
        {
            tilemap.SetColor(pos, warningColor);
            //Hazard 생성 코드
            GameObject hazard = new GameObject("NegativeTileHazard");
            hazard.transform.position = tilemap.GetCellCenterWorld(pos);

            BoxCollider2D col = hazard.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = new Vector2(1f, 1f);
            hazard.AddComponent<NegativeTileHazard>();
            Destroy(hazard, dangerDuration);
        }

        yield return new WaitForSeconds(dangerDuration);
        foreach (var pos in selectedTiles)
        {
            tilemap.SetColor(pos, originalColors[pos]);
        }
        

        selectedTiles.Clear();
        originalColors.Clear();

    }

    public bool IsDanger(Vector3Int pos)
    {
        return tilemap.color == dangerColor;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Grid grid;//게임 시작시 인스턴스               
    [SerializeField] private Tilemap tilemap;         
    [SerializeField] private GameObject playerPrefab;
    public GameUI gameUI;

    void Start()
    {
        Vector3Int spawnIndex = new Vector3Int(-4, 0, 0); // 원하는 셀 인덱스
        Vector3 spawnPos = grid.GetCellCenterWorld(spawnIndex);

        GameObject playerObj = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

        Player playerScript = playerObj.GetComponent<Player>();
        playerScript.SetDependencies(grid, tilemap, gameUI, spawnIndex);
    }

   
}

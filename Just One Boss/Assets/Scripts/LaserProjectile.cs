using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LaserProjectile : Collidable
{
    public override bool CauseDamage() => true;

    public float growSpeed = 30f;
    public float maxLength = 12f;
    public float moveSpeed = 6f;
    public float lifeTime = 1.5f;

    private float currentLength = 0f;
    private bool fullyGrown = false;

    private Vector3 moveDir = Vector3.zero;

    private Tilemap tilemap;
    private int spawnX;
    private int centerY;

    public void Init(Tilemap tilemapRef, int spawnX)
    {
        this.tilemap = tilemapRef;
        this.spawnX = spawnX;

        BoundsInt bounds = tilemap.cellBounds;
        centerY=((bounds.yMin + bounds.yMax) / 2) - 1;

        // 처음 길이는 0
        transform.localScale = new Vector3(1f, 0f, 1f);

        // === 여기서 이동 방향(left/right)을 직접 계산 ===
        DecideMoveDirection();
    }

    private void DecideMoveDirection()
    {
        int leftTiles = 0;
        int rightTiles = 0;

        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < spawnX; x++)
            if (tilemap.HasTile(new Vector3Int(x, centerY, 0)))
                leftTiles++;

        for (int x = spawnX + 1; x < bounds.xMax; x++)
            if (tilemap.HasTile(new Vector3Int(x, centerY, 0)))
                rightTiles++;

        moveDir = (leftTiles > rightTiles) ? Vector3.left : Vector3.right;
    }

    private void Update()
    {
        if (!fullyGrown)
        {
            GrowLaser();
        }
        else
            MoveLaser();
    }

    private void GrowLaser()
    {
        currentLength += growSpeed * Time.deltaTime;

        float length = Mathf.Min(currentLength, maxLength);
        transform.localScale = new Vector3(1f, length, 1f);

        if (length >= maxLength)
        {
            fullyGrown = true;
            StartCoroutine(DestoryAfter(lifeTime));
        }
    }

    private void MoveLaser()
    {
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private IEnumerator DestoryAfter(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class Card : Collidable
{
    public override bool CauseDamage() => true;
    private Tilemap tilemap;


    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private float moveSpeed = 5f;
    private Vector3 moveDir = Vector3.right;

    public void SetDependencies(Tilemap tilemap)
    {
        this.tilemap = tilemap;
    }
    void Start()
    {

    }
    public void Init(Vector3 dir, float moveSpeed, float rotateSpeed, Tilemap tilemap)
    {
        this.moveDir = dir.normalized;
        this.moveSpeed = moveSpeed;
        this.rotateSpeed = rotateSpeed;
        this.tilemap = tilemap;
    }
    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();

        if (IsOutOfBounds(transform.position))
        {
            //Debug.Log("card ³ª°¨");
            Destroy(gameObject);
        }
    }


    private void Move()
    {
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void Rotate()
    {
        transform.Rotate(new Vector3(0, 0, -1) * rotateSpeed * Time.deltaTime);
    }

    private bool IsOutOfBounds(Vector3 worldPos)
    {
        if (!tilemap) return false;

        Vector3Int cell = tilemap.WorldToCell(worldPos);


        if (!tilemap.cellBounds.Contains(cell))
            return true;


        return !tilemap.HasTile(cell);
    }

    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Player : MonoBehaviour
{
    private SpriteRenderer sr;

    private Grid grid;
    private Tilemap tilemap;
    private Vector3Int currentPosition;
    public GameUI gameUI;
    [SerializeField] private float invincibleTime = 1.5f;
    private bool isInvincible = false;
    
    InputHandler inputHandler;
    private Vector3 moveDir;

    //[SerializeField] private Vector2Int minTileIndex = new Vector2Int(-8, -1);
    //[SerializeField] private Vector2Int maxTileIndex = new Vector2Int(-1, 2);
    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        inputHandler.OnMoveEvent += OnMove;
    }

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>(); // 자식에 있을 수도 있으니까
        Spawner spawner = FindObjectOfType<Spawner>();
        if (spawner != null)
            spawner.SetPlayer(this);
    }

    void OnMove(Vector2 moveDir)
    {
        // 가공
        this.moveDir = moveDir;
    }
    
    public void SetDependencies(Grid grid, Tilemap tilemap, GameUI gameUI, Vector3Int spawnIndex)
    {
        this.grid = grid;
        this.tilemap = tilemap;
        this.gameUI = gameUI;
        this.currentPosition = spawnIndex;
        this.transform.position = grid.GetCellCenterWorld(spawnIndex);
    }


    void Update()
    {
        // Vector2Int direction = Vector2Int.zero;
        //
        // if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        // {
        //     direction = Vector2Int.up;
        // }
        // else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        // {
        //     direction = Vector2Int.left;
        // }
        // else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        // {
        //     direction = Vector2Int.down;
        // }
        // else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        // {
        //     direction = Vector2Int.right;
        // }

        if (moveDir != Vector3.zero)
        {
            Move2(moveDir);
        }
    }

    public int GetX()
    {
        return currentPosition.x;
    }

    public int GetY()
    {
        return currentPosition.y;
    }
    private void Move(Vector2Int translation)
    {
        Vector3Int newPos = currentPosition + new Vector3Int(translation.x, translation.y, 0);

        if (Board.instance.IsValid(newPos))
        {
            currentPosition = newPos;
            transform.position = grid.GetCellCenterWorld(currentPosition);
        }
        else
        {
            Debug.Log("맵 경계 밖이라 이동 불가!");
        }

    }

    private void Move2(Vector3 translation)
    {
        transform.position += translation * (3 * Time.deltaTime); 
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInvincible) return; // 무적일 때 충돌 무시

        if(collision.TryGetComponent<Collidable>(out Collidable hit))
        {
            // card.~();

            if (hit.CauseDamage())
            {
                gameUI.ResetCombo();
                FloatingTextManager.ClearAll();
                Spawner spawner = FindObjectOfType<Spawner>();
                if (spawner != null) spawner.ResetDash();
                gameUI.ReduceHeart();  // 호출 성공
                StartCoroutine(Damage());
                StartCoroutine(Invincible());
            }

        }
    }

    IEnumerator Damage()
    {
        for (int i = 0; i < 3; i++)
        {
            sr.color = Color.gray;
            yield return new WaitForSeconds(0.2f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Invincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

}

using UnityEngine;

public enum TileState
{
    Normal,
    Warning,
    Danger
}


public class BoardTile : MonoBehaviour
{
    public Vector2Int GridPos { get; private set; }

    private SpriteRenderer spriteRenderer;
    private TileState currentState;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Initialize(Vector2Int gridPos)
    {
        GridPos = gridPos;
        SetState(TileState.Normal);
    }

    public void SetState(TileState state)
    {
        currentState = state;

        switch (state)
        {
            case TileState.Normal:
                spriteRenderer.color = Color.white;
                break;
            case TileState.Warning:
                spriteRenderer.color = Color.yellow;
                break;
            case TileState.Danger:
                spriteRenderer.color = Color.red;
                break;
        }
    }

    public TileState GetState() => currentState;
}

using System.Collections;
using UnityEngine;

public class PlayerGridMoveController : MonoBehaviour
{
    public float moveDuration = 0.1f;
    [SerializeField] Vector2Int startOffsetFromCenter;

    Player player;
    BoardManager board;
    InputHandler inputHandler;
    PlayerAnimationController animController;

    Vector2Int currentGridPos;
    bool isMoving;
    bool isInitialized;

    public Vector2Int CurrentGridPos => currentGridPos;

    public void Initialize(Player player)
    {
        if (isInitialized) return;

        this.player = player;
        board = BoardManager.Instance;
        inputHandler = player.InputHandler;
        animController = player.PlayerAnimationController;

        if (board == null) return;

        board.OnBoardResized += HandleBoardResized;

        if (inputHandler != null)
        {
            inputHandler.OnMoveEvent += HandleMoveInput;
            inputHandler.OnDashEvent += HandleDashInput;
        }

        Vector2Int center = new Vector2Int(board.width / 2, board.height / 2);
        currentGridPos = center + startOffsetFromCenter;
        ClampPosToBoard();
        transform.position = board.GetWorldPosition(currentGridPos);

        isInitialized = true;
    }

    void OnDestroy()
    {
        if (inputHandler != null)
        {
            inputHandler.OnMoveEvent -= HandleMoveInput;
            inputHandler.OnDashEvent -= HandleDashInput;
        }
        if (board != null)
            board.OnBoardResized -= HandleBoardResized;
    }

    void HandleBoardResized()
    {
        if (!isInitialized || board == null) return;
        ClampPosToBoard();
        transform.position = board.GetWorldPosition(currentGridPos);
    }

    void HandleMoveInput(Vector2 v)
    {
        if (!isInitialized || isMoving) return;

        Vector2Int dir = Vector2ToDir(v);
        if (dir == Vector2Int.zero) return;

        Vector2Int target = currentGridPos + dir;
        if (!board.IsInsideBoard(target)) return;

        animController?.StartMove(dir);
        StartCoroutine(MoveToCell(target));
    }

    void HandleDashInput()
    {
        // dash later
    }

    Vector2Int Vector2ToDir(Vector2 v)
    {
        int x = Mathf.RoundToInt(v.x);
        int y = Mathf.RoundToInt(v.y);

        if (Mathf.Abs(x) == 1 && y == 0) return new Vector2Int(x, 0);
        if (Mathf.Abs(y) == 1 && x == 0) return new Vector2Int(0, y);
        return Vector2Int.zero;
    }

    System.Collections.IEnumerator MoveToCell(Vector2Int target)
    {
        isMoving = true;

        Vector3 start = transform.position;
        Vector3 end = board.GetWorldPosition(target);

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;
        currentGridPos = target;

        isMoving = false;
        animController?.StopMove();

        ItemManager.Instance?.TryPickupItem(currentGridPos);
    }

    void ClampPosToBoard()
    {
        currentGridPos.x = Mathf.Clamp(currentGridPos.x, 0, board.width - 1);
        currentGridPos.y = Mathf.Clamp(currentGridPos.y, 0, board.height - 1);
    }
}
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BoardCameraController : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private float padding = 0.5f; // 여유 여백

    private BoardManager board;

    public void Initialize(BoardManager boardManager)
    {
        board = boardManager;

        if (targetCamera == null)
            targetCamera = GetComponent<Camera>();

        if (board == null || targetCamera == null)
        {
            Debug.LogError("BoardCameraController 초기화 실패");
            return;
        }

        // 보드 리사이즈 이벤트 구독
        board.OnBoardResized += HandleBoardResized;

        // 초기 보드에도 맞춰준다
        AdjustCamera();
    }

    private void OnDestroy()
    {
        if (board != null)
        {
            board.OnBoardResized -= HandleBoardResized;
        }
    }

    private void HandleBoardResized()
    {
        AdjustCamera();
    }

    private void AdjustCamera()
    {
        if (board == null || targetCamera == null)
            return;

        // 보드 실제 월드 크기
        float boardWidthWorld = (board.width - 1) * board.cellSize;
        float boardHeightWorld = (board.height - 1) * board.cellSize;

        float halfWidth = boardWidthWorld * 0.5f + padding;
        float halfHeight = boardHeightWorld * 0.5f + padding;

        float aspect = targetCamera.aspect;

        // 세로 기준 크기 vs 가로 기준 크기 중 더 큰 값 사용
        float sizeByHeight = halfHeight;
        float sizeByWidth = halfWidth / aspect;

        targetCamera.orthographicSize = Mathf.Max(sizeByHeight, sizeByWidth);

        // 카메라는 보드 중심이 (0,0) 기준이므로 x,y는 0에 두고 z만 유지
        Vector3 pos = targetCamera.transform.position;
        targetCamera.transform.position = new Vector3(0f, 0f, pos.z);
    }
}

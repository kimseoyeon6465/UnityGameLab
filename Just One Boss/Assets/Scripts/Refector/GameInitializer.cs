using System.Collections;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] BoardManager boardManager;
    [SerializeField] BossController bossController;
    [SerializeField] Player player;
    [SerializeField] BoardCameraController cameraController;

    [SerializeField] int boardWidth = 10;
    [SerializeField] int boardHeight = 6;

    IEnumerator Start()
    {
        boardManager.InitializeBoard(boardWidth, boardHeight);
        yield return null;

        bossController.InitializeBoss();
        yield return null;

        player.Initialize();
        yield return null;

        var warning = BoardWarningSystem.Instance;
        warning.Initialize(boardManager, player.PlayerGridMoveController);
        yield return null;

        if (cameraController != null)
            cameraController.Initialize(boardManager);

        // game state -> playing
    }
}

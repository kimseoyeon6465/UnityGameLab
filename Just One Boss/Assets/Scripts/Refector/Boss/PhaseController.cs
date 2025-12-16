using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseController : MonoBehaviour
{
    private BoardManager board;
    public Player player;

    private void Start()
    {
        board = BoardManager.Instance;
    }

    public void PhaseStart()
    {
        board.InitializeBoard(8, 5);
        //player.PlayerGridMoveController.OnBoardResized(); 
    }
}

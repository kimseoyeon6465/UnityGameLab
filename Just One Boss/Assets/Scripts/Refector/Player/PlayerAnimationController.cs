using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator animator;
    [SerializeField] Transform rotatePivot;

    static readonly int HashIsMoving = Animator.StringToHash("IsMoving");

    public void Initialize(Player player)
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void StartMove(Vector2 dir)
    {
        if (dir == Vector2.zero)
        {
            animator.SetBool(HashIsMoving, false);
            return;
        }

        animator.SetBool(HashIsMoving, true);
        Flip(dir);
    }

    public void StopMove()
    {
        animator.SetBool(HashIsMoving, false);
    }

    void Flip(Vector2 dir)
    {
        if (dir.x > 0)
            rotatePivot.localScale = new Vector3(1, 1, 1);
        else if (dir.x < 0)
            rotatePivot.localScale = new Vector3(-1, 1, 1);
    }
}

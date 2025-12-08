using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnoimtionController : MonoBehaviour
{
    InputHandler inputHandler;
    Animator animator;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        inputHandler = GetComponent<InputHandler>();
        inputHandler.OnMoveEvent += OnMove;
        
    }

    void OnMove(Vector2 moveVector)
    {
        Flip(moveVector);
    }

    void Flip(Vector2 moveVector)
    {
        if(moveVector.x > 0)
            spriteRenderer.flipX = true;
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    
    
}

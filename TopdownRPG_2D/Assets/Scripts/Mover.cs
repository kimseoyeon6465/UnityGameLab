using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;

    protected float xSpeed = 1.0f;
    protected float ySpeed = 0.75f;
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    
    protected virtual void UpdateMotor(Vector3 input)
    {
        //Reset MoveDelta
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        //Swap sprite dirsection, wether you're going right or left
        if (moveDelta.x > 0)
            transform.localScale = Vector3.one;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        //Make sure we can move int this direction, by casting a box there first, if the box returns null, we're free to move
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        //Raycast는 광선을 쏘고 Boxcast는 박스를 던짐
        //origin의 위치에서 size크기의 박스를 던지는데
        //angle만큼 회전시키고 direction방향으로 던짐
        //distance만큼의 거리를 던짐
        //layerMask는 충돌 가능한 대상의 레이어
        if (hit.collider == null)
        {
            //Make this thing move
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);

        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Make this thing move
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);

        }
    }
}

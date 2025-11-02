using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public ContactFilter2D filter;
    private BoxCollider2D boxCollider;
    private Collider2D[] hits = new Collider2D[10];

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        // ContactFilter2D를 명시적으로 초기화 (Unity 2021+에서는 기본값이 전부 감지라서 오류남)
        filter = new ContactFilter2D();
        filter.useTriggers = false;
        filter.SetLayerMask(LayerMask.GetMask("Actor"));//충돌감지할 레이어(플레이어는 Actor)

    }

    protected virtual void Update()
    {
        //Collision work
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;

            OnCollide(hits[i]);
            //The array is not cleaned up, so we do it ourself
            hits[i] = null;


        }

    }

    protected virtual void OnCollide(Collider2D coll)
    {
        Debug.Log(coll.name);//오브젝트.name은 해당 오브젝트의 이름 가져옴
    }
}


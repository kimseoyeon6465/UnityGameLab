using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativeTileHazard : Collidable
{
    // 플레이어와 위험 타일 충돌 처리용
    public override bool CauseDamage()
    {
        return true; // 위험 타일은 데미지 O
    }
}

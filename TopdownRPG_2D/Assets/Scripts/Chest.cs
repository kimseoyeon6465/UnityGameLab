using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    protected override void OnCollect()
    {
        if(!collected)
        {
            collected = true;
        }
    }
}

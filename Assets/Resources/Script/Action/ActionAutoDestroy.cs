using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAutoDestroy : Action
{
    public float destroyDelay;

    public override void Activate(HitResult hr)
    {
        if (hr.IsValid() == false)
            return;

        Wall hitWall = hr.target as Wall;
        if (hitWall == null)
            return;

        Destroy(gameObject, destroyDelay);
    }
}

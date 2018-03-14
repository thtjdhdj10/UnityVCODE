using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : DynamicUnit
{







    public override void OnDestroyShield()
    {
        CustomLog.CompleteLog("Player DestroyedShield HP");
    }

    public override void OnLoseHP()
    {
        CustomLog.CompleteLog("Player Lose HP");
    }
}

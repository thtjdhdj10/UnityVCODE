using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldComponent : UnitComponent
{
    public override void Init()
    {
        base.Init();

        remainChargeDelay = 0f;

        ShieldCount = 0;

        ShieldCount = maxShieldCount;
    }

    public int maxShieldCount;

    private int shieldCount;
    public int ShieldCount
    {
        get
        {
            return shieldCount;
        }
        set
        {
            if(value > shieldCount)
            {
                owner.OnGenerateShield();
            }
            else if(value < shieldCount)
            {
                owner.OnDestroyShield();
            }
            else
            {
                return;
            }

            shieldCount = VEasyCalculator.MinMax(shieldCount, 0, maxShieldCount);
        }
    }

    public float shieldChargeDelay;

    private float remainChargeDelay;
    private float RemainChargeDelay
    {
        get
        {
            return remainChargeDelay;
        }
        set
        {
            remainChargeDelay = value;

            if(remainChargeDelay < 0f)
            {
                ShieldCount += 1;

                remainChargeDelay = 0f;

                return;
            }

            remainChargeDelay = Mathf.Min(value, shieldChargeDelay);
        }
    }

    //

    private void FixedUpdate()
    {
        if (IsActivated() == false)
            return;

        if(ShieldCount < maxShieldCount)
        {
            RemainChargeDelay -= Time.fixedDeltaTime;
        }
    }

    public bool ShieldBlockProcess() // return is Blocked
    {
        if(ShieldCount <= 0)
        {
            return false;
        }
        else
        {
            ShieldCount -= 1;

            return true;
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldComponent : UnitComponent
{
    public override void Init()
    {
        base.Init();

        restTimeToShield = 0f;

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

    public float timeToShield;

    private float restTimeToShield;
    public float RestTimeToShield
    {
        get
        {
            return restTimeToShield;
        }
        set
        {
            restTimeToShield = VEasyCalculator.MinMax(value, 0f, timeToShield);
        }
    }

    //

    private void FixedUpdate()
    {
        if (IsActivated() == false)
            return;

        if(ShieldCount < maxShieldCount)
        {
            if(RestTimeToShield < timeToShield)
            {
                RestTimeToShield += Time.fixedDeltaTime;
            }
            else
            {
                ShieldCount += 1;

                RestTimeToShield = 0f;
            }
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileComponent : UnitComponent
{
    public Bullet bullet;

    public bool isEnableAutoFire = true;

    public int projectCount = 1;

    public float projectDirection = 0f;

    public float cooldown;
    private float remainCooldown;
    public float RemainCooldown
    {
        get
        {
            return remainCooldown;
        }
        set
        {
            remainCooldown = value;

            if(remainCooldown <= 0f)
            {
                if (IsActivated() == false)
                    return;

                remainCooldown = cooldown;

                Fire();
            }
        }
    }

    public enum TargetType
    {
        NONE,
        UNIT,
        LOCATION,
    }

    public TargetType targetType;

    [System.NonSerialized]
    public Vector2 targetPosition;

    public override void Init()
    {
        base.Init();

        RemainCooldown = cooldown;
    }

    private void FixedUpdate()
    {
        CooldownProcess();
    }

    private void CooldownProcess()
    {
        if (isEnableAutoFire == false)
            return;

        RemainCooldown -= Time.fixedDeltaTime;
    }

    public void Fire()
    {
        if (projectCount == 1)
        {
            FireEach(new Vector2(0, 0), 0f);
        }
        else
        {
            for (int i = 0; i < projectCount; ++i)
            {
                float relativeDir = projectDirection / (float)(projectCount - 1) * i - (float)projectDirection * 0.5f;

                FireEach(new Vector2(0, 0), relativeDir);
            }
        }


    }

    private void FireEach(Vector2 relativePosition, float relativeDirection)
    {
        Bullet _bullet = Instantiate(bullet);

        if (_bullet == null)
        {
            Debug.LogWarning("Invalid Bullet Type");
        }

        switch (targetType)
        {
            case TargetType.UNIT:
                {
                    _bullet.defaultTarget = owner.defaultTarget;
                }
                break;
            case TargetType.LOCATION:
                {

                }
                break;
            case TargetType.NONE:
                {

                }
                break;
        }

        _bullet.Init(this, relativePosition, relativeDirection);
    }
}

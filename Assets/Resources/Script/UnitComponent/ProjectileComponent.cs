using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileComponent : UnitComponent
{
    public Bullet bullet;

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

            if(remainCooldown < 0f)
            {
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
        if (IsActivated() == false)
            return;

        if (RemainCooldown > 0f)
        {
            RemainCooldown -= Time.fixedDeltaTime;
        }
    }

    public bool Fire()
    {
        Bullet _bullet = Instantiate(bullet);

        if (_bullet == null)
        {
            Debug.LogWarning("Invalid Bullet Type");
            return false;
        }

        switch(targetType)
        {
            case TargetType.UNIT:
                {
                    _bullet.defaultTarget = owner.defaultTarget;
                    _bullet.Init(this);
                }
                break;
        }

        return true;
    }
}

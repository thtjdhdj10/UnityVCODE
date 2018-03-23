using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class CollisionComponent : UnitComponent
{
    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (IsActivated() == false)
            return;

        CollisionComponent target = collider.GetComponent<CollisionComponent>();
        if (target == null)
            return;

        //if (target.gameObject.activeInHierarchy == false)
        //    return;

        //if (target.owner.unitActive == false)
        //    return;

        Action.Activate(owner, new Action.HitResult(owner, target.owner));

        DamageProcess(target);
    }

    public void DamageProcess(CollisionComponent col)
    {
        if (isHittable == true &&
            col.isBeHittable == true)
        {
            if (IsImmune() == true)
                return;

            if (Unit.IsChildTypeOf(targetType, col.owner.unitType))
            {
                col.BeHit(this);
            }
        }
    }

    //

    public void BeHit(CollisionComponent target)
    {
        if (IsActivated() == false)
            return;

        if (GetComponent<ShieldComponent>() != null)
        {
            if (GetComponent<ShieldComponent>().ShieldBlockProcess() == false)
            {
                CurrentHP -= target.damage;
            }
        }
        else
        {
            CurrentHP -= target.damage;
        }
    }

    //

    public override void Init()
    {
        currentHP = maxHP;
    }

    public int damage;

    public int maxHP = 1;

    private int currentHP;
    public int CurrentHP
    {
        get
        {
            return currentHP;
        }
        set
        {
            if(isBeHittable == false)
            {
                return;
            }

            if(value < currentHP)
            {
                if (enableStaticDamage == true)
                {
                    value = Mathf.Max(value, currentHP - staticDamage);
                }

                owner.OnLoseHP();
            }
            else if(value > currentHP)
            {
                owner.OnHealHP();
            }
            else
            {
                return;
            }

            if(value <= 0)
            {
                owner.OnDeath();
            }

            currentHP = VEasyCalculator.MinMax(currentHP, 0, maxHP);
        }
    }

    //

    public enum DamageEffectType
    {
        NONE = 0,
        SPARK_TO_REVERSE,
        SPARK_TO_SIDE,
    }

    public bool isHittable;

    public Unit.UnitType targetType; // TODO editor 로, is hittable 이 true일 때만 노출

    public DamageEffectType damageEffectType;

    public bool enableStaticDamage;

    public int staticDamage;

    //

    public bool isBeHittable;

    public Dictionary<ImmunityType, bool> isImmunityDic = new Dictionary<ImmunityType, bool>();
    public enum ImmunityType
    {
        BE_HIT, // 플레이어는 공격받으면 잠시 무적됨
        PATTERN, // 특정 유닛의 패턴에 의한 무적
    }

    public bool IsImmune()
    {
        foreach (bool immune in isImmunityDic.Values)
        {
            if (immune == true)
                return true;
        }

        return false;
    }

}

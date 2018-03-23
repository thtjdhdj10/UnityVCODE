using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : DynamicUnit
{
//    public bool useChromaHP = true;

//    private void OnChangeHP()
//    {
//        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
//        CollisionComponent collisionComponent = GetComponent<CollisionComponent>();

//        float hpRemainRatio = (float)collisionComponent.maxHP / (float)collisionComponent.CurrentHP;

//        spriteRenderer.color = new Color(hpRemainRatio, hpRemainRatio * 0.06f, hpRemainRatio * 0.03f, 1f);

////        spriteRenderer.
//    }

//    public override void Init()
//    {
//        base.Init();

//        OnChangeHP();
//    }

//    public override void OnLoseHP()
//    {
//        base.OnLoseHP();

//        OnChangeHP();
//    }

//    public override void OnHealHP()
//    {
//        base.OnHealHP();

//        OnChangeHP();
//    }

    public override bool SearchTarget()
    {
        target = FindObjectOfType<PlayerUnit>();

        return target != null;
    }

}

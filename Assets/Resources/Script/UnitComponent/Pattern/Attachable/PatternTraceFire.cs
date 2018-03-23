using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternTraceFire : PatternAttachable {

    public GameObject target;
    public Bullet bulletPrefab;

    public int count;
    public float delay;
    
    public enum TraceType
    {
        STATIC,
//        ESTIMATE_BY_DIRECTION, // 예측탄
//        ESTIMATE_BY_POSITION,
    }

    public TraceType type;

    protected override IEnumerator PatternFramework()
    {
        isPatternRunning = true;

        for(int i = 0; i < count; ++i)
        {
            if(blockBulletFire ==false)
            {
                direction = VEasyCalculator.GetDirection(position, target.transform.position);

                Bullet bulletInstance = Instantiate(bulletPrefab);

                bulletInstance.transform.position = position;

                MovementComponent move = bulletInstance.GetComponent<MovementComponent>();

                if (move == null)
                    Destroy(bulletInstance);

                switch(type)
                {
                    case TraceType.STATIC:
                        {
                            move.Direction = direction;
                        }
                        break;
                }

                if (delay > 0f)
                    yield return new WaitForSeconds(delay);
            }
        }

        blockBulletFire = false;
        isPatternRunning = false;
    }
}

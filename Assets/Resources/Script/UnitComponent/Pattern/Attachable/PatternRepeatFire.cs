using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternRepeatFire : PatternAttachable
{
    public Bullet bulletPrefab;

    public int count;
    public float delay;

    protected override IEnumerator OnStartPattern()
    {
        isPatternRunning = true;

        for(int i = 0; i < count; ++i)
        {
            if(blockBulletFire == false)
            {
                Bullet bulletInstance = Instantiate(bulletPrefab);

                bulletInstance.transform.position = position;

                MovementComponent move = bulletInstance.GetComponent<MovementComponent>();

                if(move == null)
                {
                    Destroy(bulletInstance);
                }

                move.Direction = direction;
            }

            if (delay > 0f)
                yield return new WaitForSeconds(delay);
        }

        isPatternRunning = false;
    }
}
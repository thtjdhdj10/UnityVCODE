using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternParallelFire : PatternAttachable
{
    public Bullet bulletPrefab;

    public float width = 0.5f;
    public int count = 5;
    public float delay = 0.1f;
    public float distance = 0.1f;

    protected override IEnumerator PatternFramework()
    {
        isPatternRunning = true;

        for(int i = 0; i < count; ++i)
        {
            if(blockBulletFire == false)
            {
                float deltaDistance = distance * i / (count - 1) - width * 0.5f;

                Vector2 deltaPosition = VEasyCalculator.GetRotatedPosition(direction - 90f, deltaDistance);

                Vector2 targetPosition = position + deltaPosition;

                Bullet bulletInstance = Instantiate(bulletPrefab);

                bulletInstance.transform.position = targetPosition;

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

        blockBulletFire = false;
        isPatternRunning = false;
    }
}

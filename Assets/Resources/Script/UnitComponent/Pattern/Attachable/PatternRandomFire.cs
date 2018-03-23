using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternRandomFire : PatternAttachable
{
    public Bullet bulletPrefab;
    public List<Bullet> spawnedBulletList = new List<Bullet>();

    public int count;
    public float delay;
    public float fireAngle;

    protected override IEnumerator PatternFramework()
    {
        isPatternRunning = true;

        for(int i = 0; i < count; ++i)
        {
            if(blockBulletFire == false)
            {
                float min = direction - fireAngle * 0.5f;
                float max = direction + fireAngle * 0.5f;

                float targetDirection = UnityEngine.Random.Range(min, max);

                Bullet bulletInstance = Instantiate(bulletPrefab);

                MovementComponent move = bulletInstance.GetComponent<MovementComponent>();
                if (move == null)
                    Destroy(bulletInstance);

                bulletInstance.owner = owner;

                spawnedBulletList.Add(bulletInstance);

                bulletInstance.transform.position = position;
                move.Direction = direction;
            }

            if (delay > 0f)
                yield return new WaitForSeconds(delay);
        }

        blockBulletFire = false;
        isPatternRunning = false;
    }
}
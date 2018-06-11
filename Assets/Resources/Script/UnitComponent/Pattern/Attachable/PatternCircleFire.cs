using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternCircleFire : PatternAttachable
{
    public Bullet bulletPrefab;
    public List<Bullet> spawnedBulletList = new List<Bullet>();

    public int count;
    public float fireAngle;
    public float delay;

    public void Init(Bullet bullet, GameObject posRoot, GameObject dirRoot, bool attachPos, bool attachDir,
        int _count, float _fireAngle, float _delay)
    {
        bulletPrefab = bullet;

        positionRoot = posRoot;
        directionRoot = dirRoot;
        attachPositionToRoot = attachPos;
        attachDirectionToRoot = attachDir;

        count = _count;
        fireAngle = _fireAngle;
        delay = _delay;
    }

    protected override IEnumerator PatternFramework()
    {
        isPatternRunning = true;

        for(int i = 0; i < count; ++i)
        {
            if(blockBulletFire == false)
            {
                float targetDirection = direction - fireAngle * 0.5f + fireAngle * i / (count - 1);

                Bullet bulletInstance = MonoBehaviour.Instantiate(bulletPrefab);

                MovementComponent move = bulletInstance.GetComponent<MovementComponent>();
                if (move == null)
                    MonoBehaviour.Destroy(bulletInstance);

                bulletInstance.owner = owner;

                spawnedBulletList.Add(bulletInstance);

                bulletInstance.transform.position = position;
                move.Direction = targetDirection;
            }

            if (delay > 0f)
                yield return new WaitForSeconds(delay);
        }

        blockBulletFire = false;
        isPatternRunning = false;
    }

}
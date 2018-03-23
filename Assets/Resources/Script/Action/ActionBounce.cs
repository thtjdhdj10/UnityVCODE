using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBounce : Action
{
    public enum BounceProcessType
    {
        NONE,
        TO_TARGET, // target 을 향하게 direction 갱신
        TO_TARGET_REVERSE, // target 방향의 반대로 향하게
        REVERSE, // 방향 180도 전환
        REFLECT, // 거울반사
        DESTROY, // 벽충돌 시 1초후 destroy
        BLOCK, // 길막만 함
    }

    public BounceProcessType bounceProcessType;

    public Unit.UnitType targetType;

    public float destroyDelay = 1f;

    public override void Activate(HitResult hr)
    {
        if (hr.IsValid() == false)
            return;

        BounceProcess(hr, bounceProcessType);
    }

    private void BounceProcess(HitResult hr, BounceProcessType _type)
    {
        Unit owner = GetComponent<Unit>();

        MovementComponent ownerMovable = hr.owner.GetComponent<MovementComponent>();

        if (ownerMovable == null)
            return;

        switch (_type)
        {
            case BounceProcessType.REFLECT:
                {
                    Wall hitWall = hr.target as Wall;
                    if (hitWall == null)
                        return;

                    if (hitWall.direction == WorldGeneral.Direction.DOWN)
                    {
                        if (ownerMovable.Direction >= 90f &&
                            ownerMovable.Direction < 270f)
                        {
                            ownerMovable.Direction = 180f - ownerMovable.Direction;
                        }
                    }
                    else if (hitWall.direction == WorldGeneral.Direction.LEFT)
                    {
                        if (ownerMovable.Direction < 180f)
                        {
                            ownerMovable.Direction = 360f - ownerMovable.Direction;
                        }
                    }
                    else if (hitWall.direction == WorldGeneral.Direction.RIGHT)
                    {
                        if (ownerMovable.Direction >= 180f)
                        {
                            ownerMovable.Direction = 360f - ownerMovable.Direction;
                        }
                    }
                    else if (hitWall.direction == WorldGeneral.Direction.UP)
                    {
                        if (ownerMovable.Direction < 90f ||
                            ownerMovable.Direction >= 270f)
                        {
                            ownerMovable.Direction = 180f - ownerMovable.Direction;
                        }
                    }
                }
                break;
            case BounceProcessType.DESTROY:
                {
                    Wall hitWall = hr.target as Wall;
                    if (hitWall == null)
                        return;

                    StartCoroutine(DelayedDestroy(destroyDelay));
                }
                break;
            case BounceProcessType.REVERSE:
                {
                    ownerMovable.Direction += 180f;
                }
                break;
            case BounceProcessType.TO_TARGET:
                {
                    if (owner.target == null)
                    {
                        BounceProcess(hr, BounceProcessType.REVERSE);
                        return;
                    }

                    float dirToTarget = VEasyCalculator.GetDirection(ownerMovable.owner, ownerMovable.owner.target);
                    ownerMovable.Direction = dirToTarget;
                }
                break;
            case BounceProcessType.TO_TARGET_REVERSE:
                {
                    if (owner.target == null)
                    {
                        BounceProcess(hr, BounceProcessType.REVERSE);
                        return;
                    }

                    float dirToTarget = VEasyCalculator.GetDirection(ownerMovable.owner, ownerMovable.owner.target);
                    ownerMovable.Direction = dirToTarget + 180f;
                }
                break;
            case BounceProcessType.BLOCK:
                {
                    // TODO: position 만 충돌 반대 방향으로 밈
                }
                break;
        }
    }

    IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBounce : Action
{
    public string targetTypeName;

    public enum Type
    {
        NONE,
        TO_TARGET, // target 을 향하게 direction 갱신
        TO_TARGET_REVERSE, // target 방향의 반대로 향하게
        REVERSE, // 방향 180도 전환
        WALL_REFLECT, // 거울반사
        WALL_DESTROY, // 벽충돌 시 1초후 destroy
    }

    public Type type;

    public float destroyDelay = 1f;

    public override void Activate(HitResult hr)
    {
        if (hr.IsValid() == false)
            return;

        BounceProcess(hr, type);
    }

    private void BounceProcess(HitResult hr, Type _type)
    {
        Unit owner = GetComponent<Unit>();

        System.Type bounceTargetType =
            System.Type.GetType(hr.owner.defaultTargetTypeName); // Bounce 처리할 대상의 타입

        if (bounceTargetType == null)
            bounceTargetType = System.Type.GetType(targetTypeName);

        if (bounceTargetType == null)
            return;

        if(bounceTargetType.IsSubclassOf(hr.target.GetType()) == false)
            return;

        MovementComponent ownerMovable = hr.owner.GetComponent<MovementComponent>();

        if (ownerMovable == null)
            return;

        switch (_type)
        {
            case Type.WALL_REFLECT:
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
            case Type.WALL_DESTROY:
                {
                    Wall hitWall = hr.target as Wall;
                    if (hitWall == null)
                        return;

                    StartCoroutine(DelayedDestroy(destroyDelay));
                }
                break;
            case Type.REVERSE:
                {
                    ownerMovable.Direction += 180f;
                }
                break;
            case Type.TO_TARGET:
                {
                    if (owner.defaultTarget == null)
                    {
                        BounceProcess(hr, Type.REVERSE);
                        return;
                    }

                    float dirToTarget = VEasyCalculator.GetDirection(ownerMovable.owner, ownerMovable.owner.defaultTarget);
                    ownerMovable.Direction = dirToTarget;
                }
                break;
            case Type.TO_TARGET_REVERSE:
                {
                    if (owner.defaultTarget == null)
                    {
                        BounceProcess(hr, Type.REVERSE);
                        return;
                    }

                    float dirToTarget = VEasyCalculator.GetDirection(ownerMovable.owner, ownerMovable.owner.defaultTarget);
                    ownerMovable.Direction = dirToTarget + 180f;
                }
                break;
            //case Type.WALL_BLOCK:
            //    {
            //        Wall hitWall = hr.target as Wall;
            //        if (hitWall == null)
            //            return;

            //        float wallScale = VEasyCalculator.SelectFloat(
            //            (hitWall.direction == WorldGeneral.Direction.UP ||
            //            hitWall.direction == WorldGeneral.Direction.DOWN),

            //            wallScale = hitWall.GetComponent<BoxCollider2D>().size.y,
            //            wallScale = hitWall.GetComponent<BoxCollider2D>().size.x);

            //        float ownerScale = 0f;

            //        CircleCollider2D ownerCircleCollider = owner.GetComponent<CircleCollider2D>();
            //        if (ownerCircleCollider != null)
            //        {
            //            ownerScale = ownerCircleCollider.radius;
            //        }
            //        else
            //        {
            //            BoxCollider2D ownerBoxCollider = owner.GetComponent<BoxCollider2D>();
            //            if (ownerBoxCollider != null)
            //            {

            //                ownerScale = VEasyCalculator.SelectFloat(
            //                    (hitWall.direction == WorldGeneral.Direction.UP ||
            //                    hitWall.direction == WorldGeneral.Direction.DOWN),

            //                    ownerScale = ownerBoxCollider.size.y,
            //                    ownerScale = ownerBoxCollider.size.x);
            //            }
            //        }

            //        Vector3 targetPosition = hr.owner.transform.position;

            //        switch (hitWall.direction)
            //        {
            //            case WorldGeneral.Direction.UP:
            //                targetPosition.y = hitWall.transform.position.y - (wallScale - ownerScale);
            //                break;
            //            case WorldGeneral.Direction.DOWN:
            //                targetPosition.y = hitWall.transform.position.y + (wallScale - ownerScale);
            //                break;
            //            case WorldGeneral.Direction.LEFT:
            //                targetPosition.x = hitWall.transform.position.x + (wallScale - ownerScale);
            //                break;
            //            case WorldGeneral.Direction.RIGHT:
            //                targetPosition.x = hitWall.transform.position.x - (wallScale - ownerScale);
            //                break;
            //        }

            //        hr.owner.transform.position = targetPosition;
            //    }
            //    break;
        }
    }

    IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
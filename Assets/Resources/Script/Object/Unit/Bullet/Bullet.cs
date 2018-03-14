using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Unit
{
    [System.NonSerialized]
    public Unit owner;

    public virtual void Init(ProjectileComponent projector)
    {
        owner = projector.owner;
        MovementComponent ownerMovement = owner.GetComponent<MovementComponent>();
        MovementComponent movement = GetComponent<MovementComponent>();

        defaultTarget = owner.defaultTarget;

        switch (projector.targetType)
        {
            case ProjectileComponent.TargetType.LOCATION:
                {
                    movement.Direction = VEasyCalculator.GetDirection(owner.transform.position, projector.targetPosition);
                }
                break;
            case ProjectileComponent.TargetType.UNIT:
            case ProjectileComponent.TargetType.NONE:
                {

                }
                break;
        }

        movement.Direction = ownerMovement.Direction;

        gameObject.transform.position = owner.transform.position;
    }

}

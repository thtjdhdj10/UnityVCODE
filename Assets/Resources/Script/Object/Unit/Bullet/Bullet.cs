using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Unit
{
    [System.NonSerialized]
    public Unit owner;

    public virtual void Init(ProjectileComponent projector, Vector2 relativePosition, float relativeDirection)
    {
        owner = projector.owner;
        MovementComponent ownerMovement = owner.GetComponent<MovementComponent>();
        MovementComponent movement = GetComponent<MovementComponent>();

        defaultTarget = owner.defaultTarget;

        switch (projector.targetType)
        {
            case ProjectileComponent.TargetType.LOCATION:
                {
                    movement.Direction = VEasyCalculator.GetDirection(owner.transform.position, projector.targetPosition) + relativeDirection;
                }
                break;
            case ProjectileComponent.TargetType.UNIT:
                {

                }
                break;
            case ProjectileComponent.TargetType.NONE:
                {
                    movement.Direction = ownerMovement.Direction + relativeDirection;
                }
                break;
        }

        gameObject.transform.position = owner.transform.position + new Vector3(relativePosition.x, relativePosition.y, 0);
    }

}

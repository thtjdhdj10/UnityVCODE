using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : DynamicUnit
{
    public MovementComponent hoverMovementComponent;

    public Unit owner;

    protected override void Awake()
    {
        if(hoverMovementComponent == null)
        {
            hoverMovementComponent = gameObject.AddComponent<MovementComponent>();

            hoverMovementComponent.movementType = MovementComponent.MovementType.TURN_LERP_BY_DISTANCE;

            hoverMovementComponent.turnFactor = 1f;
            hoverMovementComponent.disFactor = 1f;
            hoverMovementComponent.maxTurnByDis = 2f;
            hoverMovementComponent.minTurnByDis = 0.5f;
        }
    }

    public void Init(Unit _owner)
    {
        owner = _owner;
    }

    public void SetHover(bool enableHover)
    {
//        GetComponent<MovementComponent>().
    }
}

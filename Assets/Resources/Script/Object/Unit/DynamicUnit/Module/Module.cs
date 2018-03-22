using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : DynamicUnit
{
    public MovementComponent hoverMovementComponent;

    public Unit moduleOwner;

    private bool enableHoverMovement = true;
    public bool EnableHoverMovement
    {
        get
        {
            return enableHoverMovement;
        }
        set
        {
            enableHoverMovement = value;

            hoverMovementComponent.activatedDic[MovementComponent.ActivatingType.DEFAULT_TOGGLE] = value;
        }
    }

    private void Awake()
    {
        if(hoverMovementComponent == null)
        {
            hoverMovementComponent = gameObject.AddComponent<MovementComponent>();

            hoverMovementComponent.movementType = MovementComponent.MovementType.TURN_LERP_BY_DISTANCE;

            hoverMovementComponent.turnFactor = 1f;
            hoverMovementComponent.disFactor = 1f;
            hoverMovementComponent.maxTurnByDis = 1f;
            hoverMovementComponent.minTurnByDis = 0f;
        }
    }

    public void Init(Unit _moduleOwner)
    {
        moduleOwner = _moduleOwner;
    }



}

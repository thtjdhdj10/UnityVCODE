using UnityEngine;
using System.Collections.Generic;

// 이 컴포넌트는 Unit 만 운용 가능.

public class MovementComponent : UnitComponent
{
    public enum MovementType
    {
        NONE,
        VECTOR, // 상하좌우로만 이동
        STRAIGHT, // 방향을 바꾸지 않음
        TURN_LERP, // target 과의 방향 비례 선회
        TURN_LERP_BY_DISTANCE, // target 과의 방향과 거리 비례 선회
        TURN_REGULAR, // 균일한 선회
        TURN_REGULAR_DISTANCE, // target 과의 거리 비례 선회
    }

    public MovementType movementType;

    public float originSpeed;

    [SerializeField]
    private float speed;
    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            if (value > 0f)
                speed = value;
            else
                speed = 0f;
        }
    }

    public enum SpeedModulateType
    {
        BY_TARGET_LOST,
        BY_DEBUFF,
    }

    // https://answers.unity.com/questions/642431/dictionary-in-inspector.html
    public Dictionary<SpeedModulateType, float> speedModulateDic = new Dictionary<SpeedModulateType, float>();

    public float targetLostSpeed = 0.5f;

    [SerializeField]
    private float direction;
    public float Direction
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value;
            VEasyCalculator.NormalizedDirection(ref direction);
            OnUpdateDirection();
        }
    }

    // Lerp 에선 target 과의 각도 차이의 몇 배 만큼 회전할 지의 값( 초당 )
    // Regular 에선 target 방향으로 몇 도 회전할 지의 값( 초당 )
    public float turnFactor;

    // Lerp 의 보조로 사용. 초당 최소 회전 제한
    public float minTurnFactor;

    // target 과의 거리 차이가 max에 이상일 때, target 과의 각도 차이의 몇 배 만큼 '더' 회전할 지의 값( 초당 )
    public float disFactor;

    public float maxTurnByDis; // 거리에 의한 turn 값이 최대가 되는 거리

    public float minTurnByDis; // 거리에 의한 turn 값이 최소가 되는 거리

    [System.NonSerialized]
    public bool[] moveDir = new bool[4]; // for vector movement

    // advanced

    public bool initRotateToTarget = true;
    
    public bool sprAngleToDir = true;

    // TODO: dodge 구현

    public bool enableDodge;

    public float dodgeDelay; // dodge 가 enable 일 때만 편집할 수 있도록

    public float dodgeRemainDelay;

    // TODO: 겹쳐있는 동안, 겹친 정도에 따라, 겹치지 않는 방향으로 아주 약간씩 이동시키는 로직 추가
    // 그 로직을 Bounce Action - Block 에서 사용하도록 할 것임
    // 아니 그냥 거기에 구현하는게 맞나?

    public override void Init()
    {
        base.Init();

        if(owner == null)
        {
            owner = GetComponent<Unit>();
        }

        if(initRotateToTarget == true)
        {
            UpdateTarget();

            if (componentTarget != null &&
                owner != null)
            {
                Direction = VEasyCalculator.GetDirection(owner, componentTarget);
            }
        }
    }

    public void UpdateTarget()
    {
        if(useDefaultTarget == true &&
            owner != null)
        {
            componentTarget = owner.defaultTarget;
        }
    }

    public void OnUpdateDirection()
    {
        if (sprAngleToDir == true)
            SetSpriteAngle();
    }

    //

    private void FixedUpdate()
    {
        if (IsActivated() == false)
            return;

        MovementProcess(movementType);
    }

    private void MovementProcess(MovementType _movementType)
    {
        Speed = originSpeed;

        foreach (float speedModulateFactor in speedModulateDic.Values)
        {
            Speed = Speed * speedModulateFactor;
        }

        UpdateTarget();

        switch (_movementType)
        {
            case MovementType.STRAIGHT:
                {
                    float moveDis = speed * Time.fixedDeltaTime;

                    Vector2 moveVector = VEasyCalculator.GetRotatedPosition(direction, moveDis);

                    Vector2 v2Pos = owner.transform.position;
                    owner.transform.position = v2Pos + moveVector;
                }
                break;
            case MovementType.TURN_LERP:
                {
                    if (componentTarget == null)
                    {
                        MovementProcess(MovementType.STRAIGHT);

                        speedModulateDic[SpeedModulateType.BY_TARGET_LOST] = targetLostSpeed;
                        return;
                    }

                    speedModulateDic[SpeedModulateType.BY_TARGET_LOST] = 1f;

                    float moveDis = speed * Time.fixedDeltaTime;

                    float dirToTarget = VEasyCalculator.GetDirection(owner, componentTarget);

                    float deltaDir = VEasyCalculator.GetLerpDirection(
                        direction, dirToTarget, turnFactor * Time.fixedDeltaTime) - direction;

                    if (Mathf.Abs(deltaDir) < minTurnFactor * Time.fixedDeltaTime)
                        Direction += Mathf.Sign(deltaDir) * minTurnFactor * Time.fixedDeltaTime;
                    else
                        Direction += deltaDir;

                    Vector2 moveVector = VEasyCalculator.GetRotatedPosition(direction, moveDis);

                    Vector2 v2Pos = owner.transform.position;
                    owner.transform.position = v2Pos + moveVector;
                }
                break;
            case MovementType.TURN_LERP_BY_DISTANCE:
                {
                    if (componentTarget == null)
                    {
                        MovementProcess(MovementType.STRAIGHT);

                        speedModulateDic[SpeedModulateType.BY_TARGET_LOST] = targetLostSpeed;
                        return;
                    }

                    speedModulateDic[SpeedModulateType.BY_TARGET_LOST] = 1f;

                    float moveDis = speed * Time.fixedDeltaTime;

                    float dirToPlayer = VEasyCalculator.GetDirection(owner, componentTarget);

                    float disToPlayer = VEasyCalculator.GetDistance(componentTarget, owner);

                    float deltaDir = VEasyCalculator.GetLerpDirection(
                        direction, dirToPlayer, turnFactor * Time.fixedDeltaTime,
                        maxTurnByDis, minTurnByDis, disToPlayer, disFactor * Time.fixedDeltaTime) - direction;

                    if (Mathf.Abs(deltaDir) < minTurnFactor * Time.fixedDeltaTime)
                        Direction += Mathf.Sign(deltaDir) * minTurnFactor * Time.fixedDeltaTime;
                    else
                        Direction += deltaDir;

                    Vector2 moveVector = VEasyCalculator.GetRotatedPosition(direction, moveDis);

                    Vector2 v2Pos = owner.transform.position;
                    owner.transform.position = v2Pos + moveVector;
                }
                break;
            case MovementType.TURN_REGULAR:
                {
                    if (componentTarget == null)
                    {
                        MovementProcess(MovementType.STRAIGHT);

                        speedModulateDic[SpeedModulateType.BY_TARGET_LOST] = targetLostSpeed;
                        return;
                    }

                    speedModulateDic[SpeedModulateType.BY_TARGET_LOST] = 1f;

                    float moveDis = speed * Time.fixedDeltaTime;

                    float dirToTarget = VEasyCalculator.GetDirection(owner, componentTarget);

                    Direction = VEasyCalculator.GetTurningDirection(
                        direction, dirToTarget, turnFactor * Time.fixedDeltaTime);

                    Vector2 moveVector = VEasyCalculator.GetRotatedPosition(direction, moveDis);

                    Vector2 v2Pos = owner.transform.position;
                    owner.transform.position = v2Pos + moveVector;
                }
                break;
            case MovementType.TURN_REGULAR_DISTANCE:
                {
                    if (componentTarget == null)
                    {
                        MovementProcess(MovementType.STRAIGHT);

                        speedModulateDic[SpeedModulateType.BY_TARGET_LOST] = targetLostSpeed;
                        return;
                    }

                    speedModulateDic[SpeedModulateType.BY_TARGET_LOST] = 1f;

                    float moveDis = speed * Time.fixedDeltaTime;

                    float dirToPlayer = VEasyCalculator.GetDirection(owner, componentTarget);

                    float disToPlayer = VEasyCalculator.GetDistance(componentTarget, owner);

                    Direction = VEasyCalculator.GetTurningDirection(
                        direction, dirToPlayer, turnFactor * Time.fixedDeltaTime,
                        maxTurnByDis, minTurnByDis, disToPlayer, disFactor * Time.fixedDeltaTime);

                    Vector2 moveVector = VEasyCalculator.GetRotatedPosition(direction, moveDis);

                    Vector2 v2Pos = owner.transform.position;
                    owner.transform.position = v2Pos + moveVector;
                }
                break;
            case MovementType.VECTOR:
                {
                    int dirCount = 0;

                    for (int d = 0; d < 4; ++d)
                    {
                        if (moveDir[d] == true)
                        {
                            dirCount++;
                        }
                    }

                    float moveDis = speed * Time.fixedDeltaTime;

                    float reduceByMultiDirection = 0.7071f; // 1/root(2)
                    if (dirCount >= 2)
                    {
                        moveDis = speed * Time.deltaTime * reduceByMultiDirection;
                    }

                    Vector2 delta = new Vector2(0f, 0f);
                    if (moveDir[(int)WorldGeneral.Direction.LEFT] == true)
                    {
                        delta.x -= moveDis;
                    }
                    else if (moveDir[(int)WorldGeneral.Direction.RIGHT] == true)
                    {
                        delta.x += moveDis;
                    }
                    if (moveDir[(int)WorldGeneral.Direction.UP] == true)
                    {
                        delta.y += moveDis;
                    }
                    else if (moveDir[(int)WorldGeneral.Direction.DOWN] == true)
                    {
                        delta.y -= moveDis;
                    }

                    Vector2 v2Pos = owner.transform.position;
                    owner.transform.position = v2Pos + delta;
                }
                break;
        }
    }

    private void SetSpriteAngle()
    {
        Vector3 rot = transform.eulerAngles;
        rot.z = direction + SpriteManager.spriteDefaultRotation;
        transform.eulerAngles = rot;
    }
}

//

// TODO: 미구현
//protected virtual void DodgeMove()
//{
//    List<Unit> bulletList = new List<Unit>();

//    // PLAYER 의 모든 총알을 대상으로
//    for (int i = 0; i < Unit.unitList.Count; ++i)
//    {
//        Unit unit = Unit.unitList[i];

//        if (unit.force == Unit.Force.PLAYER &&
//            unit.Hittable != null &&
//            unit.isActiveAndEnabled == true)
//        {
//            bulletList.Add(unit);
//        }
//    }

//    //        Vector2 originalPosition = owner.transform.position;

//    // dodgeDirection 개의 방향으로 dodgeFrame * speed 만큼 이동했을 때
//    List<Vector2> movablePos = new List<Vector2>();
//    for (int i = 0; i < dodgeDirection + 1; ++i)
//    {
//        Vector2 checkPosition = new Vector2();

//        bool collision = false;

//        float checkDistance = 0f;

//        for (int j = 0; j < bulletList.Count; ++j)
//        {
//            if (i < dodgeDirection)
//            {
//                direction = 360f / (float)dodgeDirection * (float)i;
//                checkDistance = speed * Time.fixedDeltaTime * (float)dodgeFrame;

//                collision = VEasyCalculator.FutureIntersectCheck(owner, bulletList[j], dodgeFrame);
//            }
//            else
//            {
//                direction = 0f;
//                checkDistance = 0f;

//                collision = VEasyCalculator.IntersectCheck(owner, bulletList[j]);
//            }
//        }

//        if (collision == false)
//        {
//            checkPosition = owner.transform.position;
//            checkPosition += VEasyCalculator.GetRotatedPosition(direction, checkDistance);

//            movablePos.Add(checkPosition);
//        }
//    }

//    Debug.Log(movablePos.Count);

//    if (movablePos.Count > 0)
//    {
//        owner.transform.position = movablePos[movablePos.Count - 1];
//    }

//    // 그 중에서 가장 가까운 총알 궤적까지의 거리가 가장 큰 위치를 선택
//    if (movablePos.Count > 0)
//    {

//    }
//}
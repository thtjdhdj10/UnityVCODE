using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionPlayerController : Action
{
    public float speed;

    bool[] movementInput = new bool[4];

    Dictionary<WorldGeneral.Direction, KeyManager.CommandType> dirKeyDic =
        new Dictionary<WorldGeneral.Direction, KeyManager.CommandType>();
    Dictionary<WorldGeneral.Direction, WorldGeneral.Direction> dirRevdirDic =
        new Dictionary<WorldGeneral.Direction, WorldGeneral.Direction>();

    bool attackInput = false;

    public bool rotateToCursor = true;

    public ActionPlayerController()
    {
        dirKeyDic[WorldGeneral.Direction.LEFT] = KeyManager.CommandType.MOVE_LEFT;
        dirKeyDic[WorldGeneral.Direction.RIGHT] = KeyManager.CommandType.MOVE_RIGHT;
        dirKeyDic[WorldGeneral.Direction.UP] = KeyManager.CommandType.MOVE_UP;
        dirKeyDic[WorldGeneral.Direction.DOWN] = KeyManager.CommandType.MOVE_DOWN;

        dirRevdirDic[WorldGeneral.Direction.LEFT] = WorldGeneral.Direction.RIGHT;
        dirRevdirDic[WorldGeneral.Direction.RIGHT] = WorldGeneral.Direction.LEFT;
        dirRevdirDic[WorldGeneral.Direction.UP] = WorldGeneral.Direction.DOWN;
        dirRevdirDic[WorldGeneral.Direction.DOWN] = WorldGeneral.Direction.UP;
    }

    private void Update()
    {
        RotateToCursor();
    }

    public override void Activate(KeyInput ki)
    {
        if (ki.IsValid() == false)
            return;

        UpdateKeyInput(ki.command, ki.keyPressType);

        MovementProcess(ki);

        ProjectileProcess(ki);
    }

    private void UpdateKeyInput(KeyManager.CommandType command, KeyManager.KeyPressType type)
    {
        for (int d = 0; d < 4; ++d)
        {
            WorldGeneral.Direction dir = (WorldGeneral.Direction)d;
            if (command == dirKeyDic[dir])
            {
                if (type == KeyManager.KeyPressType.DOWN)
                {
                    movementInput[(int)dir] = true;

                    movementInput[(int)dirRevdirDic[dir]] = false;
                }
                else if (type == KeyManager.KeyPressType.PRESS)
                {
                    if (movementInput[(int)dirRevdirDic[dir]] == false)
                    {
                        movementInput[(int)dir] = true;
                    }
                }
                else if (type == KeyManager.KeyPressType.UP)
                {
                    movementInput[(int)dir] = false;
                }

                return;
            }
        }

        attackInput = false;

        if (command == KeyManager.CommandType.COMMAND_ATTACK &&
            (type == KeyManager.KeyPressType.DOWN ||
            type == KeyManager.KeyPressType.PRESS))
        {
            attackInput = true;
        }
        if (command == KeyManager.CommandType.COMMAND_ATTACK &&
            type == KeyManager.KeyPressType.UP)
        {
            attackInput = false;
        }

    }

    private void MovementProcess(KeyInput ki)
    {
        MovementComponent movable = null;

        MovementComponent[] movables = ki.player.GetComponents<MovementComponent>();

        for (int i = 0; i < movables.Length; ++i)
        {
            if (movables[i].movementType == MovementComponent.MovementType.VECTOR)
            {
                movable = movables[i];
                break;
            }
        }

        if (movable == null)
        {
            movable = ki.player.gameObject.AddComponent<MovementComponent>();
            movable.movementType = MovementComponent.MovementType.VECTOR;
        }

        movable.Speed = speed;
        movable.moveDir = movementInput;
    }

    private void ProjectileProcess(KeyInput ki)
    {
        ProjectileComponent[] projectiles = ki.player.GetComponents<ProjectileComponent>();

        for(int i = 0; i < projectiles.Length; ++i)
        {
            projectiles[i].activatedDic[UnitComponent.ActivatingType.CONTROLLER] = attackInput;
            projectiles[i].targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void RotateToCursor()
    {
        if(rotateToCursor == true)
        {
            Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.eulerAngles = new Vector3(0, 0,
                VEasyCalculator.GetDirection(playerPosition, mouseWorldPosition) + SpriteManager.spriteDefaultRotation);
        }
    }
}

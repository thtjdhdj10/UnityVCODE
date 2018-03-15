using UnityEngine;
using System.Collections.Generic;

/* 
 * 각 Operable 은 static 으로 동종의 Operable List 를 가짐.
 * ( Operable 순회 작업을 쉽게 하기 위하여 )
 */

public class UnitComponent : MonoBehaviour
{
    [System.NonSerialized]
    public Unit owner;

    [System.NonSerialized]
    public ComponentType type;

    public virtual void Init()
    {
        owner = GetComponent<Unit>();

        activatedDic[ActivatingType.OWNER] = owner.Activated;
    }

    // UnitComponent 들은 activate 에 의해 on/off 될 수 있음.
    // Shield 의 경우, 일정 시간마다 재생성되는 로직이 on/off 되고, Collision 은 충돌처리를 하거나 안하거나 하는 식.
    // 반대로, 피해를 받거나 하는 식의 수동적인 동작은 Activate 와 상관없이 동작함.
    // 그러니까 이걸로 스턴이나 속박같은걸 구현하면 됨
    public Dictionary<ActivatingType, bool> activatedDic = new Dictionary<ActivatingType, bool>();
    public enum ActivatingType
    {
        OWNER,
        CONTROLLER,
    }

    public bool IsActivated()
    {
        foreach(bool activated in activatedDic.Values)
        {
            if (activated == false)
                return false;
        }

        return true;
    }

    public enum ComponentType
    {
        NONE = 0,
        MOVEMENT,
        COLLISION,
        CONTROL,
        PATTERN,
        PROJECTILE,
        SHIELD,
    }

    protected virtual void Awake()
    {
        Init();

        if (this is MovementComponent)
            type = ComponentType.MOVEMENT;
        else if (this is CollisionComponent)
            type = ComponentType.COLLISION;
        else if (this is ControlComponent)
            type = ComponentType.CONTROL;
        else if (this is PatternComponent)
            type = ComponentType.PATTERN;
        else if (this is ProjectileComponent)
            type = ComponentType.PROJECTILE;
        else if (this is ShieldComponent)
            type = ComponentType.SHIELD;
        else
            type = ComponentType.NONE;

        if (type == ComponentType.NONE)
        {
            Destroy(this);
            return;
        }

        if(OperableListDic.ContainsKey(type) == false)
        {
            OperableListDic.Add(type, new List<UnitComponent>());
        }

        OperableListDic[type].Add(this);
    }

    public static Dictionary<ComponentType, List<UnitComponent>> OperableListDic = new Dictionary<ComponentType, List<UnitComponent>>();

    protected virtual void OnDestroy()
    {
        if(OperableListDic.ContainsKey(type) == true)
        {
            OperableListDic[type].Remove(this);
        }
    }
}

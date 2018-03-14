using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Unit : MyObject
{
    public static List<Unit> unitList = new List<Unit>();
    
    public Unit defaultTarget;

    public float targetSearchDelay = 0.25f;
    private float remainSearchDelay = 0f;

    // 최대한 게임에 종속되지 않게 하고 싶었지만 어쩔 수 없음.
    // 하지만 그렇게 나쁜 구조는 아닌듯..
    // 아니면 Editor 를 써서 노출시키는 방법도 있긴함. 근데 그러면 Unit마다 Editor 만들어야될거같은데. 그건 좀..
    public enum UnitType
    {
        NONE = 0,
        USE_DEFAULT_TARGET_TYPE,

        UNIT,

        LIVING_UNIT,

        ALLY,
        ENEMY,

        ALLY_LIVING_UNIT,
        ENEMY_LIVING_UNIT,

        ALLY_PLAYER,
        ENEMY_BOSS,

        ALLY_MODULE,
        ENEMY_MODULE,

        BULLET,

        ALLY_BULLET,
        ENEMY_BULLET,
    }

    public UnitType unitType;

    // GetChildrenType() 로 USE_DEFAULT_TARGET_TYPE 를 사용했을 때, unit 의 defaultTargetType 이 선택됨.
    public UnitType defaultTargetType;

    public static List<UnitType> GetChildrenType(Unit owner, UnitType type)
    {
        List<UnitType> ret = new List<UnitType>();

        UnitType _type = type;
        if (owner != null &&
            type == UnitType.USE_DEFAULT_TARGET_TYPE)
            _type = owner.defaultTargetType;

        switch (_type)
        {
            case UnitType.UNIT:
                ret.Add(UnitType.LIVING_UNIT);

                ret.Add(UnitType.ALLY);
                ret.Add(UnitType.ENEMY);

                ret.Add(UnitType.ALLY_LIVING_UNIT);
                ret.Add(UnitType.ENEMY_LIVING_UNIT);

                ret.Add(UnitType.ALLY_PLAYER);
                ret.Add(UnitType.ENEMY_BOSS);

                ret.Add(UnitType.ALLY_MODULE);
                ret.Add(UnitType.ENEMY_MODULE);

                ret.Add(UnitType.ALLY_BULLET);
                ret.Add(UnitType.ENEMY_BULLET);
                break;

            case UnitType.ENEMY:
                ret.Add(UnitType.ENEMY_LIVING_UNIT);

                ret.Add(UnitType.ENEMY_BOSS);
                ret.Add(UnitType.ENEMY_MODULE);
                ret.Add(UnitType.ENEMY_BULLET);
                break;

            case UnitType.ALLY:
                ret.Add(UnitType.ALLY_LIVING_UNIT);

                ret.Add(UnitType.ALLY_PLAYER);
                ret.Add(UnitType.ALLY_MODULE);
                ret.Add(UnitType.ALLY_BULLET);
                break;

            case UnitType.LIVING_UNIT:
                ret.Add(UnitType.ENEMY_LIVING_UNIT);

                ret.Add(UnitType.ENEMY_BOSS);
                ret.Add(UnitType.ENEMY_MODULE);

                ret.Add(UnitType.ALLY_LIVING_UNIT);

                ret.Add(UnitType.ALLY_PLAYER);
                ret.Add(UnitType.ALLY_MODULE);
                break;

            case UnitType.ALLY_LIVING_UNIT:
                ret.Add(UnitType.ALLY_PLAYER);
                ret.Add(UnitType.ALLY_MODULE);
                break;

            case UnitType.ENEMY_LIVING_UNIT:
                ret.Add(UnitType.ENEMY_BOSS);
                ret.Add(UnitType.ENEMY_MODULE);
                break;

            case UnitType.NONE:
                return ret;

            case UnitType.USE_DEFAULT_TARGET_TYPE:
                return ret;
        }

        ret.Add(_type);

        return ret;
    }

    //

    private bool activated = true;
    public bool Activated
    {
        get
        {
            return activated;
        }
        set
        {
            activated = value;

            UnitComponent[] comps = GetComponents<UnitComponent>();
            for (int i = 0; i < comps.Length; ++i)
            {
                comps[i].activatedDic[UnitComponent.ActivatingType.OWNER] = activated;
            }
        }
    }

    protected virtual void Start()
    {
        OnSpawn();
    }

    public virtual void OnSpawn()
    {
        Init();
    }

    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        VEasyPoolerManager.ReleaseObjectRequest(gameObject);
    }

    public virtual void OnLoseHP()
    {
        // TODO 플레이어가 체력잃었을 때 등 맞았을 때 효과
    }

    public virtual void OnHealHP()
    {

    }

    public virtual void OnGenerateShield()
    {
        // TODO 실드 생기는 이펙트 추가
    }

    public virtual void OnDestroyShield()
    {
        // TODO 실드 깨지는 이펙트 추가
    }

    //

    public virtual void Init()
    {
        InitSprite();

        SearchTarget();

        UnitComponent[] uc = GetComponents<UnitComponent>();
        for(int i = 0; i < uc.Length;++i)
        {
            uc[i].Init();
        }
    }

    public virtual void InitSprite()
    {

    }

    //

    protected virtual void OnEnable()
    {
        unitList.Add(this);
    }

    protected virtual void OnDisable()
    {
        unitList.Remove(this);
    }

    //

    protected virtual void FixedUpdate()
    {
        SearchProcess();


    }

    private void SearchProcess()
    {
        if (defaultTarget == null)
        {
            if (remainSearchDelay <= 0f)
            {
                TargetLost();
                remainSearchDelay = targetSearchDelay;
            }
            else
            {
                remainSearchDelay -= Time.fixedDeltaTime;
            }
        }
    }

    //

    public virtual void TargetLost()
    {
        SearchTarget();
    }

    public virtual bool SearchTarget()
    {
        return false;
    }
}

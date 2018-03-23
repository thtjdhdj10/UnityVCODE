using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Action : MonoBehaviour
{
    public static Dictionary<MyObject, List<Action>> actionsDic = new Dictionary<MyObject, List<Action>>();

    public static void Activate(MyObject caster, ActionData actionData)
    {
        if (actionData.IsValid() == false)
            return;

        if (actionsDic.ContainsKey(caster) == false)
            return;

        for (int i = 0; i < actionsDic[caster].Count; ++i)
        {
            Action action = actionsDic[caster][i];

            if (action.isWork == false)
                continue;

            action.Activate(actionData);

            if (action.isDisposable == true)
                Destroy(action);
        }
    }

    public bool isDisposable;

    public bool isWork = true;

    protected virtual void Awake()
    {
        MyObject owner = GetComponent<MyObject>();
        if (owner == null)
            Destroy(this);

        if(actionsDic.ContainsKey(owner) == false)
        {
            List<Action> actions = new List<Action>();
            actions.Add(this);
            actionsDic.Add(owner, actions);
        }
        else
        {
            actionsDic[owner].Add(this);
        }
    }

    //

    public virtual void Activate()
    {

    }

    public virtual void Activate(ActionData ad)
    {
        if (ad is HitResult)
            Activate(ad as HitResult);

        if (ad is KeyInput)
            Activate(ad as KeyInput);
    }

    public virtual void Activate(HitResult ad)
    {

    }

    public virtual void Activate(KeyInput ad)
    {

    }

    public class ActionData
    {
        public virtual bool IsValid()
        {
            return false;
        }
    }

    public class HitResult : ActionData
    {
        public Unit owner;
        public Unit target;

        public HitResult(Unit _owner, Unit _target)
            : base()
        {
            owner = _owner;
            target = _target;
        }

        public override bool IsValid()
        {
            if (owner == null ||
                target == null)
                return false;

            return true;
        }
    }

    public class KeyInput : ActionData
    {
        public MyObject player;
        public KeyManager.CommandType command;
        public KeyManager.KeyPressType keyPressType;

        public KeyInput(MyObject _player, KeyManager.CommandType _command, KeyManager.KeyPressType _keyPressType)
            : base()
        {
            player = _player;
            command = _command;
            keyPressType = _keyPressType;
        }

        public override bool IsValid()
        {
            if (player == null ||
                command == KeyManager.CommandType.NONE ||
                keyPressType == KeyManager.KeyPressType.NONE)
                return false;

            return true;
        }
    }
}
using UnityEngine;
using System.Collections.Generic;

public class ControlComponent : UnitComponent
{
    public virtual void ReceiveCommand(KeyManager.CommandType command, KeyManager.KeyPressType pressType)
    {
        if (IsActivated() == false)
            return;

        Action.Activate(owner, new Action.KeyInput(owner, command, pressType));
    }
}

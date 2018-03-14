using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using DicKeyCommand = System.Collections.Generic.Dictionary<int,
System.Collections.Generic.Dictionary<UnityEngine.KeyCode,
    KeyManager.CommandType>>;

// TODO: 조합 키 지원

public class KeyManager : MonoBehaviour {
    
    private int keySetNumber = 0;
    private int keySetCount = 0;

    public static DicKeyCommand keySettings = new DicKeyCommand();

    //

    private delegate bool GetKeyEachType(KeyCode kc);
    
    private Dictionary<KeyPressType, GetKeyEachType> GetKeyFunctions = new Dictionary<KeyPressType, GetKeyEachType>();

    public enum KeyPressType
    {
        NONE = 0,
        DOWN,
        UP,
        PRESS,
    }

    private void GetFunctionMatch()
    {
        GetKeyFunctions[KeyPressType.DOWN] = Input.GetKeyDown;
        GetKeyFunctions[KeyPressType.UP] = Input.GetKeyUp;
        GetKeyFunctions[KeyPressType.PRESS] = Input.GetKey;
    }

    //

    void Awake()
    {
        GetFunctionMatch();

    }

    void Update()
    {
        GiveCommand();

    }

    // 사용중인 key 값으로 dictionary 순회.
    // 유효한 KeyCode 들이 선택되어, Controlable 레이어에 있는 모든 유닛들에게
    // KeyCode 와 매칭되는 KeyCommand 가 전달된다.
    void GiveCommand()
    {
        List<GameObject> controlableList = new List<GameObject>();

        ControlComponent[] unitArr = FindObjectsOfType<ControlComponent>();
        for (int i = 0; i < unitArr.Length; ++i)
        {
            controlableList.Add(unitArr[i].gameObject);
        }

//      VEasyPoolerManager.RefObjectListAtLayer(LayerManager.StringToMask("Controlable"));

        if (controlableList == null)
            return;

        List<KeyCode> keyCodeList = keySettings[keySetNumber].Keys.ToList();

        for (int i = 0; i < keyCodeList.Count; ++i)
        {
            KeyCode keyCode = keyCodeList[i];
            
            foreach(KeyPressType type in GetKeyFunctions.Keys)
            {
                if (GetKeyFunctions[type](keyCode) == false)
                    continue;

                for (int j = 0; j < controlableList.Count; ++j)
                {
                    var controler = controlableList[j].GetComponent<ControlComponent>();
                    if (controler == null)
                        continue;

                    CommandType command = keySettings[keySetNumber][keyCode];

                    controler.ReceiveCommand(command, type);
                }
            }  
        }

    }

    void Start()
    {
        // 임시로 V_CODE 에 해당하는 KeySetting 사용

        int number = CreateKeySettings(GetDefaultKeySetting(KeySettingType.V_CODE));
        SetKeySetting(number);
    }

    public enum CommandType
    {
        NONE = 0,

        MOVE_LEFT,
        MOVE_RIGHT,
        MOVE_UP,
        MOVE_DOWN,

        SKILL_01,
        SKILL_02,
        SKILL_03,
        SKILL_04,
        SKILL_05,
        SKILL_06,
        SKILL_07,
        SKILL_08,
        SKILL_09,
        SKILL_10,
        SKILL_11,
        SKILL_12,

        ITEM_1,
        ITEM_2,
        ITEM_3,
        ITEM_4,
        ITEM_5,
        ITEM_6,
        ITEM_7,
        ITEM_8,
        ITEM_9,

        COMMAND_SKILL,
        COMMAND_ATTACK,
        COMMAND_JUMP,

        COMMAND_SPECIAL,
        COMMAND_RELOAD,
        COMMAND_SWAP,

        COMMAND_ZOOM,
        COMMAND_VIEW_ME,
        COMMAND_SIT,

        COMMAND_STOP,
        COMMAND_HOLD,
        COMMAND_MOVE,

        COMMAND_APPLY,
        COMMAND_MOVE_APPLY,

    }

    public enum KeySettingType
    {
        NONE,
        CUSTOM,
        DNF,
        V_CODE,
        FPS,
        STARCRAFT,
        LOL,
    }

    private Dictionary<KeyCode, CommandType> GetDefaultKeySetting(KeySettingType setting)
    {
        switch(setting)
        {
            case KeySettingType.DNF:
                {
                    return Setting_DNF();
                }
            case KeySettingType.FPS:
                {
                    return Setting_FPS();
                }
            case KeySettingType.LOL:
                {
                    return Setting_LOL();
                }
            case KeySettingType.STARCRAFT:
                {
                    return Setting_Starcraft();
                }
            case KeySettingType.V_CODE:
                {
                    return Setting_V_CODE();
                }
            default:
                {
                    return null;
                }
        }
    }

    // DNF
    private Dictionary<KeyCode, CommandType> Setting_DNF()
    {
        var ret = new Dictionary<KeyCode, CommandType>();

        {
            ret[KeyCode.LeftArrow] = CommandType.MOVE_LEFT;
            ret[KeyCode.RightArrow] = CommandType.MOVE_RIGHT;
            ret[KeyCode.UpArrow] = CommandType.MOVE_UP;
            ret[KeyCode.DownArrow] = CommandType.MOVE_DOWN;

            ret[KeyCode.A] = CommandType.SKILL_01;
            ret[KeyCode.S] = CommandType.SKILL_02;
            ret[KeyCode.D] = CommandType.SKILL_03;
            ret[KeyCode.F] = CommandType.SKILL_04;
            ret[KeyCode.G] = CommandType.SKILL_05;
            ret[KeyCode.H] = CommandType.SKILL_06;
            ret[KeyCode.Q] = CommandType.SKILL_07;
            ret[KeyCode.W] = CommandType.SKILL_08;
            ret[KeyCode.E] = CommandType.SKILL_09;
            ret[KeyCode.R] = CommandType.SKILL_10;
            ret[KeyCode.T] = CommandType.SKILL_11;
            ret[KeyCode.Y] = CommandType.SKILL_12;

            ret[KeyCode.Alpha1] = CommandType.ITEM_1;
            ret[KeyCode.Alpha2] = CommandType.ITEM_2;
            ret[KeyCode.Alpha3] = CommandType.ITEM_3;
            ret[KeyCode.Alpha4] = CommandType.ITEM_4;
            ret[KeyCode.Alpha5] = CommandType.ITEM_5;
            ret[KeyCode.Alpha6] = CommandType.ITEM_6;

            ret[KeyCode.Z] = CommandType.COMMAND_SKILL;
            ret[KeyCode.X] = CommandType.COMMAND_ATTACK;
            ret[KeyCode.C] = CommandType.COMMAND_JUMP;
            ret[KeyCode.Space] = CommandType.COMMAND_SPECIAL;
        }

        return ret;
    }

    // FPS
    Dictionary<KeyCode, CommandType> Setting_FPS()
    {
        var ret = new Dictionary<KeyCode, CommandType>();

        {
            ret[KeyCode.A] = CommandType.MOVE_LEFT;
            ret[KeyCode.D] = CommandType.MOVE_RIGHT;
            ret[KeyCode.W] = CommandType.MOVE_UP;
            ret[KeyCode.S] = CommandType.MOVE_DOWN;

            ret[KeyCode.Space] = CommandType.COMMAND_JUMP;

            ret[KeyCode.R] = CommandType.COMMAND_RELOAD;
            ret[KeyCode.Q] = CommandType.COMMAND_SWAP;
            ret[KeyCode.Mouse0] = CommandType.COMMAND_ATTACK;
            ret[KeyCode.Mouse1] = CommandType.COMMAND_ZOOM;
            ret[KeyCode.LeftShift] = CommandType.COMMAND_SIT;

            ret[KeyCode.Alpha1] = CommandType.ITEM_1;
            ret[KeyCode.Alpha2] = CommandType.ITEM_2;
            ret[KeyCode.Alpha3] = CommandType.ITEM_3;
            ret[KeyCode.Alpha4] = CommandType.ITEM_4;
            ret[KeyCode.Alpha5] = CommandType.ITEM_5;
            ret[KeyCode.Alpha6] = CommandType.ITEM_6;
        }

        return ret;
    }

    // V_CODE
    Dictionary<KeyCode, CommandType> Setting_V_CODE()
    {
        var ret = new Dictionary<KeyCode, CommandType>();

        {
            ret[KeyCode.A] = CommandType.MOVE_LEFT;
            ret[KeyCode.D] = CommandType.MOVE_RIGHT;
            ret[KeyCode.W] = CommandType.MOVE_UP;
            ret[KeyCode.S] = CommandType.MOVE_DOWN;

            ret[KeyCode.Mouse0] = CommandType.COMMAND_ATTACK;
            ret[KeyCode.Mouse1] = CommandType.COMMAND_SKILL;
            ret[KeyCode.Space] = CommandType.COMMAND_SPECIAL;

            ret[KeyCode.Alpha1] = CommandType.ITEM_1;
            ret[KeyCode.Alpha2] = CommandType.ITEM_2;
            ret[KeyCode.Alpha3] = CommandType.ITEM_3;
            ret[KeyCode.Alpha4] = CommandType.ITEM_4;
            ret[KeyCode.Alpha5] = CommandType.ITEM_5;
            ret[KeyCode.Alpha6] = CommandType.ITEM_6;
        }

        return ret;
    }

    // starcraft
    Dictionary<KeyCode, CommandType> Setting_Starcraft()
    {
        var ret = new Dictionary<KeyCode, CommandType>();

        {
            ret[KeyCode.Space] = CommandType.COMMAND_VIEW_ME;

            ret[KeyCode.R] = CommandType.COMMAND_RELOAD;
            ret[KeyCode.A] = CommandType.COMMAND_ATTACK;
            ret[KeyCode.M] = CommandType.COMMAND_MOVE;
            ret[KeyCode.H] = CommandType.COMMAND_HOLD;
            ret[KeyCode.S] = CommandType.COMMAND_STOP;

            ret[KeyCode.Mouse0] = CommandType.COMMAND_APPLY;
            ret[KeyCode.Mouse1] = CommandType.COMMAND_MOVE_APPLY;

            ret[KeyCode.Alpha1] = CommandType.ITEM_1;
            ret[KeyCode.Alpha2] = CommandType.ITEM_2;
            ret[KeyCode.Alpha3] = CommandType.ITEM_3;
            ret[KeyCode.Alpha4] = CommandType.ITEM_4;
            ret[KeyCode.Alpha5] = CommandType.ITEM_5;
            ret[KeyCode.Alpha6] = CommandType.ITEM_6;
            ret[KeyCode.Alpha7] = CommandType.ITEM_7;
            ret[KeyCode.Alpha8] = CommandType.ITEM_8;
            ret[KeyCode.Alpha9] = CommandType.ITEM_9;
        }

        return ret;
    }

    // LOL
    Dictionary<KeyCode, CommandType> Setting_LOL()
    {
        var ret = new Dictionary<KeyCode, CommandType>();

        {
            ret[KeyCode.Space] = CommandType.COMMAND_VIEW_ME;

            ret[KeyCode.Q] = CommandType.SKILL_01;
            ret[KeyCode.W] = CommandType.SKILL_02;
            ret[KeyCode.E] = CommandType.SKILL_03;
            ret[KeyCode.R] = CommandType.SKILL_04;

            ret[KeyCode.Alpha1] = CommandType.ITEM_1;
            ret[KeyCode.Alpha2] = CommandType.ITEM_2;
            ret[KeyCode.Alpha3] = CommandType.ITEM_3;
            ret[KeyCode.Alpha4] = CommandType.ITEM_4;
            ret[KeyCode.Alpha5] = CommandType.ITEM_5;
            ret[KeyCode.Alpha6] = CommandType.ITEM_6;

            ret[KeyCode.A] = CommandType.COMMAND_ATTACK;
            ret[KeyCode.M] = CommandType.COMMAND_MOVE;
            ret[KeyCode.S] = CommandType.COMMAND_STOP;

            ret[KeyCode.Mouse0] = CommandType.COMMAND_APPLY;
            ret[KeyCode.Mouse1] = CommandType.COMMAND_MOVE_APPLY;
        }

        return ret;
    }

    int CreateKeySettings(Dictionary<KeyCode, CommandType> keySet)
    {
        keySettings[keySetCount] = keySet;

        keySetCount++;

        return keySetCount - 1;
    }

    void EditKeySettings(Dictionary<KeyCode, CommandType> keySet, int idx)
    {
        if (idx < 0 || idx >= keySetCount)
            return;

        keySettings[idx] = keySet;
    }

    void SetKeySetting(int number)
    {
        if (number < 0 || number >= keySetCount)
            return;

        keySetNumber = number;
    }
}

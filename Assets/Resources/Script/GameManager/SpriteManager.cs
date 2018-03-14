using UnityEngine;
using System.Collections.Generic;

using TypeSpriteDictionary =
System.Collections.Generic.Dictionary<string,
System.Collections.Generic.Dictionary<string,
System.Collections.Generic.Dictionary<string, SpriteManager.SpriteAttribute>>>;

public class SpriteManager : MonoBehaviour
{
    public const bool PRINT_DEBUG = true;

    public int countLoadSprite;

    public static float spriteDefaultRotation = -90f;

    public static int spriteDefaultFramePerSec = 12;

    public static SpriteManager manager;

    void Awake()
    {
        manager = this;

        LoadSprite();
    }

    public TypeSpriteDictionary typeSpriteDic = new TypeSpriteDictionary();

    public class SpriteAttribute
    {
        public Sprite sprite;

        public bool isAnimation;

        public RuntimeAnimatorController controller;
        public float speed;
        public int frameCount;
        public float length;
    }

    [SerializeField]
    private List<string> loadingSpriteNameList = new List<string>();

    [SerializeField]
    private List<string> categoryKeywordList;
    [SerializeField]
    private List<string> nameKeywordList;
    [SerializeField]
    private List<string> statusKeywordList;

    public const string RESOURCES_PATH = "Resources";
    public const string SPRITE_PATH = "Sprite";

    const string EXCLUDE_KEYWORD = "@";

    public static SpriteAttribute GetSpriteAttribute(string category, string name)
    {
        return GetSpriteAttribute(category, name, "");
    }

    public static SpriteAttribute GetSpriteAttribute(string category, string name, string status)
    {
        if (manager.typeSpriteDic.ContainsKey(category) == false)
            return null;

        if (manager.typeSpriteDic[category].ContainsKey(name) == false)
            return null;

        if (manager.typeSpriteDic[category][name].ContainsKey(status) == false)
            return null;

        return manager.typeSpriteDic[category][name][status];
    }

    public static float AssignSpriteAttribute(GameObject go, SpriteAttribute sa) // return length
    {
        float ret = 0f;

        if(sa == null)
            return ret;

        if(sa.isAnimation == true &&
            sa.controller != null)
        {
            Animator a = go.GetComponent<Animator>();
            if (a == null)
                a = go.AddComponent<Animator>();

            a.runtimeAnimatorController = Instantiate(sa.controller);

            a.speed = sa.speed;

            a.Play("Entry");

            ret = sa.length;
        }
        else
        {
            Animator a = go.GetComponent<Animator>();
            if (a != null)
                Destroy(a);

            go.GetComponent<SpriteRenderer>().sprite = sa.sprite;
        }

        return ret;
    }

    //

    private void LoadSprite()
    {
        countLoadSprite = 0;

        loadingSpriteNameList.Clear();

        string fullPath = Application.dataPath + "/" +
            RESOURCES_PATH + "/" +
            SPRITE_PATH + "/";

        // Sprite Directory 이하의 Directory 들을 가져옴
        string[] targetDirectoryWithPath = System.IO.Directory.GetDirectories(fullPath);
        List<string> targetDirectoryWithPathList = new List<string>(targetDirectoryWithPath);
        targetDirectoryWithPathList.Add(fullPath);

        for (int i = 0; i < targetDirectoryWithPathList.Count; ++i)
        {
            // directory 들 이하의 png file 들을 가져옴
            string[] spriteNameWithPath = System.IO.Directory.GetFiles(targetDirectoryWithPathList[i], "*.png");

            CustomLog.CompleteLog("Sprite Root: " + targetDirectoryWithPathList[i]);

            for (int j = 0; j < spriteNameWithPath.Length; ++j)
            {
                if (spriteNameWithPath[j].Substring(0,1) == EXCLUDE_KEYWORD)
                    continue;

                // 4 파트로 나뉜 이름
                string[] strType = { "", "", "", "" };
                string[] splitName = GetSplitName(spriteNameWithPath[j]);

                // 소문자로 설정
                ToLowerNames(ref splitName);

                for (int k = 0; k < Mathf.Min(4, splitName.Length); ++k)
                {
                    strType[k] = splitName[k];
                }

                SpriteAttribute sa = new SpriteAttribute();

                // resource.load 를 위한 이름
                string resourceLoadName = GetLoadingName(spriteNameWithPath[j]);
                sa.sprite = Resources.Load<Sprite>(resourceLoadName);

                if(GetSpriteAttribute(strType) == "")
                {
                    sa.isAnimation = false;
                }
                else
                {
                    sa.isAnimation = true;

                    sa.frameCount = GetSpriteFrameCount(strType);
                    sa.speed = GetSpriteSpeed(strType);
                    sa.length = (1f / (float)spriteDefaultFramePerSec) / sa.speed * (float)(sa.frameCount - 1);

                }

                CutSpriteAttribute(ref strType);

                //

                if (sa.sprite == null)
                {
                    CustomLog.CompleteLogWarning(
                        "Invalid Sprite: " + resourceLoadName,
                        PRINT_DEBUG);

                    continue;
                }

                loadingSpriteNameList.Add(strType[0] + " " + strType[1] + " " + strType[2]);

                string category = strType[0];
                string name = strType[1];
                string status = strType[2];

                if (typeSpriteDic.ContainsKey(category) == false)
                    typeSpriteDic.Add(category, new Dictionary<string, Dictionary<string, SpriteAttribute>>());
                if (typeSpriteDic[category].ContainsKey(name) == false)
                    typeSpriteDic[category].Add(name, new Dictionary<string, SpriteAttribute>());
                if (typeSpriteDic[category][name].ContainsKey(status) == false)
                    typeSpriteDic[category][name].Add(status, sa);
                ++countLoadSprite;
            }
        }

        for (int i = 0; i < targetDirectoryWithPathList.Count; ++i)
        {
            string[] controllerNameWithPath = System.IO.Directory.GetFiles(targetDirectoryWithPathList[i], "*.controller");

            // load 된 sprite 가 animation 이면 attribute 에 controller 추가
            for (int k = 0; k < controllerNameWithPath.Length; ++k)
            {
                string[] strTypeController = { "", "", "" };
                string[] splitControllerName = GetSplitName(controllerNameWithPath[k]);

                // 소문자로 설정
                ToLowerNames(ref splitControllerName);

                for (int l = 0; l < Mathf.Min(3, splitControllerName.Length); ++l)
                {
                    strTypeController[l] = splitControllerName[l];
                }

                CutSpriteAttribute(ref strTypeController);

                SpriteAttribute sa = GetSpriteAttribute(strTypeController[0], strTypeController[1], strTypeController[2]);

                if (sa != null)
                {
                    if (sa.isAnimation == true)
                    {
                        string controllerLoadName = GetLoadingName(controllerNameWithPath[k]);

                        sa.controller = Instantiate(Resources.Load<RuntimeAnimatorController>(controllerLoadName));
                    }
                }
            }
        }

        CustomLog.CompleteLog("Load Sprite Count: " + countLoadSprite);
    }

    // [speed]_strip[frame]
    // [speed] 는 배속을 인자로 받음. 기본값은 1
    // return 값은 스프라이트 재생 간격 시간.
    private int GetSpriteFrameCount(string[] name)
    {
        string attribute = GetSpriteAttribute(name);
        if (attribute == "")
            return 1;

        int frameCountStartIndex = attribute.LastIndexOf("_strip") + "_strip".Length;

        attribute = attribute.Substring(frameCountStartIndex);

        return System.Convert.ToInt32(attribute);
    }

    private float GetSpriteSpeed(string[] name)
    {
        string attribute = GetSpriteAttribute(name);
        if (attribute == "")
            return (1f / (float)spriteDefaultFramePerSec);

        int speedEndIndex = attribute.LastIndexOf("_strip");

        attribute = attribute.Substring(0, speedEndIndex);

        return System.Convert.ToSingle(attribute);
    }

    private void CutSpriteAttribute(ref string[] name)
    {
        for (int i = 0; i < name.Length; ++i)
        {
            if (name[i].Contains("_strip") == true ||
                name[i] == "")
            {
                switch(i)
                {
                    case 0:
                        {
                            name = new string[] { "", "", "" };
                        }
                        return;
                    case 1:
                        {
                            name = new string[] { name[0], "", "" };
                        }
                        return;
                    case 2:
                        {
                            name = new string[] { name[0], name[1], "" };
                        }
                        return;
                    case 3:
                        {
                            name = new string[] { name[0], name[1], name[2] };
                        }
                        return;
                }
            }
        }
    }

    private string GetSpriteAttribute(string[] name)
    {
        for (int i = name.Length - 1; i >= 0; --i)
        {
            if (name[i].Contains("_strip") == true)
            {
                return name[i];
            }
        }

        return "";
    }


    private string[] GetSplitName(string name)
    {
        string[] splitName = name.Split('/', '\\');

        string nameWithExtension = splitName[splitName.Length - 1];

        string originName = nameWithExtension.Split('.')[0];

        return originName.Split(' ');
    }

    private void ToLowerNames(ref string[] name)
    {
        for (int i = 0; i < name.Length; ++i)
        {
            name[i] = name[i].ToLower();
        }
    }

    private string GetLoadingName(string name)
    {
        // / or \ 로 split
        string[] splitName = name.Split('/', '\\');

        string loadingName = "";

        // Resouces 하위의 상대경로 + 파일이름( 확장자명 제외 )

        bool b = false;
        for (int i = 0; i < splitName.Length; ++i)
        {
            if (splitName[i] == RESOURCES_PATH)
            {
                b = true;
                continue;
            }
            if (b)
            {
                loadingName += splitName[i] + "/";
            }
        }
        return loadingName.Split('.')[0];
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

using System.Diagnostics;

public enum ERenderEfficient
{
    High = 0,
    Middle = 1,
    Low = 2
}

public enum EReleaseVersion
{
    CHINA = 0,
    MALAYSIA = 1,
    CHINATAIWAN = 2,
    THAILAND = 3,
    ENGLISH = 4,
    Vietnam = 5
}

public enum ELayerDef
{
    UILayer = 5,
    RenderTex = 9,          //渲染纹理
    RenderTexEffect = 12,   //渲染特效

    BattleRole = 16,        //战斗角色
    BattleMap = 17,         //战斗地图

    Effect = 20,            //特效
    SkillPrepare = 25,      //技能准备
    SkillShow = 26,         //技能特写
}

public enum EPickState
{
    None = 0,
    PickDown = 1,
    PickMove = 2,
    PickUp = 3,
}

public class ParameterDef
{
    public static ERenderEfficient s_eRenderEfficient = ERenderEfficient.High;

    public static bool s_bCsvIsEncryptData = false;

    //public const int LAYER_EFFECT = 20;   //EffectLayer

    //public const string ANIM_DEATH_SKILL = "deathskill";
    public const string ANIM_BE_HIT = "behit";
    public const string ANIM_RUN = "run";
    public const string ANIM_IDLE = "idle";
    public const string ANIM_WIN = "win";
    public const string ANIM_DEATH = "death";
    //public const string ANIM_SPELL03 = "spell03";
    public const string ANIM_DEBUFF = "debuff";
    //public const string ANIM_LEVITATE = "fukong";
    //public const string ANIM_HIT_UP = "jifei";
    //public const string ANIM_SPELL02 = "spell02";
    public const string ANIM_ATTACK = "attack";

    public const string ACTOR_XIAOYANG = "g001_xiaoyang";
    public const string ACTOR_DEFAULT = "actorProxy";
    public const string ACTOR_YINDAO = "xinshouzhidaoyuan";
    public const string ACTOR_MUBEI = "g001_mubei";
    public const string ACTOR_BAOXIANG02 = "g001_baoxiang02";

    public const string MAP_HIT_GRID = "MapGridPoint";
    public const string MAP_PICK_SIGN = "PickSign";
    public const string SKILL_CAMERA_HEIDI = "CameraHeiDi";
    public const string MAP_PICK_LINK_LINE = "PickLinkLine";

    public const string MODEL_BAOXIANG = "g001_baoxiang";
    public const string MODEL_JINBI = "g001_jinbi";
    public const string MODEL_YILAGUAN = "g001_yilaguan0";

    //public const string LEVEL_FAKE_BATTLE = "shengbeizhanzheng02";
    //public const string LEVEL_STORY_BATTLE = "BattleScene024";

    public const string CAMERA_ANCHOR1 = "CameraAnchor1";

    public const string BATTLE_ROLES = "BattleRoles";
    public static bool s_bIsBattleCheat = false;
    public static string s_strBattleCheatContent = "";

    //public const string STR_IS_ALREADY_PLAY = "IsAlreadyPlay";

    public const float MAP_CELL_X_LENGTH = 0.012f;
    public const float MAP_CELL_Z_LENGTH = 0.012f;

    //public const int MAP_CELL_X_COUNT = 1000;
    //public const int MAP_CELL_Z_COUNT = 500;

    public const int MAP_REGION_CHECK_OFFSET = 50;      //地图区域检测偏移长度

    public const float MAP_PICK_MOUSE_DOWN_TIME = 0.2f; //地图鼠标按下后拾取连线显示等待时间

    public const string TINT_COLOR = "_TintColor";

    public const float TIME_SCALE_SLOW_WAIT_TIME = 3f;          //时间缩放慢速等待时间 
    public const float TIME_SCALE_SLOW_DOUBLE_SPEED = 100f;     //设置战斗慢速倍数

    public const string CAMERA_SKILL_PREPARE = "SkilPrepareCamera";     //技能准备摄像机
    public const string CAMERA_SKILL_SHOW = "SkillShowCamera";          //技能特写摄像机

    public const string BONE_DUMMY01 = "Dummy01";
    public const string BONE_WTOP = "w_top";
    public const string BONE_FBLOW = "f_blow";
    public const string BONE_SCENE_CENTER = "SceneTopCenter";

    public const float GAME_SPEED_NORMAL = 1.25f;                //游戏正常速度
    public const float GAME_SPEED_ACCELERATE = 1.8f;            //游戏加速速度

    public static Color COLOR_ROLE_HIT = new Color(100f / 255f, 0f / 255f, 0f / 255f, 0f);
    public static Color COLOR_SHI_HUA = new Color(64.0f / 255f, 64.0f / 255f, 64.0f / 255f);

    //public static int BATTLE_WIN_POS_MAX = 5;                   //战斗获胜的最大位置数量
    public static float BATTLE_WIN_POS_CENTER_Z = -2.0f;        //战斗获胜的中心位置(0,-2)
    public static float BATTLE_WIN_POS_INTERVAL = 2.0f;         //战斗获胜的位置间隔

    public const uint GAME_PICK_DELAY_TIME = 100;               //游戏拾取延迟时间

    public const int ROLE_ANIM_PAUSE_FRAME = 2;                 //角色战斗受击后动画暂停帧数

    public const string LOGIN_USER_UID_ARRAY = "LoginUserUidArray";         //登陆账户UID列表 //本地设备

    public const string LOGIN_LAST_SERVER_ID = "LastLoginServerId";         //上次登陆服务器ID
    public const string LOGIN_LAST_SERVER_ARRAY = "LastLoginServerArray";   //上次登陆服务器数组

    public const string SHADER_MODEL_NORMAL = "TXWS_Battle_Specular_ChiBi";     //模型正常shader
    public const string SHADER_MODEL_GRAY = "TXWS_Battle_Specular_ChiBiGray";   //模型灰度shader

    public const string SHADER_OUTLINE_COLOR_NAME = "_OutlineColor";        //shader轮廓渲染颜色参数名
    public const string SHADER_OUTLINE_WIDTH_NAME = "_Outline";             //shader轮廓渲染宽度参数名
    public const float SHADER_OUTLINE_WIDTH_VALUE = 0.03f;                  //shader轮廓渲染宽度数值

    public const float GUIDE_BTN_NEXT_SHOW_TIME = 3.0f;                     //新手引导下一步按钮显示时间

    public const float BATTLE_SHIP_SKILL_SHOW_TIME = 3.0f;                  //战斗战船技能展示时间

    public const float BATTLE_WIN_ANIM_MAX_TIME = 3.0f;                     //战斗胜利动画最大时长

    public const string SERVER_CHANNEL_LIST_FILE = "server.lst";            //服务器渠道列表文件名
    public const string SERVER_NOTIFY_FILE = "notify.lst";                  //服务器通告文件名

    public const string CHAT_SHOW_USER_INFO = "xytdgod8741";               //更改昵称使用命令密匙
    
    public static void SetChildLayer(Transform t, int layer)
    {
        t.gameObject.layer = layer;
        for (int i = 0; i < t.childCount; ++i)
        {
            Transform child = t.GetChild(i);

            child.gameObject.layer = layer;
            SetChildLayer(child, layer);
        }
    }

    static List<GameObject> m_slistRemoveObjects = new List<GameObject>();
    public static void RemoveChildrenNodes(GameObject objRoot)
    {
        if (objRoot != null)
        {
            Transform tranRoot = objRoot.transform;
            m_slistRemoveObjects.Clear();
            for (int i = 0; i < tranRoot.childCount; ++i)
            {
                GameObject objChild = tranRoot.GetChild(i).gameObject;
                m_slistRemoveObjects.Add(objChild);
            }
            for (int i = 0; i < m_slistRemoveObjects.Count; ++i)
            {
                GameObject.Destroy(m_slistRemoveObjects[i]);
            }
            m_slistRemoveObjects.Clear();
        }
    }

    public static void CreateDirectory(string strLocalPath)
    {
        if (!Application.isEditor)
        {
            return;
        }
        if (Directory.Exists(strLocalPath) == false)
        {
            Directory.CreateDirectory(strLocalPath);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

    }

    public static byte[] GetEncryptDataBytes(byte[] buffData)
    {
        byte[] buff = new byte[buffData.Length + 2];
        buff[buff.Length - 2] = 103;
        buff[buff.Length - 1] = 52;

        for (int i = 0; i < buffData.Length; ++i)
        {
            buff[i] = (byte)~buffData[i];
            //buff[i] = buffData[i];
        }
        return buff;
    }
    public static byte[] DecryptionDataBytes(byte[] data)
    {
        byte[] tmp = new byte[data.Length - 2];
        for (int i = 0; i < data.Length - 2; i++)
        {
            tmp[i] = (byte)~data[i];
            //tmp[i] = data[i];
        }
        return tmp;
    }

    public static void CreateEncryptFile(string strSourcePath, string strDestPath)
    {
        UnityEngine.Debug.Log("CreateEncryptFile:strSourcePath(" + strSourcePath + "),strDestPath(" + strDestPath + ")");

        FileStream fsSource = new FileStream(strSourcePath, FileMode.Open, FileAccess.ReadWrite);

        byte[] buffData = new byte[fsSource.Length];
        fsSource.Read(buffData, 0, (int)fsSource.Length);

        byte[] buff = ParameterDef.GetEncryptDataBytes(buffData);

        fsSource.Close();
        //File.Delete(strPath);

        FileStream fsDest = new FileStream(strDestPath, FileMode.Create);
        fsDest.Write(buff, 0, buff.Length);
        fsDest.Close();

        buff = null;

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
#endif

    }

    public static void CreateEncryptFile(MemoryStream msData, string strDestPath)
    {
        UnityEngine.Debug.Log("CreateEncryptFile:buff(" + msData.Length + "),strDestPath(" + strDestPath + "),Position(" + msData.Position + ")");

        //msData.Seek(0, SeekOrigin.Begin);
        //byte[] buffData = new byte[msData.Length];
        //msData.Read(buffData, 0, (int)msData.Length);
        byte[] buffData = msData.ToArray();
        //Debug.Log("buffData:" + buffData.Length);
        byte[] buff;
        if (ParameterDef.s_bCsvIsEncryptData)
        {
            buff = ParameterDef.GetEncryptDataBytes(buffData);
        }
        else
        {
            buff = buffData;
        }

        FileStream fsDest = new FileStream(strDestPath, FileMode.Create);
        fsDest.Write(buff, 0, buff.Length);
        fsDest.Close();

        buff = null;

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public static void SetGameOjbectMaterialsReset(GameObject objInput)
    {
        if (objInput == null)
            return;
        List<Component> componentsSkin = new List<Component>(objInput.transform.GetComponentsInChildren(typeof(Renderer)));
        List<Renderer> skinnedMeshList = componentsSkin.ConvertAll(c => (Renderer)c);
        //Debug.Log("SetGameOjbectMaterialsReset:" + skinnedMeshList.Count);

        for (int j = 0; j < skinnedMeshList.Count; j++)
        {
            Renderer smr = skinnedMeshList[j];
            SetRenderMaterialsReset(smr);
        }
    }

    public static void SetRenderMaterialsReset(Renderer smr)
    {
        Material[] materials = smr.GetComponent<Renderer>().sharedMaterials;
        //Debug.Log("materials(" + materials.Length + ")");

        foreach (Material mat in materials)
        {
            //Debug.Log("Shader(" + mat.shader.name + ")");
            if (mat != null)
            {
                Shader tmpShader = mat.shader;
                if (tmpShader != null)
                {
                    mat.shader = Shader.Find(tmpShader.name);
                }
            }
        }

    }

    public static GameObject SceneFindGameObject(string strName)
    {
        Scene scene = SceneManager.GetActiveScene();
        GameObject objFind = null;
        GameObject[] aRootObejcts = scene.GetRootGameObjects();
        for (int i = 0; i < aRootObejcts.Length; ++i)
        {
            bool bIsFind = TranFindGameObjectChild(aRootObejcts[i].transform, strName, ref objFind);
            if (bIsFind)
            {
                break;
            }
        }
        return objFind;
    }

    public static bool TranFindGameObjectChild(Transform tran, string strName, ref GameObject objFind)
    {
        bool bResult = false;
        if (tran.name == strName)
        {
            bResult = true;
            objFind = tran.gameObject;
        }
        else
        {
            for (int i = 0; i < tran.childCount; ++i)
            {
                Transform child = tran.GetChild(i);
                bResult = TranFindGameObjectChild(child, strName, ref objFind);
                if (bResult)
                {
                    break;
                }
            }
        }
        return bResult;
    }
}

using System;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Security.Cryptography;

public class Utils
{
    static TimeSpan mTimeSpan;
    static readonly DateTime mStartDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    //public static List<Item> itemCountList = new List<Item>();
    public static string UrlEncode(string str)
    {
        StringBuilder sb = new StringBuilder();

        byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
        for (int i = 0; i < byStr.Length; i++)
        {
            sb.Append(@"%" + Convert.ToString(byStr[i], 16));
        }

        return (sb.ToString());
    }
    /// <summary>
    /// 获取例子播放的时长
    /// </summary>
    /// <param name="transform">例子transform</param>
    /// <returns></returns>
    public static float ParticleSystemLength(Transform transform)
    {
        ParticleSystem[] particleSystems = transform.GetComponentsInChildren<ParticleSystem>();
        float maxDuration = 0;
        foreach (ParticleSystem ps in particleSystems)
        {
            if (ps.emission.enabled)
            {
                if (ps.main.loop)
                {
                    return -1f;
                }
                float dunration = 0f;
                if (ps.emission.rateOverTimeMultiplier <= 0)
                {
                    dunration = ps.main.startDelayMultiplier + ps.main.startLifetimeMultiplier;
                }
                else
                {
                    dunration = ps.main.startDelayMultiplier + Mathf.Max(ps.main.duration, ps.main.startLifetimeMultiplier);
                }
                if (dunration > maxDuration)
                {
                    maxDuration = dunration;
                }
            }
        }
        return maxDuration;
    }
    /// <summary>
    /// sha1 处理
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string SHA1HashStringForUTF8String(string s)
    {
        //string newValue = s.ToLower();
        byte[] bytes = Encoding.UTF8.GetBytes(s);


        var sha1 = SHA1.Create();
        byte[] hashBytes = sha1.ComputeHash(bytes);

        return HexStringFromBytes(hashBytes);
    }

    /// <summary>
    /// Convert an array of bytes to a string of hex digits
    /// </summary>
    /// <param name="bytes">array of bytes</param>
    /// <returns>String of hex digits</returns>
    public static string HexStringFromBytes(byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }
        return sb.ToString();
    }
    private const string MSG_ID_START_TAG = "\"MsgId\": ";
    private const string MSG_ID_END_TAG = ",";
#if ThaiDefine
    private const int    DATE_OFFSET      = 7              * 3600;
#else
    private const int DATE_OFFSET = 8 * 3600;
#endif
    /// <summary>
    /// 时间戳转换成日期
    /// </summary>
    public static DateTime GetTime(long timeStamp)
    {
        DateTime converted = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        DateTime newdatetime = converted.AddSeconds(timeStamp + DATE_OFFSET);
        return newdatetime.ToUniversalTime();
    }

    public static void SetTimeSpan(DateTime timeStamp)
    {
        TimeSpan span = DateTime.Now - timeStamp;
        mTimeSpan = span;
    }

    public static DateTime GetNowTime()
    {
        DateTime newNowTime = DateTime.Now - mTimeSpan;
        return newNowTime;
    }
    /// <summary>
    /// 0xe10=3600 倒计时
    /// </summary>
    /// <param name="timeDiff"></param>
    /// <returns></returns>
    public static string GetTimeDiffFormatString(TimeSpan timeSpan)
    {
        if (timeSpan.Equals(TimeSpan.Zero))
        {
            return "00:00:00";
        }
        return string.Format("{0,2:D2}:{1,2:D2}:{2,2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }
    public static int GetTimeSpanSeconds(TimeSpan timeSpan)
    {
        return (int)timeSpan.TotalSeconds;
    }
    public static string GetTimeDiffFormatString(long timeDiff)
    {
        if (timeDiff <= 0)
        {
            return "00:00";
        }
        return string.Format("{0,2:D2}:{1,2:D2}", (timeDiff % 0xe10) / 60, timeDiff % 60);
    }
    public static string GetTimeFormatString(long timeDiff)
    {
        if (timeDiff <= 0)
        {
            return "00:00:00";
        }
        return string.Format("{0,2:D2}:{1,2:D2}:{2,2:D2}", timeDiff / 0xe10, (timeDiff % 0xe10) / 60, timeDiff % 60);
    }
    public static int GetSecondByHour(int hour)
    {
        return hour * 3600;
    }
    /// <summary>
    /// 返回字符串的Int数值
    /// </summary>
    public static int GetString2Int(string json, string startString, string endString)
    {
        int itag = json.IndexOf(startString);
        if (itag != -1)
        {
            int startTag = itag + startString.Length;
            int endTag = json.IndexOf(endString, startTag);
            if (endTag != -1 && endTag > startTag)
            {
                return int.Parse(json.Substring(startTag, endTag - startTag));
            }
        }
        else
        {
            string strError = "Error! GetString2Int:length(" + json.Length + "),json(" + json + ")";
            Debuger.LogError(strError);
        }
        return -1;
    }
    public static int CaclStrengthCoinExpend(int exp)
    {
        return 2 * exp;
    }
    /************************************************************************/
    /* 比较string[] 是否存在数组中                                          */
    /************************************************************************/
    public static bool GetBoolInArray<T>(T[] arr, T compareVal)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i].Equals(compareVal))
            {
                return true;
            }
        }
        return false;
    }
    public static int RandomUnity(int startVal, int endVal)
    {
        return int.Parse(UnityEngine.Random.Range(startVal, endVal).ToString());
    }
    public static int ToInt(string v, int def)
    {
        int.TryParse(v, out def);
        return def;
    }

    public static string RegexReplace(string inputStr, string replaceStr, string replaceContent)
    {
        Regex regex = new Regex(replaceStr);
        string result = regex.Replace(inputStr, replaceContent, 100);
        return result;
    }
    public static List<string> StringIndexOf(string origin, string op)
    {
        List<string> arr = new List<string>();
        if (origin.IndexOf("|") == -1)
        {
            arr.Add(origin);
        }
        else
        {
            string[] tmpArr = origin.Split('|');
            for (int i = 0; i < tmpArr.Length; i++)
            {
                arr.Add(tmpArr[i]);
            }
        }
        return arr;

    }
    public static object DeepCopy(object s)
    {
        Type t = s.GetType();
        object obj = Activator.CreateInstance(s.GetType());
        PropertyInfo[] pis = t.GetProperties();
        for (int i = 0; i < pis.Length; i++)
        {
            PropertyInfo pi = pis[i];
            pi.SetValue(obj, pi.GetValue(s, null), null);
        }
        return obj;
    }
    public static T Clone<T>(T RealObject) where T : class,new()
    {
        //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制  
        BinaryFormatter formatter = new BinaryFormatter();
        Stream objectStream = new MemoryStream();
        formatter.Serialize(objectStream, RealObject);
        objectStream.Seek(0, SeekOrigin.Begin);
        return (T)formatter.Deserialize(objectStream);
    }
    public static string GetLabourIcon(int icon)
    {
        return "icon/icon" + string.Format("{0:D4}", icon + 1);
    }
    //生成小写字母
    public static string GetLowerCase(System.Random rnd)
    {
        int i = rnd.Next(97, 123);
        char c = (char)i;
        return c.ToString();
    }
    //生成大写字母
    public static string GetUpperCase(System.Random rnd)
    {
        int i = rnd.Next(65, 91);
        char c = (char)i;
        return c.ToString();
    }
    //生成大小写字母
    public static string GetUpperAndLowerCase(System.Random rnd)
    {
        int i = rnd.Next(65, 123);
        char c = (char)i;
        if (char.IsLetter(c))
        {
            return c.ToString();
        }
        else
        {
            return GetUpperAndLowerCase(rnd);
        }

    }
    //生成大小写字母和数字
    public static string GetNumberAndUpperAndLowerCase(System.Random rnd)
    {
        int i = rnd.Next(0, 123);
        if (i < 10)
        {
            return i.ToString();
        }
        char c = (char)i;
        if (char.IsLetter(c))
        {
            return c.ToString();
        }
        else
        {
            return GetUpperAndLowerCase(rnd);
        }
    }
    static Color _red = new Color(1.0f, 42f / 255f, 0, 1);
    public static Color GetRedColor()
    {
        return _red;
    }
    public static string GetColorRgbaStr(Color color)
    {
        int r, g, b, a;
        r = GetRoundNum(color.r * 255);
        g = GetRoundNum(color.g * 255);
        b = GetRoundNum(color.b * 255);
        a = GetRoundNum(color.a * 255);
        StringBuilder colorSB = new StringBuilder();
        colorSB.Append(string.Format("{0:X2}", r));
        colorSB.Append(string.Format("{0:X2}", g));
        colorSB.Append(string.Format("{0:X2}", b));
        colorSB.Append(string.Format("{0:X2}", a));
        string colorStr = colorSB.ToString().ToUpper();
        return colorStr.ToString();
    }
    public static Color GetColorByRgbaStr(string str)
    {
        if (str.Length < 8)
        {
            Debug.LogError("Error! getColorByRgbaStr, str(" + str + ")");
            return Color.white;
        }
        string strR, strG, strB, strA;
        strR = str.Substring(0, 2);
        strG = str.Substring(2, 2);
        strB = str.Substring(4, 2);
        strA = str.Substring(6, 2);

        Color color = new Color
        {
            r = Convert.ToInt32(strR, 16) / 255f,
            g = Convert.ToInt32(strG, 16) / 255f,
            b = Convert.ToInt32(strB, 16) / 255f,
            a = Convert.ToInt32(strA, 16) / 255f
        };
        return color;
    }
    public static int GetRoundNum(float num)
    {
        return (int)Math.Round(num);
    }
    static public byte[] ToByte(ref string s)
    {
        return System.Text.Encoding.GetEncoding("gb2312").GetBytes(s);
    }
    static public string ToBase64(string s)
    {
#pragma warning disable CS0618 // 类型或成员已过时
        return WWW.EscapeURL(s);
#pragma warning restore CS0618 // 类型或成员已过时
    }
    static public string Write(List<List<string>> dataList, char compart)
    {
        string result = "";
        List<string> strList;
        for (int i = 0; i < dataList.Count; ++i)
        {
            strList = dataList[i];
            for (int j = 0; j < strList.Count; ++j)
            {
                result += strList[j];
                result += compart;
            }

            result += '\n';
        }

        return result;
    }

    public static bool StringToEnum<T>(string str, out T v)
    {
        try
        {
            v = (T)System.Enum.Parse(typeof(T), str, true);
            return true;
        }
        catch (System.Exception)
        {
            v = default;
            return false;
        }
    }
    public static void ResetTransform(GameObject go)
    {
        Transform tr = go.transform;
        tr.localScale = Vector3.one;
        tr.localPosition = Vector3.zero;
        tr.localRotation = Quaternion.identity;
        go.SetActive(true);
    }
    public static T StringToEnum<T>(string str, T def)
    {
        try
        {
            def = (T)System.Enum.Parse(typeof(T), str, true);
        }
        catch (System.Exception)
        {

        }

        return def;
    }

    // 随机从list当中取nums个数值，保证不会重复提取
    public static T[] RandList<T>(T[] list, int nums)
    {
        nums = Mathf.Min(nums, list.Length);
        int[] indexs = new int[list.Length];
        for (int i = 0; i < indexs.Length; ++i)
            indexs[i] = i;

        T[] result = new T[nums];
        for (int i = 0; i < nums; ++i)
        {
            int randv = UnityEngine.Random.Range(i, indexs.Length);
            if (randv != i)
            {
                int v = indexs[i];
                indexs[i] = indexs[randv];
                indexs[randv] = v;
            }

            result[i] = list[indexs[i]];
        }

        return result;
    }

    public static List<T> ArrayToList<T>(T[] array)
    {
        List<T> list = new List<T>(array);
        return list;
    }

    // 地形投影到相机范围的多边形
    public static Vector3[] GetCameraScreenTerrain(Camera camera, string strTerrain)
    {
        int layer = 1 << LayerMask.NameToLayer(strTerrain);
        Ray[] rays = new Ray[4];
        rays[0] = camera.ScreenPointToRay(new Vector3(0f, 0f, 0f));
        rays[1] = camera.ScreenPointToRay(new Vector3(camera.pixelWidth, 0f, 0f));
        rays[2] = camera.ScreenPointToRay(new Vector3(camera.pixelWidth, camera.pixelHeight, 0f));
        rays[3] = camera.ScreenPointToRay(new Vector3(0f, camera.pixelHeight, 0f));

        Vector3[] poss = new Vector3[4];
        for (int i = 0; i < rays.Length; ++i)
        {
            if (Physics.Raycast(rays[i], out RaycastHit info, float.MaxValue, layer) == false)
                poss[i] = Vector3.zero;
            else
                poss[i] = info.point;
        }

        return poss;
    }

    /** Checks if \a p is inside the polygon (XZ space)
         * \author http://unifycommunity.com/wiki/index.php?title=PolyContainsPoint (Eric5h5)
         * */
    public static bool ContainsPoint(Vector3[] polyPoints, Vector3 p)
    {
        int j = polyPoints.Length - 1;
        bool inside = false;

        for (int i = 0; i < polyPoints.Length; j = i++)
        {
            if (((polyPoints[i].z <= p.z && p.z < polyPoints[j].z) || (polyPoints[j].z <= p.z && p.z < polyPoints[i].z)) &&
                (p.x < (polyPoints[j].x - polyPoints[i].x) * (p.z - polyPoints[i].z) / (polyPoints[j].z - polyPoints[i].z) + polyPoints[i].x))
                inside = !inside;
        }
        return inside;
    }

    static public Vector3 ScreenToWorldPosition(Vector3 screenPosition, Camera camera, int layer, out RaycastHit info)
    {
        Ray ray = camera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out info, float.MaxValue, layer) == false)
            return Vector3.zero;

        return info.point;
    }

    public static Vector3 QuaternionToDirection(Quaternion q)
    {
        return Matrix4x4.TRS(Vector3.zero, q, Vector3.one).MultiplyVector(Vector3.forward);
    }

    static public float GetCamerToTerrainDistance(Camera camera, out RaycastHit info, string strTerrain)
    {
        Ray ray = new Ray(camera.transform.localToWorldMatrix.MultiplyPoint(Vector3.back), QuaternionToDirection(camera.transform.rotation));
        if (Physics.Raycast(ray, out info, float.MaxValue, 1 << LayerMask.NameToLayer(strTerrain)) == false)
        {
            ray.origin = ray.GetPoint(-10000f);
            if (Physics.Raycast(ray, out info, float.MaxValue, 1 << LayerMask.NameToLayer(strTerrain)) == false)
                return 0f;

            return -info.distance;
        }

        return info.distance;
    }

    // a和b是线段的两个端点， c是检测点XZSpace
    public static float DistanceFromPointToLine(Vector3 l1, Vector3 l2, Vector3 point)
    {
        // given a line based on two points, and a point away from the line,
        // find the perpendicular distance from the point to the line.
        // see http://mathworld.wolfram.com/Point-LineDistance2-Dimensional.html
        // for explanation and defination.
        return (float)(Math.Abs((l2.x - l1.x) * (l1.z - point.z) - (l1.x - point.x) * (l2.z - l1.z)) / Math.Sqrt(Math.Pow(l2.x - l1.x, 2f) + Math.Pow(l2.z - l1.z, 2f)));
    }
    //判断输入是否为中文
    public static bool HasChinese(string content)
    {
        //判断是不是中文
        string regexstr = @"[\u4e00-\u9fa5]";
        //判断是不是泰文
        string regexstrthai = @"[\u0E00-\u0E7F]";
        if (Regex.IsMatch(content, regexstr) || Regex.IsMatch(content, regexstrthai))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //判断是不是数字
    public static bool IsInterger(string str)
    {
        if (str == "")
        {
            return false;
        }
        else
        {
            foreach (char c in str)
            {
                if (char.IsNumber(c))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
        }
        return true;

    }

    //只允许数字或字母的判断
    public static bool IsIntergerOrLetter(string content)
    {
        System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
        return reg1.IsMatch(content);
    }

    public static GameObject FindGameObjectInChildren(string gameObjectName, Transform trans)
    {
        if (trans == null)
        {
            return null;
        }

        foreach (Transform child in trans)
        {
            if (child.name == gameObjectName)
            {
                return child.gameObject;
            }

            GameObject go = FindGameObjectInChildren(gameObjectName, child);

            if (go != null)
            {
                return go;
            }
        }

        return null;
    }
    public static T GetComponentSingleton<T>(GameObject go) where T : Component
    {
        T com = go.GetComponent<T>();
        if (com == null)
        {
            go.AddComponent<T>();
        }
        com = go.GetComponent<T>();
        return com;
    }
    public static void DestroyComponent<T>(GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();
        if (component != null)
        {
            UnityEngine.Object.Destroy(component);
        }
    }
    /// <summary>
    /// 获取枚举类型Description中文说明
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string GetDescription(Enum obj)
    {
        string objName = obj.ToString();
        Type t = obj.GetType();
        FieldInfo fi = t.GetField(objName);
        System.ComponentModel.DescriptionAttribute[] arrDesc = (System.ComponentModel.DescriptionAttribute[])fi.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
        return arrDesc[0].Description;
    }
    /// <summary>
    /// 优化string 拼接方式 代替加号
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string StringAdd(params string[] args)
    {
        StringBuilder sb = new StringBuilder();
        if (args.Length == 3)
        {
            sb.AppendFormat("{0}{1}{2}", args);
        }
        else
        {
            sb.AppendFormat("{0}{1}", args);
        }
        return sb.ToString();
    }
    /// <summary>
    /// 去掉字串#后部分
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string GetSubString(string str)
    {
        int index = 0;
        if (!string.IsNullOrEmpty(str) && str.Contains("#"))
        {
            index = str.LastIndexOf('#');
            str = str.Substring(0, index);
        }
        return str;
    }
    public static string GetTimeString(int time)
    {
        string timeStr = "";
        int second, minute;
        second = time % 60;
        minute = time / 60;
        timeStr = string.Format("{0:D2}", minute) + ":" + string.Format("{0:D2}", second);
        return timeStr;
    }

    public static bool ContainsAnim(Animation animation, string animName)
    {
        foreach (AnimationState state in animation)
        {
            if (state.name.CompareTo(animName) == 0)
            {
                return true;
            }
        }
        return false;
    }
    public static void ListCopy<T>(List<T> listNew, List<T> listOld)
    {
        listNew.Clear();
        for (int i = 0; i < listOld.Count; i++)
        {
            listNew.Add(listOld[i]);
        }
    }
    public static System.Action[] GetActionArray(int actionNum, params System.Action[] actions)
    {
        if (actions != null && actions.Length == actionNum)
        {
            return actions;
        }
        else
        {
            Action[] actionArr = new Action[actionNum];
            for (int i = 0; i < actionNum; i++)
            {
                if (actions != null && actions.Length > i)
                {
                    actionArr[i] = actions[i];
                }
                else
                {
                    actionArr[i] = null;
                }
            }
            return actionArr;
        }
    }
    public static void SetAppleTexture(GameObject androidTexture, GameObject iosTexture)
    {
        androidTexture.SetActive(false);
        iosTexture.SetActive(false);
#if UNITY_ANDROID
        androidTexture.SetActive(true);
#elif UNITY_IOS
        iosTexture.SetActive(true);
#elif UNITY_EDITOR
        androidTexture.SetActive(true);
#endif
    }
}
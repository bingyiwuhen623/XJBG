using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using System.IO;
using System.Threading;
using XJBG.Base;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using System.Reflection;

public delegate IEnumerator InitLoad(string strName);
[Serializable]
public class Table<T> where T : new()
{
    private static Table<T> me;
    public static Table<T> Me
    {
        get
        {
            if (me == null)
            {
                me = new Table<T>();
                me.Init();
            }
            return me;
        }
    }

    [NonSerialized] private static readonly string PROPERTY_ID = "ID";

    public Dictionary<long, long> mIdIndexs;
    public T[] mDatas;

    private void Init()
    {
        mIdIndexs = new Dictionary<long, long>();
    }

    public T GetById(long id)
    {
        long index = mIdIndexs[id];
        return mDatas[index];
    }

    public IEnumerator DeSerializable(string name)
    {
        string filename = Application.streamingAssetsPath + "/DataBytes/" + name + ".bytes";
        bool fileExist = File.Exists(filename);
        if (fileExist) { }
        if (Application.platform != RuntimePlatform.Android)
        {
            filename = "file://" + filename;
        }
        Debuger.Log("DeSerializable:" + filename);
#pragma warning disable CS0618 // 类型或成员已过时
        WWW www = new WWW(filename);
#pragma warning restore CS0618 // 类型或成员已过时
        yield return www;
#if UNITY_ANDROID
        Interlocked.Increment(ref StartGame.Me.mNumThreads);
        ThreadPool.QueueUserWorkItem(ProcessTableDataFileFormat, www.bytes);
#else
        ProcessTableDataFileFormat(www.bytes);
#endif
        www = null;
    }

    private void ProcessTableDataFileFormat(object objBytesData)
    {
        byte[] bytesData = (byte[])objBytesData;
        MemoryStream fs = new MemoryStream(bytesData);

        if (fs != null)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                string json = formatter.Deserialize(fs) as string;
                DeSerializableJson(json);
                fs.Close();
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Ex:" + ex.ToString());
            }
        }
#if UNITY_ANDROID
        Interlocked.Decrement(ref StartGame.Me.mNumThreads);
        Debuger.Log("ProcessTableDataFileFormat.End:" + this.GetType());
#endif
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="json"></param>
    private void DeSerializableJson(string json)
    {
        mDatas = JsonMapper.ToObject<T[]>(json);
        Sort();
    }

    /// <summary>
    /// 序列化
    /// </summary>
    public void SerializeNow(string name)
    {
        BinaryFormatter bft;

        LoadDirectly(name);
        
        string strSerializePath = Application.streamingAssetsPath + "/DataBytes/";
        ParameterDef.CreateDirectory(strSerializePath);
        string strBytesFile = strSerializePath + name + ".bytes";

        MemoryStream msData = new MemoryStream();
        bft = new BinaryFormatter();
        bft.Serialize(msData, JsonMapper.ToJson(mDatas));

        ParameterDef.CreateEncryptFile(msData, strBytesFile);
    }

    public void LoadDirectly(string strName, bool bIsResource = false)
    {
        TextAsset textAsset = null;
#if UNITY_EDITOR
        string strAssetPath = "Assets/DataExcel/" + strName + ".csv";
        textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(strAssetPath);
#endif
        if (null == textAsset)
        {
            Debug.LogError("Table<" + typeof(T).Name + "> Load Failed.");
            return;
        }
        string strAllContent = textAsset.text;
        LoadDirectlyStr(strAllContent);
    }

    public void LoadDirectlyStr(string strAllContent)
    {

        char[] trimChars = new char[] { '\r', '\n', '\t', ' ' };
        char[] trimReturnChars = new char[] { '\r', '\n' };
        
        string[] lines = strAllContent.Split('\n');
        string[] names = lines[0].Split('\t');

        int iRowCount = lines.Length;
        Debug.Log("LoadDirectlyStr: iRowCount(" + iRowCount + ")");
        mDatas = new T[iRowCount - 1];
        for (int i = 0; i < names.Length; i++)
        {
            names[i] = names[i].TrimEnd(trimChars);
            names[i] = names[i].TrimStart(trimChars);
        }
        for (int i = 1; i < iRowCount; i++)
        {
            string line = lines[i];
            line = line.TrimEnd(trimReturnChars);
            if (line.Length == 0)
            {
                continue;
            }
            string[] values = line.Split('\t');

            if (values.Length != names.Length)
            {
                Debug.LogWarning("names(" + names.Length + "),values(" + values.Length + ")_" + line);
            }

            for (int j = 0; j < values.Length; j++)
            {
                values[j] = values[j].TrimEnd(trimChars);
                values[j] = values[j].TrimStart(trimChars);
            }
            mDatas[i - 1] = GetItemFromNames(names, values, i);
        }
        Sort();

        Debuger.Log(typeof(T).Name + " has loaded!");
    }

    private void Sort()
    {
        PropertyInfo info = typeof(T).GetProperty(PROPERTY_ID);
        if (info == null)
        {
            Debug.LogError("表格错误，没有ID列:" + typeof(T));
            return;
        }
        List<T> a = new List<T>();
        for (int i = 0; i < mDatas.Length; i++)
        {
            T t = mDatas[i];
            if (t != null)
                a.Add(t);
        }
        a.Sort(CompareT);
        mDatas = a.ToArray();

        int index = 0;
        foreach (T data in mDatas)
        {
            long id = Convert.ToInt64(info.GetValue(data, null));
            mIdIndexs.Add(id, index);
            index++;
        }
    }

    public int CompareT(T x, T y)
    {
        PropertyInfo info = typeof(T).GetProperty(PROPERTY_ID);
        long xid = Convert.ToInt64(info.GetValue(x, null));
        long yid = Convert.ToInt64(info.GetValue(y, null));
        int iReturn = 0;
        if (xid > yid)
        {
            iReturn = 1;
        }
        else if (xid < yid)
        {
            iReturn = -1;
        }
        return iReturn;
    }

    private int GetPropertyIndexFromName(string strPropName, string[] aNames)
    {
        for (int i = 0; i < aNames.Length; i++)
        {
            if (0 == aNames[i].CompareTo(strPropName))
            {
                return i;
            }
            else if (i >= aNames.Length - 1)
            {
                //Debug.LogWarning("Error! GetPropertyIndexFromName:propName(" + strPropName + ")");
            }
        }
        return -1;
    }

    private T GetItemFromNames(string[] names, string[] values, int iRow)
    {
        T item = new T();
        PropertyInfo[] propInfos = typeof(T).GetProperties();

        string strValue = null;

        for (int x = 0; x < propInfos.Length; ++x)
        {
            PropertyInfo info = propInfos[x];
            
            strValue = null;
            int iPropIndex = GetPropertyIndexFromName(info.Name, names);
            if (iPropIndex >= 0)
            {
                if (iPropIndex < values.Length)
                {
                    strValue = values[iPropIndex];
                }
                else
                {
                    Debug.LogWarning("Error! GetItemFromNames:info.Name(" + info.Name + "),iPropIndex(" + iPropIndex + "),values.Length(" + values.Length + ")");
                }
            }
            if (!string.IsNullOrEmpty(strValue))
            {
                try
                {
                    if (info.PropertyType == typeof(string))
                    {
                        string value = strValue.Replace("\\n", "\n");
                        info.SetValue(item, value, null);
                    }
                    else if (info.PropertyType == typeof(float))
                    {
                        float value = float.Parse(strValue);
                        info.SetValue(item, value, null);
                    }
                    else if (info.PropertyType == typeof(byte) ||
                             info.PropertyType == typeof(System.Byte))
                    {
                        byte value = byte.Parse(strValue);
                        info.SetValue(item, value, null);
                    }
                    else if (info.PropertyType == typeof(int) ||
                             info.PropertyType == typeof(System.Int32))
                    {
                        int value = int.Parse(strValue);
                        info.SetValue(item, value, null);
                    }
                    else if (info.PropertyType == typeof(long) ||
                             info.PropertyType == typeof(System.Int64))
                    {
                        long value = long.Parse(strValue);
                        info.SetValue(item, value, null);
                    }
                    else if (info.PropertyType == typeof(string[]))
                    {
                        string[] value = strValue.Split('|');
                        info.SetValue(item, value, null);
                    }
                    else if (info.PropertyType == typeof(float[]))
                    {
                        string[] _values = strValue.Split('|');
                        float[] fvalues = new float[_values.Length];
                        for (int j = 0; j < _values.Length; j++)
                        {
                            fvalues[j] = float.Parse(_values[j]);
                        }
                        info.SetValue(item, fvalues, null);
                    }
                    else if (info.PropertyType == typeof(int[]))
                    {
                        string[] _values = strValue.Split('|');
                        int[] fvalues = new int[_values.Length];
                        for (int j = 0; j < _values.Length; j++)
                        {
                            fvalues[j] = int.Parse(_values[j]);
                        }
                        info.SetValue(item, fvalues, null);
                    }
                    else if (info.PropertyType == typeof(byte[]))
                    {
                        string[] _values = strValue.Split('|');
                        byte[] fvalues = new byte[_values.Length];
                        for (int j = 0; j < _values.Length; j++)
                        {
                            fvalues[j] = byte.Parse(_values[j]);
                        }
                        info.SetValue(item, fvalues, null);
                    }
                    else
                    {
                        Debuger.Log("strValue(" + strValue + "),info(" + info.Name + ")");
                        object o = Enum.Parse(info.PropertyType, strValue);
                        info.SetValue(item, o, null);
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("GetItemFromNames:iRow(" + iRow + "),col(" + x + "),info(" + info.Name + "),PropertyType(" + info.PropertyType + "),strValue(" + values[iPropIndex] + "),ex(" + ex + ")");
                }

            }
        }
        return item;
    }
}
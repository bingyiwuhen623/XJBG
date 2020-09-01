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
using System.Data;
using Excel;
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

    public Dictionary<int, T> Datas;

    private void Init()
    {
        Datas = new Dictionary<int, T>();
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
        ProcessTableDataFileFormat(bytesData);
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

    private void DeSerializableJson(string json)
    {
        T[] tables = JsonMapper.ToObject<T[]>(json);
        PropertyInfo idProp = null;
        PropertyInfo[] propInfos = typeof(T).GetProperties();
        for (int i = 0; i < propInfos.Length; i++)
        {
            string name = propInfos[i].Name;
            if (name.Equals("ID") || name.Equals("id") || name.Equals("Id"))
            {
                idProp = propInfos[i];
            }
        }
        if (idProp == null)
        {
            Debug.LogError("该表无ID列：" + typeof(T));
        }
        int index = 0;
        foreach (T table in tables)
        {
            int id = index;
            if (idProp != null)
            {
                id = int.Parse(idProp.GetValue(table).ToString());
            }
            Datas.Add(id, table);
        }
    }

    /// <summary>
    /// 序列化
    /// </summary>
    public void SerializeNow(string name)
    {
        //FileStream fileStream;
        BinaryFormatter bft;

        LoadDirectly(name);

        string strSerializePath = Application.streamingAssetsPath + "/DataBytes/";
        ParameterDef.CreateDirectory(strSerializePath);
        string strBytesFile = strSerializePath + name + ".bytes";

        MemoryStream msData = new MemoryStream();
        bft = new BinaryFormatter();
        bft.Serialize(msData, JsonMapper.ToJson(Datas));

        ParameterDef.CreateEncryptFile(msData, strBytesFile);
    }

    public void LoadDirectly(string strName, bool bIsResource = false)
    {
        DataSet result = null;
#if UNITY_EDITOR
        string xlsxPath = Application.dataPath + "/DataExcel/" + strName + ".xlsx";
        Debug.Log("xlsxPath:" + xlsxPath);
        FileStream fileStream = File.Open(xlsxPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
        result = excelReader.AsDataSet();
        excelReader.Close();
#endif
        if (null == result)
        {
            Debug.LogError("Table<" + typeof(T).Name + "> Load Failed.");
            return;
        }
        LoadDirectlyStr(result, strName);
    }

    public void LoadDirectlyStr(DataSet dataSet, string name)
    {
        DataTable firstTable = dataSet.Tables[0];
        // 第一行数据，列名
        DataRow firstRow = firstTable.Rows[0];
        string[] names = new string[firstRow.ItemArray.Length];
        for (int i = 0; i < firstRow.ItemArray.Length; i++)
        {
            names[i] = firstRow[i].ToString();
        }

        int column = firstTable.Columns.Count;
        int row = firstTable.Rows.Count;
        Debug.LogWarning(column + "  " + row);
        for (int i = 1; i < row; i++)
        {
            DataRow singleRow = firstTable.Rows[i];
            string[] singleValues = new string[singleRow.ItemArray.Length];
            for (int j = 0; j < singleRow.ItemArray.Length; j++)
            {
                singleValues[j] = singleRow[j].ToString();
            }

            T item = GetItemByData(names, singleValues, i);

            int id = int.Parse(singleValues[0].ToString());
            Datas.Add(id, item);
        }
    }

    private T GetItemByData(string[] names, string[] values, int iRow)
    {
        T item = new T();

        //string strIdValue = GetStringByPropertyName("Id", names, values);
        //Debug.Log("strIdValue = " + strIdValue);
        //int id = int.Parse(strIdValue);
        PropertyInfo[] propInfos = typeof(T).GetProperties();

        string strValue = null;

        for (int x = 0; x < propInfos.Length; ++x)
        {
            PropertyInfo info = propInfos[x];

            //string strValue = GetStringByPropertyName(info.Name, names, values);
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
                        //Debuger.Log("strValue(" + strValue + "),info(" + info.Name + ")");
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

    int GetPropertyIndexFromName(string strPropName, string[] aNames)
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
}
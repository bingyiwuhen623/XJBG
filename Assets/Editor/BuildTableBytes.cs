using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildTableBytes
{
    private static Dictionary<string, Action<string>> mTableNames;
    [MenuItem("Tools/BuildBytes")]
    public static void BuildBytes()
    {
        mTableNames = new Dictionary<string, Action<string>>();
        AddName("TableItem", TableItem.Me.SerializeNow);

        DeSerializableBytes();
    }

    private static void AddName(string tableName, Action<string> action)
    {
        mTableNames.Add(tableName, action);
    }

    public static void DeSerializableBytes()
    {
        foreach (KeyValuePair<string, Action<string>> pair in mTableNames)
        {
            pair.Value.Invoke(pair.Key);
        }
        Debug.Log("build完成");
    }
}
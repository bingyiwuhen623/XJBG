using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : Singleton<TableManager>
{
    private Dictionary<string, InitLoad> mTableNames;

    public void Init()
    {
        mTableNames = new Dictionary<string, InitLoad>();
        InitName();
    }

    private void InitName()
    {
        AddName("TableItem", TableItem.Me.DeSerializable);
    }

    private void AddName(string tableName, InitLoad action)
    {
        mTableNames.Add(tableName, action);
    }

    public IEnumerator DeSerializableBytes(Action endCallBack)
    {
        foreach (KeyValuePair<string, InitLoad> pair in mTableNames)
        {
            yield return pair.Value.Invoke(pair.Key);
        }
        yield return new WaitForSecondsRealtime(0.1f);
        if (endCallBack != null)
        {
            endCallBack.Invoke();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;

public class TableManager : Singleton<TableManager>
{
    private Dictionary<string, InitLoad> mTableNames;
    private Action m_DeSerializeCallBack = null;

    public void Init()
    {
        mTableNames = new Dictionary<string, InitLoad>();
    }

    private void InitName()
    {
        AddName("TableItem", Table<TableItem>.Me.DeSerializable);
    }

    private void AddName(string tableName, InitLoad action)
    {
        mTableNames.Add(tableName, action);
    }

    public IEnumerator DeSerializableBytes()
    {
        foreach (KeyValuePair<string, InitLoad> pair in mTableNames)
        {
            yield return pair.Value.Invoke(pair.Key);
        }
        if (m_DeSerializeCallBack != null)
        {
            m_DeSerializeCallBack.Invoke();
        }
    }
}
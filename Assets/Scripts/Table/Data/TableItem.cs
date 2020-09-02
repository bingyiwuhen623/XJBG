using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TableItem : Table<TableItemData> { }

[Serializable]
public class TableItemData
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Type { get; set; }
    public int Cost { get; set; }
}
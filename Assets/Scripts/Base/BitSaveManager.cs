using System.Collections.Generic;

/// <summary>
/// 解锁需要弹窗的话，类型加在这里，统一管理
/// </summary>
public enum BitSaveType
{

}

/// <summary>
/// 解锁提示存储管理
/// </summary>
public class BitSaveManager : Singletion<BitSaveManager>
{
    // 64位，每一位存储一个类型对应的值，0或1，多于64位的时候bitSave长度大于1
    private List<long> bitSave;
    private void Init()
    {
        bitSave = new List<long>();
        //bitSave = GameSave.GetInstance().OnlineSave.UnlockTips;
        //Debuger.Log("[BitSaveManager]初始化，list：\n" + bitSave.To_String() + "\n[赵璐]");
    }

    /// <summary>
    /// 获取存储的值
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private long GetValueByType(BitSaveType type)
    {
        int index = (int)type;
        int listIndex = index / 64;
        if (listIndex >= bitSave.Count)
        {
            bitSave.Add(0);
            //GameSave.GetInstance().OnlineSave.BitSave = bitSave;
            return 0;
        }
        else
        {
            return bitSave[index / 64];
        }
    }

    /// <summary>
    /// 获取对应类型在对应值上的索引
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private int GetIndexByType(BitSaveType type)
    {
        int index = (int)type;
        return index % 64;
    }

    /// <summary>
    /// 获取是否展示过
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool GetSave(BitSaveType type)
    {
        long value = GetValueByType(type);
        int index = GetIndexByType(type);
        // 00..00100...0000
        long oneBitValue = 1 << index;
        // 00000...0000000n  n为0或1  bitValue的值只会是0或1
        long bitValue = (value & oneBitValue) >> index;
        return bitValue != 0;
    }

    /// <summary>
    /// 设置解锁值
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="showed">是否展示过</param>
    public void SetSave(BitSaveType type, bool showed)
    {
        long value = GetValueByType(type);
        int index = GetIndexByType(type);
        if (showed)
        {
            // 00..00100...0000
            long oneBitValue = 1 << index;
            // 目的是将对应的位置改为1
            value |= oneBitValue;
        }
        else
        {
            // 11..11011...1111
            long oneBitValue = ~(1 << index);
            // 目的是将对应的位置改为0
            value &= oneBitValue;
        }
        bitSave[(int)type / 64] = value;
        //GameSave.GetInstance().OnlineSave.BitSave = bitSave;
    }
}
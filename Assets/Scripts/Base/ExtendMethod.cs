using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static class Extend
{

    public static PropertyInfo GetInfoByName(Type type, string propertyName)
    {
        var infos = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
        foreach (var item in infos)
        {
            if (item.Name == propertyName)
            {
                return item;
            }
        }
        return null;
    }
    public static FieldInfo GetFieldInfoByName(Type type, string fieldName)
    {
        var infos = type.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
        foreach (var item in infos)
        {
            if (item.Name == fieldName)
            {
                return item;
            }
        }
        return null;
    }
    public static object GetMenberByName(this object obj, string name)
    {
        PropertyInfo[] infos = obj.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
        foreach (PropertyInfo item in infos)
        {
            if (item.Name == name)
            {
                return item.GetValue(obj);
            }
        }
        return null;
    }

    /// <summary>
    /// 从List中获取随机的一项，如果没有内容则返回默认值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="rand"></param>
    /// <returns></returns>
    public static T Random<T>(this IList<T> list, int rand)
    {
        if (list.Count == 0) return default(T);
        return list[rand % list.Count];
    }
    /// <summary>
    /// 从字典的Key中获取随机的一项，value是每个Key的权重
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="rand"></param>
    /// <returns></returns>
    public static T Random_ByWeight<T>(this IDictionary<T, int> dic)
    {
        List<int> wights = new List<int>();
        List<T> keys = new List<T>();

        int sum = 0;
        foreach (var item in dic)
        {
            sum += item.Value;
            wights.Add(sum);
            keys.Add(item.Key);
        }
        int RandomValue = UnityEngine.Random.Range(0, wights[wights.Count - 1]);
        int resultIndex = wights.FindIndex((tmp) => { return tmp > RandomValue; });
        return keys[resultIndex];
    }

    /// <summary>
    /// 从List中获取随机的一项，如果没有内容则返回默认值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="rand"></param>
    /// <returns></returns>
    public static void Insert_Random<T>(this IList<T> list, T item, int rand)
    {
        list.Insert(rand % list.Count, item);
    }

    public static string To_String<T>(this IList<T> list, string separator = "\n")
    {
        string s = "";
        for (int i = 0; i < list.Count; i++)
        {
            if (i < list.Count - 1)
                s += list[i].ToString() + separator;
            else s += list[i].ToString();
        }
        return s;
    }

    public static string To_String<K, V>(this IDictionary<K, V> dict, string kvseparator = ",", string separator = "\n")
    {
        string s = "";
        foreach (var item in dict)
        {
            s += $"({item.Key}{kvseparator}{item.Value})" + separator;
        }
        return s;
    }

    /// <summary>
    /// 随机移除List中的一项
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="rand"></param>
    public static void RemoveRandom<T>(this IList<T> list, int rand)
    {
        if (list.Count == 0) return;
        list.RemoveAt(rand % list.Count);
    }

    /// <summary>
    /// 循环随机移除项，直到剩余n个
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="rand"></param>
    public static void RemoveUtilLast<T>(this IList<T> list, int rand, int n)
    {
        while (list.Count > n)
        {
            list.RemoveAt(rand % list.Count);
        }

    }

    /// <summary>
    /// 往list内填充T，直到final_count项
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="rand"></param>
    /// <returns></returns>
    public static void Fill<T>(this IList<T> list, int final_count, T t = default(T)) where T : struct
    {
        while (list.Count < final_count)
        {
            list.Add(t);
        }
    }

    /// <summary>
    /// 返回离f最近的multiple_of的倍数
    /// </summary>
    /// <param name="f"></param>
    /// <param name="multiple_of"></param>
    /// <returns></returns>
    public static float RoundTo(this float f, float multiple_of)
    {
        return (int)(f / multiple_of + 0.5f) * multiple_of;

    }

    /// <summary>
    /// 返回离f最近的list中的一个数字
    /// </summary>
    /// <param name="f"></param>
    /// <param name="multiple_of"></param>
    /// <returns></returns>
    public static float RoundInto(this float f, params float[] list)
    {
        if (list.Length == 0) return f;
        float min = float.PositiveInfinity;
        int mini = 0;
        for (int i = 0; i < list.Length; i++)
        {
            float diff = Mathf.Abs(list[i] - f);
            if (diff < min)
            {
                min = diff;
                mini = i;
            }
        }
        return list[mini];

    }

    /// <summary>
    /// 得到拥有最小值的那个物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="l"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static T FindMin<T>(this IList<T> l, Func<T, float> func)
    {
        if (l.Count == 0) return default(T);
        float min = float.PositiveInfinity;
        int mini = 0;

        for (int i = 0; i < l.Count; i++)
        {
            float f = func(l[i]);
            if (f < min)
            {
                min = f;
                mini = i;
            }

        }
        return l[mini];
    }

    /// <summary>
    /// 升级版TryGetValue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dict"></param>
    /// <returns></returns>
    public static R TryGetClassEX<R>(this Dictionary<string, object> dict, string key, R default_value = default) where R : class
    {
        if (dict.ContainsKey(key))
        {
            return dict[key] as R;
        }
        return default_value;
    }

    /// <summary>
    /// 升级版TryGetValue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dict"></param>
    /// <returns></returns>
    public static R TryGetStructEX<R>(this Dictionary<string, object> dict, string key, R default_value = default) where R : struct
    {
        if (dict.ContainsKey(key))
        {
            return (R)dict[key];
        }
        return default_value;
    }

    /// <summary>
    /// 将一个统计类的字典转成list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dict"></param>
    /// <returns></returns>
    public static List<T> To_List<T>(this Dictionary<T, int> dict)
    {
        List<T> r = new List<T>();
        foreach (var item in dict)
        {
            for (int i = 0; i < item.Value; i++)
            {
                r.Add(item.Key);
            }
        }
        return r;
    }

    /// <summary>
    /// 将一个统计类的字典转成list
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <param name="dict"></param>
    /// <returns></returns>
    public static List<(K, V)> To_List<K, V>(this Dictionary<K, V> dict)
    {
        List<(K, V)> r = new List<(K, V)>();
        foreach (var item in dict)
        {
            r.Add((item.Key, item.Value));
        }
        return r;
    }

    /// <summary>
    /// 根据一个判断方法来分割成两个集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dict"></param>
    /// <returns></returns>
    public static (List<T>, List<T>) Sperate<T>(this ICollection<T> list, Predicate<T> predicate)
    {
        List<T> yes = new List<T>();
        List<T> no = new List<T>();
        foreach (T item in list)
        {
            (predicate(item) ? yes : no).Add(item);
        }
        return (yes, no);
    }

    /// <summary>
    /// 统计list中项的个数变成
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dict"></param>
    /// <returns></returns>
    public static Dictionary<T, int> To_Dict<T>(this List<T> dict)
    {
        Dictionary<T, int> r = new Dictionary<T, int>();
        foreach (var item in dict)
        {
            if (r.ContainsKey(item))
            {
                r[item] += 1;
            }
            else
            {
                r.Add(item, 1);
            }
        }
        return r;
    }

    /// <summary>
    /// 统计list中项的个数变成
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static Dictionary<T, V> To_Default_Dict<T, V>(this List<T> list)
    {
        Dictionary<T, V> r = new Dictionary<T, V>();
        foreach (var item in list)
        {
            r[item] = default;
        }
        return r;
    }

    /// <summary>
    /// 如果不在场景内，则复制到场景内，否则没有动作，如果null则返回null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T Instantiate_if_not_in_scene<T>(this T obj, Transform parent = null) where T : Component
    {
        if (obj == null) return null;
        return obj.gameObject.scene.name == null ? GameObject.Instantiate(obj, parent) : obj;
    }

    /// <summary>
    /// 获取字典的第N个项
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="dict"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static KeyValuePair<K, V> GetItemAt<K, V>(this Dictionary<K, V> dict, int index)
    {
        int sum = 0;
        foreach (var key in dict)
        {
            sum++;
            if (sum == (index + 1))
                return key;
        }
        return default;
    }


    /// <summary>
    /// 等待m毫秒后调用a方法
    /// </summary>
    /// <param name="a"></param>
    /// <param name="millisecond"></param>
    public static async void Invoke(Action a, int millisecond)
    {
        await Task.Delay(millisecond);
        a?.Invoke();
    }

    /// <summary>
    /// 当一个Task执行完调用
    /// </summary>
    /// <param name="task"></param>
    /// <param name="action"></param>
    public static async void OnComplet(this Task task, Action action)
    {
        await task;
        action?.Invoke();
    }

    /// <summary>
    /// 当一个Task执行完调用
    /// </summary>
    /// <param name="task"></param>
    /// <param name="action"></param>
    public static async void OnComplet<T>(this Task<T> task, Action<T> action)
    {
        action?.Invoke(await task);
    }

    /// <summary>
    /// 等待直到某个表达式为true
    /// </summary>
    /// <param name="func"></param>
    /// <param name="period"></param>
    /// <param name="maxDelayTime">最多等待多长时间，暂定15秒</param>
    /// <returns></returns>
    public static async Task WaitUntil(this Func<bool> func, int period = 10)//, int maxDelayTime = 15000)
    {
        //int thieIndex = index++;
        //Debuger.Log("[WaitUntil]开始等待：" + thieIndex);
        //int num = 0;
        //int hasDelayTime = 0;
        while (func != null && !func())
        {
            //if (printTime < period * num)
            //{
            //    Debuger.Log("[WaitUntil]正在等待：" + thieIndex);
            //}
            //num++;
            await Task.Delay(period);
            //hasDelayTime += period;
            //if (hasDelayTime >= maxDelayTime)
            //{
            //    break;
            //}
        }
        //Debuger.Log("[WaitUntil]结束等待：" + thieIndex);
    }

    public class WaitUntilTimeOut : Exception
    {

    }

    /// <summary>
    /// 用classname创建一个实例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="class_name"></param>
    /// <returns></returns>
    public static T Create_Object_By_Class_Name<T>(string class_name, params object[] parameters) where T : class
    {
        return Activator.CreateInstance(Type.GetType(class_name, true, true), parameters) as T;
    }

    /// <summary>
    /// 扩展 获取变量名称(字符串)
    /// </summary>
    /// <param name="var_name"></param>
    /// <param name="exp"></param>
    /// <returns>return string</returns>
    public static string GetVarName<T>(this T var_name, System.Linq.Expressions.Expression<Func<T, T>> exp)
    {
        return ((System.Linq.Expressions.MemberExpression)exp.Body).Member.Name;
    }

    /// <summary>
    /// 将一组字符串装换成对应的类型实例
    /// 所有的Type需要满足以下两个条件之一
    /// 1，是int，float，double，string中的一个
    /// 2，该Type下定义了public static 'Type' FromString(string)方法
    /// </summary>
    /// <param name="values"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public static List<object> Activate(this IList<string> values, IList<Type> types)
    {
        List<object> parameter_to_use = new List<object>();


        for (int i = 0; i < types.Count; i++)
        {
            if (types[i] == typeof(int))
            {
                parameter_to_use.Add(int.Parse(values[i]));
            }
            else if (types[i] == typeof(float))
            {
                parameter_to_use.Add(float.Parse(values[i]));
            }
            else if (types[i] == typeof(double))
            {
                parameter_to_use.Add(double.Parse(values[i]));
            }
            else if (types[i] == typeof(string))
            {
                parameter_to_use.Add(values[i]);
            }
            else
            {
                parameter_to_use.Add(types[i].GetMethod("FromString", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { values[i] }));
            }
        }
        return parameter_to_use;
    }

    /// <summary>
    /// 递归Find一个子物体
    /// </summary>
    /// <returns></returns>
    public static Transform FindChildByRecursive(this Transform aParent, string aName)
    {
        var result = aParent.Find(aName);
        if (result != null)
            return result;
        foreach (Transform child in aParent)
        {
            result = child.FindChildByRecursive(aName);
            if (result != null)
                return result;
        }
        return null;
    }

    public static string Translate(this string str)
    {
        return str;

    }

    /// <summary>
    /// 获取某个类型的所有子类型
    /// </summary>
    /// <param name="baseType"></param>
    /// <returns></returns>
    public static Type[] GetSubTypes(Type baseType)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly => assembly.FullName.Contains("Assembly"))
            .SelectMany(assembly => assembly.GetTypes()
               .Where(T => (T.IsClass && !T.IsAbstract) && T.IsSubclassOf(baseType))
            ).ToArray();
    }


    /// <summary>
    /// 打印错误，会尝试区分（转场景，关闭unity）等操作带来的错误和真正的错误
    /// </summary>
    public static void PrintError(this MonoBehaviour m, string description, Exception e)
    {
        if (!Application.isPlaying)
        {
            Debuger.Log($"[IgnoredError]（原因：关闭程序）{description}\n{e}");
        }
        else if (m == null || m.gameObject == null)
        {
            Debuger.Log($"[IgnoredError]（原因：切换场景时的未销毁Task）{description}\n{e}");
        }
        else
        {
            Debuger.Log($"[Error]{description}\n{e}");
        }
    }
}
using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using Anole;
using System.Collections.Generic;


/// <summary>
/// 基本扩展类通用型
/// </summary>
public static class HelperBasic
{
    /// <summary>
    /// 查找某个子对象的Buttom，并添加点击事件
    /// </summary>
    /// <param name="r_form"></param>
    /// <param name="r_path"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Button FindOnButton(this Transform r_form, string r_path, UnityAction action)
    {
        if (r_form == null)
        {
            DeBug.LogError($">>>>>>>>>>> 注意：传入 Transform 为空");
            return null;
        }
        Button buttom = r_form.Find(r_path).GetComponent<Button>();
        if (buttom == null)
        {
            Debug.LogError($">>>>>>>>>>> 注意：查找的子对象没有Buttom组件");
            return null;
        }

        buttom?.onClick.AddListener(action);
        return buttom;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="r_form"></param>
    /// <param name="r_path"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Transform FindOnButton2(this Transform r_form, string r_path, UnityAction action)
    {
        return r_form.FindOnButton(r_path, action).transform;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="r_Component"></param>
    /// <param name="r_bool"></param>
    public static void SetActive2(this Component r_Component, bool r_bool)
    {
        if (r_Component != null)
        {
            r_Component.gameObject?.SetActive(r_bool);
        }
    }

    /// <summary>
    /// 查找列表是否有这个元素
    /// 没有就直接添加进去
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="r_list"></param>
    /// <param name="r_item"></param>
    public static void TryAddValue<T>(this List<T> r_list, T r_item)
    {
        if (r_list == null)
        {
            Debug.LogError(">>>>>>>>>> 传入的列表为空！");
            return;
        }
        else
        {
            if (!r_list.Contains(r_item)) r_list.Add(r_item);
        }
    }

    /// <summary>
    /// 查询多个字符串其中一个是否为空或者为Empty
    /// </summary>
    /// <param name="r_str"></param>
    /// <param name="r_ary"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(params string[] r_ary)
    {
        if (r_ary?.Length > 0)
        {
            for (int i = 0; i < r_ary.Length; i++)
            {
                if (string.IsNullOrEmpty(r_ary[i])) return true;
            }
        }
        return false;
    }

}


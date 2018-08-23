using Anole;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class HelperBasicUser
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="r_form"></param>
    /// <param name="r_path"></param>
    /// <param name="action"></param>
    public static ExpandButton FindUserButton(this Transform r_form, string r_path, UnityAction action)
    {
        if (r_form == null)
        {
            DeBug.LogError($">>>>>>>>>>> 注意：传入 Transform 为空");
            return null;
        }
        ExpandButton buttom = r_form.Find(r_path).GetComponent<ExpandButton>();
        if (buttom == null)
        {
            Debug.LogError($">>>>>>>>>>> 注意：查找的子对象没有 UserButton 组件");
            return null;
        }
        else
        {
            buttom?.onClick.AddListener(action);
            return buttom;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <param name="type"></param>
    public static void SetUserType(this ExpandButton user, ExpandButton.UserClickType type)
    {



    }

}

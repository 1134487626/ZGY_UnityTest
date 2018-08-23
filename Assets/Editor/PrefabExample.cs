using UnityEngine;
using UnityEditor;


/// <summary>
/// 批量创建预制体
/// </summary>
public class PrefabExample : EditorWindow
{
    static string savePath = "Assets/Resources/MahJongScene/CardsPool/";

    [MenuItem("Examples/Create Prefab")]
    static void CreatePrefab()
    {
        GameObject[] objectArray = Selection.gameObjects;

        foreach (GameObject item in objectArray)
        {
            //将路径设置为在资产文件夹中，并将其命名为GameObject的名称，并使用.prefab格式
            string localPath = savePath + item.name + ".prefab";
            //AlterObjProperty(item);

            //检查在路径中是否已经存在预装配和/或名称
            if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)))
            {
                //创建系统对话框，询问用户是否确信他们想覆盖现有的预制件
                if (EditorUtility.DisplayDialog("温馨提示：", "预制已经存在，你想要覆盖它吗", "确定", "退出"))
                {
                    CreateNew(item, localPath); //如果用户按下【确定】按钮，则创建预制件
                }
            }
            //如果名称不存在，创建新的预制件
            else
            {
                CreateNew(item, localPath);
            }
        }
    }

    [MenuItem("Examples/Amend Prefab")]
    public static void AmendPrefab()
    {
        GameObject[] tempAry = Resources.LoadAll<GameObject>("MahJongScene/CardsPool/");

        if (tempAry?.Length > 0)
        {
            for (int i = 0; i < tempAry.Length; i++)
            {
                GameObject item = tempAry[i];
                //将路径设置为在资产文件夹中，并将其命名为GameObject的名称，并使用.prefab格式
                string localPath = savePath + item.name + ".prefab";
                AlterObjProperty(item);
                CreateNew(item, localPath); //如果用户按下【确定】按钮，则创建预制件
                Debug.LogError(item);

            }
        }
    }


    // 如果没有选择到目标，就禁用菜单项
    [MenuItem("Examples/Create Prefab", true)]
    static bool ValidateCreatePrefab()
    {
        return Selection.activeGameObject != null;
    }

    /// <summary>
    /// 在指定路径保存这个预制体
    /// </summary>
    static void CreateNew(GameObject obj, string localPath)
    {
        //在给定的路径上创建一个新的预制件，需保存的预制体覆盖这个临时预制体
        Object prefab = PrefabUtility.CreatePrefab(localPath, obj);
        PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
        //Anole.DeBug.Log(obj.name + "  预制体创建成功！");
    }

    static GameObject TingCard = null;

    static void AlterObjProperty(GameObject r_obj)
    {
        if (TingCard == null) { TingCard = Resources.Load<GameObject>(nameof(TingCard)); }

        if (r_obj != null)
        {
            GameObject game = Instantiate(TingCard, r_obj.transform, false);
            game.name = nameof(TingCard);
        }
    }
}

using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.ProjectWindowCallback;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using System.Security.Cryptography;

/// <summary>自定义创建模板</summary>
public class UIItemUtil
{
    #region script Template

    public static string UIItemTemplate_UI = "Template/UI.cs.txt";
    public static string UIItemTemplate_ItemBase = "Template/UIItem.cs.txt";

    [MenuItem("Assets/Create/UIBase", false, 60)]
    public static void CreateUIItemCS()
    {
        CreatetemPlate(Application.dataPath+"/"+ UIItemTemplate_UI, "NewUIBaseScript.cs");
    }

    [MenuItem("Assets/Create/UIItem", false, 62)]
    public static void CreateUIItemCSItem()
    {
        CreatetemPlate(Application.dataPath + "/" + UIItemTemplate_ItemBase, "NewUIBaseScript.cs");
    }




    /// <summary>创建模板   UI </summary>
    /// <param name="templatePath"></param>
    /// <param name="createName"></param>
    private static void CreatetemPlate(string templatePath, string createName)
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
           ScriptableObject.CreateInstance<CreateCSScriptAsset>(),
           GetSelectedPath() + "/" + createName,
           null,
            templatePath);
    }

    private static string GetSelectedPath()
    {
        //默认路径为Assets
        string selectedPath = "Assets";

        //获取选中的资源
        UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);

        //遍历选中的资源以返回路径
        foreach (UnityEngine.Object obj in selection)
        {
            selectedPath = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(selectedPath) && File.Exists(selectedPath))
            {
                selectedPath = Path.GetDirectoryName(selectedPath);
                break;
            }
        }

        return selectedPath;
    }
    #endregion


    class CreateCSScriptAsset : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            UnityEngine.Object o = CreateScriptAssetFromTemplate(pathName, resourceFile);
            ProjectWindowUtil.ShowCreatedAsset(o);
        }

        internal static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
        {

            //获取要创建资源的绝对路径
            string fullName = Path.GetFullPath(pathName);
            //读取本地模版文件
            StreamReader reader = new StreamReader(resourceFile);
            string content = reader.ReadToEnd();
            reader.Close();

            //获取资源的文件名
            string fileName = Path.GetFileNameWithoutExtension(pathName);
            //替换默认的文件名
            content = content.Replace("#NAME", fileName);

            content = content.Replace("#ACTOR", "hc");
            content = content.Replace("#DATE", DateTime.Now.ToLocalTime().ToString());

            //写入新文件
            StreamWriter writer = new StreamWriter(fullName, false, System.Text.Encoding.UTF8);
            writer.Write(content);
            writer.Close();

            //刷新本地资源
            AssetDatabase.ImportAsset(pathName);
            AssetDatabase.Refresh();

            return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
        }

    }

    private static string GetFullPath(string r_parentName, Transform r_select)
    {
        if (r_select == null) return string.Empty;
        StringBuilder sb = new StringBuilder(200);
        sb.Append(r_select.name);
        if (r_select.name == r_parentName)
        {
            return sb.ToString();
        }

        Transform tmp = r_select.parent;
        while (tmp != null && tmp.name != r_parentName)
        {
            if (tmp != null)
            {
                sb.Insert(0, tmp.name + "/");
            }
            tmp = tmp.parent;
        }
        return sb.ToString();
    }

    private static Transform FindTransform(Transform parent, string name)
    {
        if (parent.name == name)
        {
            return parent;
        }
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform trans = FindTransform(parent.GetChild(i), name);
            if (trans != null)
            {
                return trans;
            }
        }
        return null;

    }

    private static Type SelectAllAssembies(string typeName)
    {
        Type t = null;
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type tt = asm.GetType(typeName);
            if (tt != null)
            {
                t = tt;
                break;
            }
        }
        return t;
    }


    [MenuItem("Assets/自动创建UIItem代码 ")]
    private static void Init1()
    {
        Transform parent = Selection.activeTransform;
        string parentName = parent.name;
        Dictionary<string, Transform> allTransform = new Dictionary<string, Transform>();
        GetAllChild(allTransform, parent);
        StringBuilder sbDefine = new StringBuilder(2000);
        StringBuilder sbSelect = new StringBuilder(2000);
        foreach (var item in allTransform)
        {
            string fullName = GetFullPath(parentName, item.Value);
            if (item.Key.StartsWith("auto_img_"))
            {
                sbDefine.AppendLine(string.Format("    private Image {0};", item.Key));
                sbSelect.AppendLine(string.Format("    {0} = Find<Image>(\"{1}\");", item.Key, fullName));
                if (parent.Find(fullName).GetComponent<Image>() == null)
                    Debug.LogError(string.Format(">>>>>>>>  不存在组件 Image [[ {0}]] ", fullName));
            }
            else if (item.Key.StartsWith("auto_txt_"))
            {
                sbDefine.AppendLine(string.Format("    private Text {0};", item.Key));
                sbSelect.AppendLine(string.Format("    {0} = Find<Text>(\"{1}\");", item.Key, fullName));
                if (parent.Find(fullName).GetComponent<Text>() == null)
                    Debug.LogError(string.Format(">>>>>>>> 不存在组件 Text [[ {0}]] ", fullName));
            }
            else if (item.Key.StartsWith("auto_btn_"))
            {
                sbDefine.AppendLine(string.Format("    private Button {0};", item.Key));
                sbSelect.AppendLine(string.Format("    {0} = Find<Button>(\"{1}\");", item.Key, fullName));
                if (parent.Find(fullName).GetComponent<Button>() == null)
                    Debug.LogError(string.Format(">>>>>>>> 不存在组件 Button [[ {0}]] ", fullName));
            }
            else if (item.Key.StartsWith("auto_rect_"))
            {
                sbDefine.AppendLine(string.Format("    private RectTransform {0};", item.Key));
                sbSelect.AppendLine(string.Format("    {0} = Find<RectTransform>(\"{1}\");", item.Key, fullName));
                if (parent.Find(fullName).GetComponent<RectTransform>() == null)
                    Debug.LogError(string.Format(">>>>>>>> 不存在组件 RectTransform [[ {0}]] ", fullName));
            }
        }

        Debug.LogError(">>>>>>>代码copy\n" + sbDefine.ToString());
        Debug.LogError(">>>>>>>代码copy\n" + sbSelect.ToString());

    }


    [MenuItem("Assets/自动创建UI代码 ")]
    private static void Init2()
    {
        Transform parent = Selection.activeTransform;
        string parentName = parent.name;
        Dictionary<string, Transform> allTransform = new Dictionary<string, Transform>();
        GetAllChild(allTransform, parent);
        StringBuilder sbDefine = new StringBuilder(2000);
        StringBuilder sbSelect = new StringBuilder(2000);
        StringBuilder sbBtnOnlick = new StringBuilder(2000);
        foreach (var item in allTransform)
        {
            string fullName = GetFullPath(parentName, item.Value);
            if (fullName.Contains("UIItem"))
            {
                sbDefine.AppendLine(string.Format("    private  GameObject {0};", "Item_"+ item.Key));
                sbSelect.AppendLine(string.Format("    {0} = this.Find<RectTransform>(\"{1}\").gameObject ;", "Item_" + item.Key, fullName));
                sbSelect.AppendLine(string.Format("    {0}.SetActive(false);", "Item_" + item.Key));
                if (parent.Find(fullName).GetComponent<RectTransform>() == null)
                    Debug.LogError(string.Format(">>>>>>>>  不存在组件 Gameobject [[ {0}]] ", fullName));
            }
           else  if (item.Key.StartsWith("auto_img_"))
            {
                sbDefine.AppendLine(string.Format("    private Image {0};", item.Key));
                sbSelect.AppendLine(string.Format("    {0} = this.Find<Image>(\"{1}\");", item.Key, fullName));
                
                if (parent.Find(fullName).GetComponent<Image>() == null)
                    Debug.LogError(string.Format(">>>>>>>>  不存在组件 Image [[ {0}]] ", fullName));
            }
            else if (item.Key.StartsWith("auto_txt_"))
            {
                sbDefine.AppendLine(string.Format("    private Text {0};", item.Key));
                sbSelect.AppendLine(string.Format("    {0} = this.Find<Text>(\"{1}\");", item.Key, fullName));
                if (parent.Find(fullName).GetComponent<Text>() == null)
                    Debug.LogError(string.Format(">>>>>>>> 不存在组件 Text [[ {0}]] ", fullName));
            }
            else if (item.Key.StartsWith("auto_btn_"))
            {
                sbDefine.AppendLine(string.Format("    private Button {0};", item.Key));
                sbSelect.AppendLine(string.Format("    {0} = this.Find<Button>(\"{1}\");", item.Key, fullName));
                sbSelect.AppendLine(string.Format("    REGISTERBUTTON({0});", item.Key));
                    sbBtnOnlick.AppendLine(string.Format("                case \"{0}\":", item.Key));
                sbBtnOnlick.Append(@"               
                {
                }
                break;
");
                if (parent.Find(fullName).GetComponent<Button>() == null)
                    Debug.LogError(string.Format(">>>>>>>> 不存在组件 Button [[ {0}]] ", fullName));
            }
            else if (item.Key.StartsWith("auto_rect_"))
            {
                sbDefine.AppendLine(string.Format("    private RectTransform {0};", item.Key));
                sbSelect.AppendLine(string.Format("    {0} = this.Find<RectTransform>(\"{1}\");", item.Key, fullName));
                if (parent.Find(fullName).GetComponent<RectTransform>() == null)
                    Debug.LogError(string.Format(">>>>>>>> 不存在组件 RectTransform [[ {0}]] ", fullName));
            }
        }

        Debug.LogError(">>>>>>>代码copy\n" + sbDefine.ToString());
        Debug.LogError(">>>>>>>代码copy\n" + sbSelect.ToString());
        Debug.LogError(">>>>>>>代买 事件 \n " + sbBtnOnlick.ToString());

    }


    private static void GetAllChild(Dictionary<string, Transform> content, Transform parent)
    {
        if (parent == null) return;
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            string key = child.name;
            if (key.StartsWith("UIItem"))
            {
                if (!content.ContainsKey(key))
                    content.Add(key, child);
            }
            else
            {
                if (key.StartsWith("auto_"))
                {
                    if (content.ContainsKey(key))
                    {
                        Debug.LogError("包含了相同的名字" + key);
                    }
                    else
                    {
                        content.Add(key, child);
                    }
                }

                GetAllChild(content, child);
            }

        }

    }
    /*
     //加密和解密采用相同的key,可以任意数字，但是必须为32位
    strkeyValue = "12345678901234567890198915689039";
     */
    /// <param name="ContentInfo">要加密内容</param>
    /// <param name="strkey">key值</param>
    /// <returns></returns>
    public string encryptionContent(string ContentInfo, string strkey)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(strkey);
        RijndaelManaged encryption = new RijndaelManaged();
        encryption.Key = keyArray;
        encryption.Mode = CipherMode.ECB;
        encryption.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = encryption.CreateEncryptor();
        byte[] _EncryptArray = UTF8Encoding.UTF8.GetBytes(ContentInfo);
        byte[] resultArray = cTransform.TransformFinalBlock(_EncryptArray, 0, _EncryptArray.Length);
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }
    /// <summary>
    /// 内容解密
    /// </summary>
    /// <param name="encryptionContent">被加密内容</param>
    /// <param name="strkey">key值</param>
    /// <returns></returns>
    public string decipheringContent(string encryptionContent, string strkey)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(strkey);
        RijndaelManaged decipher = new RijndaelManaged();
        decipher.Key = keyArray;
        decipher.Mode = CipherMode.ECB;
        decipher.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = decipher.CreateDecryptor();
        byte[] _EncryptArray = Convert.FromBase64String(encryptionContent);
        byte[] resultArray = cTransform.TransformFinalBlock(_EncryptArray, 0, _EncryptArray.Length);
        return UTF8Encoding.UTF8.GetString(resultArray);
    }
}
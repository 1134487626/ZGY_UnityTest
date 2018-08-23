using UnityEditor;
using UnityEngine;

namespace Game
{
    public class TestWindow1 : EditorWindow
    {
        private string FileName;

        private void OnGUI()
        {
            FileName = EditorGUILayout.TextField("文件前缀名", FileName);

            if (GUILayout.Button("开始重命名文件名"))
            {
                Object[] Objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

                if (Objs != null&&Objs.Length!=0)
                {
                    if (!System.String.IsNullOrEmpty(FileName))
                    {
                        for (int i = 0; i < Objs.Length; i++)
                        {
                            Object obj = Objs[i];
                            string path = AssetDatabase.GetAssetPath(obj);
                            if (Objs[i] == Selection.activeObject)
                            {
                                AssetDatabase.RenameAsset(path, FileName);
                            }
                            else
                            {
                                AssetDatabase.RenameAsset(path, FileName + "_" + i);
                            }
                        }
                        Debug.Log("更改名称成功！");
                    }
                    else
                    {
                        Debug.LogError("文件名不能为空!");
                    }
                }
                else
                {
                    Debug.LogError("所选对象为空");
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SpriteToPrefab : EditorWindow
{
    [MenuItem("Tools/把图片生成对应预设")]
    static void DoIt()
    {
        Object[] objs = Selection.objects;
        if(objs.Length == 0)
        {
            return;
        }

        string imagePath = "Assets/Resources/Image";
        if (!Directory.Exists(imagePath))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Image");
            AssetDatabase.Refresh(ImportAssetOptions.Default);
        }

        for(int i=0; i< objs.Length; i++)
        {
            string assetPath = Application.dataPath;

            string path = AssetDatabase.GetAssetPath(objs[i]);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if(sprite == null)
            {
                Debug.Log(path + "不是图片");
                continue;
            }

            //创建文件夹
            string prefabPath = path.Replace("#Image", "Resources/Image");
            int index = prefabPath.IndexOf("Resources");
            if(index == -1)
            {
                continue;
            }

            string resLength = "Resources/";
            string starPath = prefabPath.Substring(0, index + resLength.Length - 1);
            string endPath = prefabPath.Substring(index + resLength.Length);

            string[] fillNum = endPath.Split('/');
            string addPath = "";
            for (int f = 0; f < fillNum.Length; f++)
            {
                addPath = addPath + "/" + fillNum[f];
                string fillPath = starPath + addPath;

                DirectoryInfo fileInfo = Directory.GetParent(fillPath);
                if (!Directory.Exists(fileInfo.FullName))
                {
                    string fileName = ReplacePath(fileInfo.FullName, imagePath);
                    fileName = fileName.Replace("/" + fileInfo.Name, "");

                    AssetDatabase.CreateFolder(fileName, fileInfo.Name);
                    AssetDatabase.Refresh(ImportAssetOptions.Default);
                }
            }

            prefabPath = prefabPath.Replace(".png", ".prefab");

            GameObject go = new GameObject();
            go.name = objs[i].name;
            SpriteRenderer spriteRender = go.AddComponent<SpriteRenderer>();
            spriteRender.sprite = sprite;

            PrefabUtility.CreatePrefab(prefabPath, go, ReplacePrefabOptions.ReplaceNameBased);
            DestroyImmediate(go);
        }

        AssetDatabase.SaveAssets();
    }

    static string ReplacePath(string fileName, string folderPath)
    {
        fileName = fileName.Replace("\\", "/");

        string assetPath = Application.dataPath;
        fileName = fileName.Replace(assetPath, "Assets");

        return fileName;
    }
}

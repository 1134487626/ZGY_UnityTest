using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class PokerToGameObject : EditorWindow
{
    [MenuItem("Tools/把卡牌生成预设")]
    static void DoIt()
    {
        string folderPath = Application.dataPath;
        string path = folderPath + "/#Image/Poker/";
        if(!Directory.Exists(path))
        {
            Debug.Log("没有路径:" + path);
            return;
        }
        string[] files = Directory.GetFiles(path);
        if (files.Length <= 0)
        {
            Debug.Log("目录下没有文件");
            return;
        }

        string resPath = folderPath + "/Resources/Poker";
        if (!Directory.Exists(resPath))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Poker");
            AssetDatabase.Refresh(ImportAssetOptions.Default);
        }

        for (int i=0; i< files.Length; i++)
        {
            string filePath = files[i].Replace(folderPath, "Assets");
            Sprite poker = AssetDatabase.LoadAssetAtPath<Sprite>(filePath);
            if (poker != null)
            {
                string prefabPath = filePath.Replace("#Image", "Resources");
                prefabPath = prefabPath.Replace(".png", ".prefab");

                GameObject go = new GameObject();
                go.name = poker.name;
                SpriteRenderer spriteRender = go.AddComponent<SpriteRenderer>();
                spriteRender.sprite = poker;

                PrefabUtility.CreatePrefab(prefabPath, go, ReplacePrefabOptions.ReplaceNameBased);
                DestroyImmediate(go);
            }
        }
        
        AssetDatabase.SaveAssets();
    }
}

using UnityEditor;
using System.IO;
using UnityEngine;

public class CreateAssetBundle
{
    readonly static string buildPath = Application.dataPath + "/AssetBundles";

    [MenuItem("Tools/BundleAsset")]
    public static void BundleAssetUnity()
    {
        ////定义文件夹名字
        if (!Directory.Exists(buildPath)) Directory.CreateDirectory(buildPath);

        Object[] ary = Selection.objects;

        for (int i = 0; i < ary.Length; i++)
        {
            Object obj = ary[i];
            if (obj != null)
            {
                AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

                buildMap[0].assetBundleName = obj.name;
                string[] enemyAssets = new string[2];
                enemyAssets[0] = AssetDatabase.GetAssetPath(obj);
                Debug.LogError(enemyAssets[0]);
                buildMap[0].assetNames = enemyAssets;

                BuildPipeline.BuildAssetBundles(buildPath, buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
            }
        }
        //}
    }


    [MenuItem("Tools/BundleAsset", true)]
    static bool FindBundleAsset()
    {
        return Selection.objects != null;
    }

}

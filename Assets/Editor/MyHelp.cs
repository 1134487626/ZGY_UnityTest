using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyHelp : Editor
{
    //public enum Change
    //{
    //    X轴,
    //    Y轴,
    //    Z轴,
    //}

    public static float interval = 0.0018F;
    private static Transform pater;

    //public static int amount = 0;
    //public static Change move = Change.X轴;
    //public static Vector3 orgPos;
    //public static Vector3 engPos;
    //public static string path = "";

    [MenuItem("Tools/AmendPrefab")]
    public static void SortForm()
    {
        GameObject baseObj = new GameObject("Parent");
        Transform form = baseObj.transform;
        GameObject[] objAry = Resources.LoadAll<GameObject>("MahJongScene/CardsPool");
        Material[] matAry = Resources.LoadAll<Material>("MahJongScene/Cards_Material");

        for (int i = 0; i < objAry.Length; i++)
        {
            string objName = objAry[i].name;
            for (int j = 0; j < matAry.Length; j++)
            {
                if (objName == matAry[i].name)
                {
                    GameObject @object = Instantiate(objAry[i], form, false);
                    @object.name = objName;
                    Transform child1 = @object.transform.GetChild(0);
                    Transform child2 = @object.transform.GetChild(1);
                    child1.name = objName + "God of wealth";
                    child2.name = objName + "GangTou";
                    break;
                }
            }
        }
    }

    [MenuItem("Tools/ClearPlayerPrefs")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

}

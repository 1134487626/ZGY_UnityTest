using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 
/// </summary>
public class OpenURLKillRoom : EditorWindow
{
    private readonly string KillRoomURL = "http://39.108.57.130//clean.php#bt/?.cleanThirteen=cleanThirteen/?.cleanShangqiu=cleanShangqiu";

    public enum MYGameRoom
    {
        聚游棋牌,

    }

    [MenuItem("Tools/Kill房")]
    static void ShowKillRoom()
    {
        GetWindow(typeof(OpenURLKillRoom));
    }

    private void OnGUI()
    {

        GUILayout.BeginVertical();

        //绘制当前正在编辑的场景
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 12;
        GUI.skin.label.alignment = TextAnchor.UpperCenter;
        GUILayout.Label(System.DateTime.Now.ToString());

        //绘制描述文本区域
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 12;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label(KillRoomURL);

        //绘制标题
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 15;
        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        GUILayout.Label("请选择游戏 KillRoom");



        //添加名为"Save Bug"按钮，用于调用SaveBug()函数
        if (GUILayout.Button("确认KillRoom"))
        {
            //Application.ExternalCall("cleanThirteen");
            //Application.OpenURL(KillRoomURL);
            Close();
        }

        GUILayout.EndVertical();
    }


}

using UnityEngine;
using UnityEditor;

namespace Game
{
    public class Test1 : Editor
    {
        [MenuItem("Assets/工具集/统一修改文件名称")]
        private static void SetFiledNames()
        {
            EditorWindow.GetWindow<TestWindow1>(true, "统一修改文件名称");
        }
    }
}
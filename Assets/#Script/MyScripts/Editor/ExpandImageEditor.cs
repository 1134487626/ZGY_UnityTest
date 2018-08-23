using UnityEngine.UI;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(ExpandImage), true)]
    [CanEditMultipleObjects]
    public class ExpandImageEditor : ImageEditor
    {
        /// <summary> 切换使用的图片 </summary>
        SerializedProperty m_HandoverSprite;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_HandoverSprite = serializedObject.FindProperty("m_HandoverSprite");

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_HandoverSprite, true);//序列化数组的核心代码 1
            if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();//序列化数组的核心代码 2
            //EditorGUIUtility.LookLikeControls();
            serializedObject.ApplyModifiedProperties();
        }

    }
}


using UnityEngine.UI;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(ExpandButton), true)]
    [CanEditMultipleObjects]
    public class ExpandButtonEditor : ButtonEditor
    {
        /// <summary> 切换使用的图片 </summary>
        SerializedProperty m_HandoverSprite;
        SerializedProperty m_UserClick;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_HandoverSprite = serializedObject.FindProperty("m_HandoverSprite");
            m_UserClick = serializedObject.FindProperty("m_UserClick");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(m_UserClick);//
            EditorGUILayout.PropertyField(m_HandoverSprite);//
            serializedObject.ApplyModifiedProperties();
        }
    }
}

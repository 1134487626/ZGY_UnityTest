using UnityEditor;
using UnityEngine;

public class Test : Editor
{
    private static RectTransform mRectChild;
    private static RectTransform mRevtParent;

    private static float m_xMin;
    private static float m_xMax;
    private static float m_yMin;
    private static float m_yMax;

    private static float m_anchorXmin;
    private static float m_anchorXmax;
    private static float m_anchorYmin;
    private static float m_anchorYmax;

    [MenuItem("Tools/使锚点与当前矩形重合")]
    static void TianRenHeYi()
    {
        if (Selection.activeTransform != null)
        {
            mRectChild = Selection.activeTransform.GetComponent<RectTransform>();
            mRevtParent = mRectChild.parent.GetComponent<RectTransform>();

            m_xMin = mRectChild.localPosition.x - (mRectChild.rect.width / 2);
            m_xMax = mRectChild.localPosition.x + (mRectChild.rect.width / 2);
            m_yMin = mRectChild.localPosition.y - (mRectChild.rect.height / 2);
            m_yMax = mRectChild.localPosition.y + (mRectChild.rect.height / 2);

            m_anchorXmin = (m_xMin - mRevtParent.rect.xMin) / mRevtParent.rect.width;
            m_anchorXmax = (m_xMax - mRevtParent.rect.xMin) / mRevtParent.rect.width;
            m_anchorYmin = (m_yMin - mRevtParent.rect.yMin) / mRevtParent.rect.height;
            m_anchorYmax = (m_yMax - mRevtParent.rect.yMin) / mRevtParent.rect.height;

            mRectChild.anchorMin = new Vector2(m_anchorXmin, m_anchorYmin);
            mRectChild.anchorMax = new Vector2(m_anchorXmax, m_anchorYmax);
            mRectChild.offsetMin = new Vector2(0, 0);
            mRectChild.offsetMax = new Vector2(0, 0);
        }
        else
        {
            Debug.Log("未选中任何物体");
        }
    }
    
}

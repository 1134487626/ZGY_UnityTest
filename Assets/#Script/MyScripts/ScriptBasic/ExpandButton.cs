using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// 自定义的Button
    /// </summary>
    [AddComponentMenu("UI/ExpandButton")]
    public class ExpandButton : Button
    {
        public enum UserClickType
        {
            Normal,
            HandoverSprite,
        }
        /// <summary> 点击时切换使用的图片 </summary>
        [SerializeField] Sprite m_HandoverSprite;//

        [SerializeField] UserClickType m_UserClick;

        /// <summary> 点击时响应的其他对象组件 </summary>
        GameObject handoverGameobj;
        Sprite m_FirstSprite;

        public UserClickType ClickType { get { return m_UserClick; } set { m_UserClick = value; } }

        protected override void Awake()
        {
            if (image != null) m_FirstSprite = image.sprite;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            OnClickFollowUp();
        }

        /// <summary> 点击后的后续回调【实现各种需求】 </summary>
        private void OnClickFollowUp()
        {
            switch (ClickType)
            {
                case UserClickType.Normal:
                    break;
                case UserClickType.HandoverSprite:
                    {
                        if (IsBeImage)
                        {
                            bool isDefault = image.sprite == m_FirstSprite;
                            image.sprite = isDefault ? m_HandoverSprite : m_FirstSprite;
                            if (SetNattiveSize) image.SetNativeSize();
                        }
                    }
                    break;
                default: break;
            }
        }

        /// <summary> 切换Button显示的图片 true 为 默认原始的图片，false 为 HandoverSprite </summary>
        public void ChangTargetSprite(bool r_bool)
        {
            if (IsBeImage)
                image.sprite = r_bool ? m_FirstSprite : m_HandoverSprite;
        }


        #region <UserClickType.HandoverSprite>

        /// <summary> 点击切换图片的时候是否设置图片的默认大小 </summary>
        public bool SetNattiveSize { get; set; } = true;

        private bool IsBeImage => image != null && m_HandoverSprite != null;

        /// <summary>
        /// 直接设置图片
        /// </summary>
        /// <param name="r_bool"> true 为默认的图片   false为需要切换的图片 </param>
        public void SetDefaultSprite(bool r_bool)
        {
            if (IsBeImage)
                image.sprite = r_bool ? m_FirstSprite : m_HandoverSprite;
        }

        #endregion

        /// <summary>
        /// 返回点击时返回改button的状态及可操作的切换对象
        /// </summary>
        /// <param name="r_obj"></param>
        /// <param name="r_bool"> 是否为默认状态（即按钮初始Image的图片，不要为空） </param>
        public delegate void ToPointerClick(GameObject r_obj, bool r_bool);

        public ToPointerClick toPointerClick;

        /// <summary>
        /// 设置切换点击时响应的其他对象组件
        /// 并返回添加点击回调的状态并操作设置的切换对象
        /// </summary>
        /// <param name="r_obj"></param>
        /// <param name="action"></param>
        public void SetHandoverAction(GameObject r_obj, ToPointerClick action)
        {
            if (r_obj != null)
            {
                handoverGameobj = r_obj;
                toPointerClick = action;
            }
        }

        /// <summary>
        /// 添加点击回调的状态并操作已有的切换对象
        /// </summary>
        /// <param name="action"></param>
        public void SetHandoverAction(ToPointerClick action)
        {
            toPointerClick = action;
        }

    }
}

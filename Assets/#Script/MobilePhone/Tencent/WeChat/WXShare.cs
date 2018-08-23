
namespace Tencent.WeChat
{
    /// <summary>
    /// 微信分享JSON类
    /// </summary>
    public class WXShare
    {
        /// <summary> WXShareType </summary>
        public int type;
        /// <summary> 分享文字标题 </summary>
        public string title;
        /// <summary> 分享文字内容 </summary>
        public string description;
        /// <summary> 分享到朋友圈或会话，或者收藏 </summary>
        public int scene;
        /// <summary> 分享的网址 </summary>
        public string webpageUrl;
        /// <summary> 分享的数据 </summary>
        public string thumbData;
    }

    /// <summary>
    /// 分享的对象
    /// </summary>
    public enum WXShareScene
    {
        分享给好友 = 0,
        分享到朋友圈 = 1,
        添加到收藏 = 2,
    }

    /// <summary>
    /// 分享的类型
    /// </summary>
    public enum WXShareType
    {
        ShareText,
        ShareImage,
        ShareMusic,
        ShareVideo,
        ShareUrl,
    }
}

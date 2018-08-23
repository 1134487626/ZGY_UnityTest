

namespace Tencent.WeChat
{
    /// <summary>
    /// 微信登录初始JSON类
    /// </summary>
    public class WXSendAuth
    {
        /// <summary> 回调游戏对象名字</summary>
        public string game;
        /// <summary>回调游戏对象方法名字 </summary>
        public string method;
        /// <summary>
        /// 应用授权作用域，如获取用户个人信息则填写snsapi_userinfo
        /// </summary>
        public string scope;
        public string state;
    }
}

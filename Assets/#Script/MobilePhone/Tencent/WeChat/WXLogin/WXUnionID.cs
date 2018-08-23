
namespace Tencent.WeChat
{
    /// <summary>
    /// 微信用户信息
    /// </summary>
    public class WXUnionID
    {
        /// <summary> 普通用户的标识，对当前开发者帐号唯一 </summary>
        public string openid;
        /// <summary> 普通用户昵称 </summary>
        public string nickname;
        /// <summary> 普通用户性别，1为男性，2为女性 </summary>
        public string sex;
        /// <summary> 普通用户个人资料填写的省份 </summary>
        public string province;
        /// <summary> 普通用户个人资料填写的城市 </summary>
        public string city;
        /// <summary> 国家，如中国为CN </summary>
        public string country;
        /// <summary> 用户头像，
        /// 最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像）
        /// 用户没有头像时该项为空
        /// </summary>
        public string headimgurl;
        /// <summary> 用户特权信息，json数组，如微信沃卡用户为（chinaunicom） </summary>
        public string privilege;
        /// <summary> 用户统一标识。针对一个微信开放平台帐号下的应用，同一用户的unionid是唯一的 </summary>
        public string unionid;
    }
}

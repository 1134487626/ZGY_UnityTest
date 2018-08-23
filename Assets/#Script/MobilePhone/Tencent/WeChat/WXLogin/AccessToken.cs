
namespace Tencent.WeChat
{
    /// <summary>  通过code或者refresh_token 获取access_token  </summary>
    public class AccessToken
    {
        /// <summary> 接口调用凭证 </summary>
        public string access_token;
        /// <summary> access_token接口调用凭证超时时间，单位（秒）</summary>
        public int expires_in;
        /// <summary> 用户刷新access_token </summary>
        public string refresh_token;
        /// <summary> 授权用户唯一标识 </summary>
        public string openid;
        /// <summary> 用户授权的作用域，使用逗号（,）分隔 </summary>
        public string scope;
        /// <summary> ------ </summary>
        public string unionid;
    }
}

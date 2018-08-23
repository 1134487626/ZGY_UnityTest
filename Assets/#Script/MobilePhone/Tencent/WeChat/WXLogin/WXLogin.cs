
namespace Tencent.WeChat
{
    public enum BaseRespErrCode
    {
        /// <summary> 成功 </summary>
        ERR_OK = 0,
        /// <summary> 成功 </summary>
        ERR_COMM = -1,
        /// <summary> 成功 </summary>
        ERR_USER_CANCEL = -2,
        /// <summary> 成功 </summary>
        ERR_SENT_FAILED = -3,
        /// <summary> 成功 </summary>
        ERR_AUTH_DENIED = -4,
        /// <summary> 成功 </summary>
        ERR_UNSUPPORT = -5,
        /// <summary> 成功 </summary>
        ERR_BAN = -6,
    }

    /// <summary> 点击微信登录返回的信息 </summary>
    public class WXLogin
    {
        public int errCode;
        public string respCode;
    }
}

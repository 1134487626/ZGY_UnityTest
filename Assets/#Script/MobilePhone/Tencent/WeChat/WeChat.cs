using System.Collections.Generic;
using System.Text;
using SimpleJson;
using System;

namespace Tencent.WeChat
{
    /// <summary> 微信 </summary>
    public sealed class WeChat
    {
        public const string AppID = "wx3286ad8d7a0b1649";
        public const string AppSecret = "94cc7091ef70c5325f93c43794edba99";

        public static AccessToken Token { get; set; }

        public static WXUnionID UnionID { get; set; }

        private Dictionary<int, WXError> m_DicError;

        public WeChat()
        {
            m_DicError = new Dictionary<int, WXError>();
            Array ary = Enum.GetValues(typeof(WXError));
            foreach (WXError item in ary)
            {
                m_DicError.Add((int)item, item);
            }
        }

        public bool IsError(string r_Data)
        {
            JSONNode node = JSONNode.Parse(r_Data);
            return node["errcode"] != null;
        }

        public WXError ErrCode(int r_code)
        {
            if (m_DicError.ContainsKey(r_code)) return m_DicError[r_code];

            return WXError.未知错误;
        }

        /// <summary> 获取AccessToken链接的网址 </summary>
        public string FullAccessToken(string loginCode)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("https://api.weixin.qq.com/sns/oauth2/access_token?appid=");
            builder.Append(AppID);
            builder.Append("&secret=");
            builder.Append(AppSecret);
            builder.Append("&code=");
            builder.Append(loginCode);
            builder.Append("&grant_type=authorization_code");
            return builder.ToString();
        }

        /// <summary> 刷新AccessToken链接的网址</summary>
        public string FullRefreshToken()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid=");
            builder.Append(AppID);
            builder.Append("&grant_type=refresh_token&refresh_token=");
            builder.Append(Token.refresh_token);
            return builder.ToString();
        }

        /// <summary> 检验授权凭证（access_token）是否有效的网址 </summary>
        public string FullCheckToken()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("https://api.weixin.qq.com/sns/auth?access_token=");
            builder.Append(Token.access_token);
            builder.Append("&openid=");
            builder.Append(Token.openid);
            return builder.ToString();
        }

        /// <summary> 获取用户个人信息（UnionID机制的网址 </summary>
        public string FullUnionID()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("https://api.weixin.qq.com/sns/userinfo?access_token=");
            builder.Append(Token.access_token);
            builder.Append("&openid=");
            builder.Append(Token.openid);
            return builder.ToString();
        }

    }
}


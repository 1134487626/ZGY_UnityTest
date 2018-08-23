
namespace Tencent.WeChat
{
    /// <summary>
    /// 访问微信数据地址
    /// </summary>
    public enum WXUrl
    {
        /// <summary> 获取访问令牌 </summary>
        Access_Token,
        /// <summary> 刷新访问令牌 </summary>
        Refresh_token,
        /// <summary> 检验核对访问令牌 </summary>
        Check_Token,
        /// <summary> 获取用户个人信息（UnionID机制） </summary>
        GetUnionID,
    }
}

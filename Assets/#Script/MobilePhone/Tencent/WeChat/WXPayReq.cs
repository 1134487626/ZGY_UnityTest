
namespace Tencent.WeChat
{
    /// <summary>
    /// 调用微信支付必须参数JSON类
    /// </summary>
    public class WXPayReq
    {
        /// <summary>  </summary>
        public string appid;
        /// <summary> 微信支付分配的商户号 </summary>
        public string partnerid;
        /// <summary> 微信返回的支付交易会话ID </summary>
        public string prepayid;
        /// <summary> 暂填写固定值Sign=WXPay </summary>
        public string packageValue;
        /// <summary> 随机字符串，不长于32位。推荐随机数生成算法 </summary>
        public string noncestr;
        /// <summary> 时间戳 </summary>
        public string timestamp;
        /// <summary> 签名 </summary>
        public string sign;
    }
}

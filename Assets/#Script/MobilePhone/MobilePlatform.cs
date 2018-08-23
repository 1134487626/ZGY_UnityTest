using UnityEngine;
using Anole;
using System.Collections;
using UnityEngine.UI;
using Tencent.WeChat;
using System;

public sealed class MobilePlatform : MonoBehaviour
{
    public static bool IsAndroid => Application.platform == RuntimePlatform.Android;
    public static bool IsIPhone => Application.platform == RuntimePlatform.IPhonePlayer;
    public static AndroidPhone Android { get; private set; }
    public static IOSPhone IPhone { get; internal set; } = new IOSPhone();

    private WeChat weChat = new WeChat();
    private static RawImage rawImage;
    private static Action<Texture> ToImage;

    internal static void OnInit()
    {
        if (!GameObject.Find(nameof(MobilePlatform)))
        {
            GameObject gameObject = new GameObject(nameof(MobilePlatform));
            DontDestroyOnLoad(gameObject);
            gameObject.AddComponent<MobilePlatform>();
            if (IsAndroid) Android = new AndroidPhone();
        }
    }

    public delegate void MobilePhoneDialog(string r_log);

    private void PhoneDebug(string r_log)
    {
        DeBug.Log("您的手机返回Log：" + r_log);
    }

    private void PhoneDialog(string r_log)
    {
        Debug.LogError("您的手机返回错误提示：" + r_log);
    }

    public static bool IsOpenGPS()
    {
        try
        {
            return Android.DevelopUtils.CallStatic<bool>("isOpenGPS");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        return false;
    }

    /// <summary> 设置微信AppIP AppSecret </summary>
    public static void SetIWXAPI()
    {
        try
        {
            if (!HelperBasic.IsNullOrEmpty(WeChat.AppID, WeChat.AppSecret))
            {
                if (IsAndroid)
                {
                    AndroidJavaClass wecht = new AndroidJavaClass("TencentPlayer.WeChat");

                    bool isGG = wecht.CallStatic<bool>("setIWXAPI",
                          WeChat.AppID,
                          WeChat.AppSecret);

                    DeBug.Log($"设置微信AppIP AppSecret 是否成功：{isGG}");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary> 微信登录 </summary>
    public static void SendReqWXLogin()
    {
        try
        {
            if (IsAndroid)
            {
                WXSendAuth sendAuth = new WXSendAuth()
                {
                    game = nameof(MobilePlatform),
                    method = nameof(WXLoginCallback),
                    scope = "snsapi_userinfo",
                    state = "WXSendAuth_test",
                };
                Android.Current.Call("sendReqWXLogin", JsonUtility.ToJson(sendAuth));
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary> 微信支付 </summary>
    public static void SendReqWXPay(WXPayReq r_payReq)
    {
        try
        {
            if (r_payReq != null)
            {
                if (IsAndroid)
                {
                    string payReq = JsonUtility.ToJson(r_payReq);
                    Android.Current.Call("sendReqWXPay", payReq);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// 选择手机相册中的一张图片
    /// </summary>
    /// <param name="r_Message"> 开启相册提示框的文字 </param>
    public static void GetPhotosOne(Action<Texture> action, string r_title = null, string r_Message = null)
    {
        if (IsAndroid)
        {
            ToImage = action;
            Android.ToDevelopUtils(new AlertDialogJson()
            {
                type = (int)AlertDialogType.PHOTOS,
                gameName = nameof(MobilePlatform),
                funcName = nameof(SelectImage),
                message = r_Message,
            });
        }
    }

    /// <summary>
    /// 提示打开摄像头拍照并返回图片路径
    /// </summary>
    /// <param name="r_Message"> 开启摄像头提示框的文字 </param>
    public static void GetTakePhotos(Action<Texture> action, string r_title = null, string r_Message = null)
    {
        if (IsAndroid)
        {
            ToImage = action;
            Android.ToDevelopUtils(new AlertDialogJson()
            {
                type = (int)AlertDialogType.CAMERA,
                gameName = nameof(MobilePlatform),
                funcName = nameof(SelectImage),
                message = r_Message,
            });
        }
    }

    /// <summary> 返回手机相册中某一张图片的路径 </summary>
    private void SelectImage(string r_path)
    {
        if (!string.IsNullOrEmpty(r_path))
        {
            if (!r_path.StartsWith("file://")) r_path = "file://" + r_path;
            StartCoroutine(SelectPhotos(r_path, ToImage));
        }
    }

    /// <summary> 调用微信登录的回调 </summary>
    private void WXLoginCallback(string r_JsonCode)
    {
        if (string.IsNullOrEmpty(r_JsonCode)) Debug.Log("返回数据出错！");
        else
        {
            WXLogin login = JsonUtility.FromJson<WXLogin>(r_JsonCode);
            BaseRespErrCode type = (BaseRespErrCode)login.errCode;
            if (type == BaseRespErrCode.ERR_OK)
            {
                string url = weChat.FullAccessToken(login.respCode);
                StartCoroutine(CallWXHttps(WXUrl.Access_Token, url));
            }
            else
            {
                Debug.LogError($"微信登录出错：{type.ToString()}");
                switch (type)
                {
                    case BaseRespErrCode.ERR_COMM: break;
                    case BaseRespErrCode.ERR_USER_CANCEL: break;
                    case BaseRespErrCode.ERR_SENT_FAILED: break;
                    case BaseRespErrCode.ERR_AUTH_DENIED: break;
                    case BaseRespErrCode.ERR_UNSUPPORT: break;
                    case BaseRespErrCode.ERR_BAN: break;
                }
            }
        }
    }

    /// <summary>
    /// 这几个步骤应该由后端来做
    /// </summary>
    /// <param name="r_type"></param>
    /// <param name="r_Url"></param>
    /// <returns></returns>
    private IEnumerator CallWXHttps(WXUrl r_type, string r_Url)
    {
        DeBug.Log($" 开始CALL ...... {r_type}=> {r_Url}");
        WWW wWW = new WWW(r_Url);
        yield return wWW;
        if (wWW.isDone)
        {
            if (string.IsNullOrEmpty(wWW.error))
            {
                string data = wWW.text;
                bool isWrong = false;
                switch (r_type)
                {
                    case WXUrl.Access_Token:
                    case WXUrl.Refresh_token:
                        isWrong = weChat.IsError(data);
                        if (!isWrong)
                        {
                            WeChat.Token = JsonUtility.FromJson<AccessToken>(data);
                            StartCoroutine(CallWXHttps(WXUrl.Check_Token, weChat.FullCheckToken()));
                        }
                        break;
                    case WXUrl.Check_Token:
                        {
                            WeChatError error = JsonUtility.FromJson<WeChatError>(data);
                            WXError errorType = weChat.ErrCode(error.errcode);
                            isWrong = errorType != WXError.成功;
                            if (!isWrong) StartCoroutine(CallWXHttps(WXUrl.GetUnionID, weChat.FullUnionID()));
                        }
                        break;
                    case WXUrl.GetUnionID:
                        {
                            isWrong = weChat.IsError(data);
                            if (!isWrong) WeChat.UnionID = JsonUtility.FromJson<WXUnionID>(data);
                        }
                        break;
                }


                if (isWrong) Debug.LogError($"微信 {r_type} 返回出错：{data}");
                else
                {
                    DeBug.Log($" 微信 {r_type} 返回正确：{data}");
                }
            }
            else
            {
                Debug.LogError("访问微信Https返回结果出错");
            }
        }
    }

    /// <summary>  </summary>
    public static IEnumerator SelectPhotos(string r_path, Action<Texture> action)
    {
        Debug.Log(" 手机文件路径：  " + r_path);
        WWW www = new WWW(r_path);
        yield return www;

        if (www.isDone)
        {
            if (string.IsNullOrEmpty(www.error))
            {
                ToImage?.Invoke(www.texture);
            }
            else
            {
                Debug.LogError("获取安卓手机图片出错：" + www.error);
            }
        }
    }

    public static bool SaveCopyText(string r_text)
    {
        if (string.IsNullOrEmpty(r_text)) return false;
        else
        {
            try
            {
                if (IsAndroid)
                {
                    Android.DevelopUtils.CallStatic("setCopyText", r_text);
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return false;
        }
    }

    public static void GotoSettingGPS(string r_str)
    {
        if (IsAndroid)
        {
            if (IsAndroid)
            {
                Android.ToDevelopUtils(new AlertDialogJson()
                {
                    type = (int)AlertDialogType.GPS,
                    message = r_str,
                });
            }
        }
    }

    public static void OpenHttpUri(string r_Uri)
    {
        if (string.IsNullOrEmpty(r_Uri)) DeBug.LogRed("注意：输入网址为空");
        else
        {
            if (IsAndroid)
            {
                Android.ToDevelopUtils(new AlertDialogJson()
                {
                    type = (int)AlertDialogType.GPS,
                    message = r_Uri,
                    extend = r_Uri,
                });
            }
            else
            {

            }
        }
    }

    public static string GetCopyText()
    {
        if (IsAndroid)
        {
            return Android.DevelopUtils.CallStatic<string>("getCopyText");
        }
        return null;
    }

    /// <summary> 微信文字分享收藏 </summary>
    public static void SendReqWXShare(WXShare r_Share)
    {
        if (r_Share != null)
        {
            if (IsAndroid)
            {
                string str = JsonUtility.ToJson(r_Share);
                Android.Current.Call("sendReqWXShare", str);
            }
        }
    }


}


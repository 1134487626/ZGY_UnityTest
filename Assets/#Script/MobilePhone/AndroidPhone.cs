using System;
using System.Text;
using UnityEngine;
using System.Security.Cryptography;

/// <summary>
/// Unity 调用 Android 
/// </summary>
public sealed class AndroidPhone
{
    public AndroidPhone()
    {
        if (MobilePlatform.IsAndroid)
        {
            try
            {
                DevelopUtils = new AndroidJavaClass("ASUtils.DevelopUtils");
                UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                CompareSignature("05c18761cf7ac96d5a7f15bee6a1b12c");
                SetDebug("MobilePlatform", "PhoneDebug", "PhoneDialog");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    public AndroidJavaClass DevelopUtils { get; }
    public AndroidJavaClass UnityPlayer { get; }

    /// <summary> 默认调用Unity的AndroidActivity </summary>
    public AndroidJavaObject Current => UnityPlayer?.GetStatic<AndroidJavaObject>("currentActivity");

    /// <summary> 设置安卓可以一直回调给Unity的方法 </summary>
    public void SetDebug(string r_Object, string r_log, string r_pop)
    {
        var temp = new AndroidJavaClass("UnityBasic.Debug");
        temp.CallStatic("setUnityDebug", r_Object, r_log, r_pop);
    }

    /// <summary> 获取安卓包签名 </summary>
    public string GetSignature
    {
        get
        {
            if (MobilePlatform.IsAndroid)
            {
                var PackageManager = new AndroidJavaClass("android.content.pm.PackageManager");
                var packageName = Current.Call<string>("getPackageName");
                var packageManager = Current.Call<AndroidJavaObject>("getPackageManager");
                var GET_SIGNATURES = PackageManager.GetStatic<int>("GET_SIGNATURES");
                var packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, GET_SIGNATURES);
                var signatures = packageInfo.Get<AndroidJavaObject[]>("signatures");

                if (signatures?.Length > 0)
                {
                    try
                    {
                        StringBuilder builder = new StringBuilder();
                        MD5 md5 = new MD5CryptoServiceProvider();
                        byte[] bytes = signatures[0].Call<byte[]>("toByteArray");
                        byte[] retVal = md5.ComputeHash(bytes);

                        for (int i = 0; i < retVal.Length; i++)
                        {
                            builder.Append(retVal[i].ToString("x2"));
                        }
                        return builder.ToString();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(">>>>>>>>>>> 获取安卓签名出错:" + ex.Message);
                    }
                }
            }
            return null;
        }
    }

    /// <summary> 比较安卓包签名 </summary>
    public void CompareSignature(string r_Sign)
    {
        if (MobilePlatform.IsAndroid)
        {
            var androidSign = GetSignature;
            string curSign = "当前应用签名为：" + androidSign;
            string tempSign = "传入应用签名为：" + r_Sign;

            if (string.IsNullOrEmpty(r_Sign)) Debug.LogError($"传入的签名为空，无法比对！\n{curSign}");
            else
            {
                string str = r_Sign == androidSign ? "一致" : "不一致";
                Debug.Log($">>>>>>>>> 比对的签名{str}\n{curSign}\n{tempSign}");
            }
        }
    }

    public void ToDevelopUtils(AlertDialogJson r_v)
    {
        string json = JsonUtility.ToJson(r_v);
        DevelopUtils.CallStatic("showNativeAD", json);
    }


}


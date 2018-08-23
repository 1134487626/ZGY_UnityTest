using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Anole;
using System;
using Tencent.WeChat;

public class CallAndroid : MonoBehaviour
{
    [SerializeField] InputField m_Content;
    [SerializeField] Text m_txtAll;
    [SerializeField] RawImage m_Raw;

    void Start()
    {
        DeBug.Log(JsonUtility.ToJson(new AlertDialogJson()));

        //MobilePlatform.OnInit();
        //AllButtonEvent();
    }

    private void AllButtonEvent()
    {
        transform.FindOnButton("CopyButton", CopyInputFieldText);
        transform.FindOnButton("PasteButton", PasteInputFieldText);
        transform.FindOnButton("btnIsOpenGPS", () =>
        {
            m_txtAll.text = $"是否打开了定位：{MobilePlatform.IsOpenGPS()}";
        });
        transform.FindOnButton("btnOpenGPS", OpenGPS);
        transform.FindOnButton("btnOpenImage", GetPhotosImage);
        transform.FindOnButton("btnOpenUri", OpenUri);
        transform.FindOnButton("btnOpenCamera", OpenCamera);

        transform.FindOnButton("btnSetWeChat", () =>
        {
            MobilePlatform.SetIWXAPI();
        });

        transform.FindOnButton("btnShowHand", () =>
        {
            if (!string.IsNullOrEmpty(WeChat.UnionID.headimgurl))
            {
                StartCoroutine(MobilePlatform.SelectPhotos(WeChat.UnionID.headimgurl, null));
            }
        });

        transform.FindOnButton("btnLoginWeChat", () =>
        {
            MobilePlatform.SendReqWXLogin();
        });

        //transform.FindOnButton("btnLoginWeChat", () =>
        //{
        //    MobilePlatform.SendReqWXPay(null);
        //});

        transform.FindOnButton("btnShareText", () =>
        {
            MobilePlatform.SendReqWXShare(new WXShare()
            {
                type = (int)WXShareType.ShareText,
                scene = (int)WXShareScene.分享给好友,
                title = "测试",
                description = "发达史蒂芬阿萨德股份阿萨德gas的gbdds",
            });
        });

    }

    private void OpenUri()
    {
        MobilePlatform.OpenHttpUri(m_Content.text);
    }

    private void OpenCamera()
    {
        MobilePlatform.GetTakePhotos(TempImg, "打开摄像头拍照并返回");
    }

    private void TempImg(Texture texture)
    {
        m_Raw.texture = texture;
    }

    /// <summary>
    /// 拷贝输入框里的内容
    /// </summary>
    private void CopyInputFieldText()
    {
        MobilePlatform.SaveCopyText(m_Content.text);
    }

    /// <summary>
    /// 将拷贝的内容粘贴到Text
    /// </summary>
    private void PasteInputFieldText()
    {
        m_txtAll.text = MobilePlatform.GetCopyText();
    }

    private void OpenGPS()
    {
        MobilePlatform.GotoSettingGPS(null);
    }

    private void GetPhotosImage()
    {
        MobilePlatform.GetPhotosOne(TempImg, "是否选取相册中的一张图片");
    }




}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// 
/// </summary>
public class AlertDialogJson
{
    /// <summary> 提示类型 </summary>
    public int type;
    /// <summary> 提示头 </summary>
    public string title;
    /// <summary> 提示内容 </summary>
    public string message;
    /// <summary> 扩展信息 </summary>
    public string extend;
    /// <summary> 是否包含取消按钮 </summary>
    public bool isCancel;
    /// <summary> 返回Unity的物体名字 </summary>
    public string gameName;
    /// <summary> 返回Unity的函数名字 </summary>
    public string funcName;
}

public enum AlertDialogType
{
    Node,
    //提示跳转到手机设置定位的界面
    GPS,
    //提示打开相册选取某张图片,并返回给Unity
    PHOTOS,
    //提示打开摄像头拍摄并保存,并返回给Unity
    CAMERA,
    //提示打开默认的浏览器访问某个网址
    BROWSER,
    //提示未安装微信App
    NoWXApp,
}


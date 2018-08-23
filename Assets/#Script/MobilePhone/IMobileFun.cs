
/// <summary>
/// 仅适用移动平台
/// </summary>
public interface IMobileFun
{
    bool IsOpenGPS();

    bool SaveCopyText(string r_str);

    string GetCopyText();

    void GotoSettingGPS(string r_str);

    void OpenHttpUri(string r_Uri);

}


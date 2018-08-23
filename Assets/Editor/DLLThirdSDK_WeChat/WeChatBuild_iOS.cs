using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using UnityEditor.iOS.Xcode;

public class WeChatBuild_iOS
{
	[PostProcessBuild(100)]
    public static void OnPostProcessBuild(BuildTarget r_target, string r_pathToBuiltProject)
    {
        Debug.Log(r_target + "  ,  " + r_pathToBuiltProject);
        switch (r_target)
        {
            case BuildTarget.iOS:
                {
                    OnIOSBuildFinished(r_target, r_pathToBuiltProject);
                }
                break;
            default:
                {
                    //
                }
                break;
        }
    }

    private static void OnIOSBuildFinished(BuildTarget r_target, string r_pathToBuiltProject)
    {
        string projPath = PBXProject.GetPBXProjectPath(r_pathToBuiltProject);
        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromString(File.ReadAllText(projPath));
        string targetGuid = pbxProject.TargetGuidByName("Unity-iPhone");

        //添加它喵的framework
        string[] frameworks = 
        {
            "Security.framework","SystemConfiguration.framework","CoreTelephony.framework","CFNetwork.framework"
        };
        foreach(var framework in frameworks)
        {
            AddFramework(pbxProject, targetGuid, framework, false);
        }

        //添加它喵的lib 
        AddLib(pbxProject, targetGuid, "libsqlite3.tbd"); 
        AddLib(pbxProject, targetGuid, "libc++.tbd"); 
        AddLib(pbxProject, targetGuid, "libz.tbd");

        // 添加flag
        pbxProject.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-Objc -all_load");

        // 应用修改
        File.WriteAllText(projPath, pbxProject.WriteToString());

        // 修改Info.plist文件 
        var plistPath = Path.Combine(r_pathToBuiltProject, "Info.plist"); 
        var plist = new PlistDocument(); 
        plist.ReadFromFile(plistPath);

        var key = plist.root.CreateArray("LSApplicationQueriesSchemes");
        key.AddString("wechat");
        key.AddString("weixin");

        plist.WriteToFile(plistPath);

        string pPath = Application.dataPath.Replace("Assets", string.Empty);
        if(ReplaceFile($"{pPath}XCClasses/UnityAppController.h", $"{r_pathToBuiltProject}/Classes/UnityAppController.h"))
        {
            if(ReplaceFile($"{pPath}XCClasses/UnityAppController.mm", $"{r_pathToBuiltProject}/Classes/UnityAppController.mm"))
            {
                //
            }
            else
            {
                EditorUtility.DisplayDialog("错误", "替换UnityAppController.mm失败，请检查XCClasses文件夹", "ok");
            }
        }
        else
        {
            EditorUtility.DisplayDialog("错误", "替换UnityAppController.h失败，请检查XCClasses文件夹", "ok");
        }
    }

    private static void AddFramework(PBXProject r_pbxProject, string r_targetGuid, string r_framework, bool r_weak)
    {
        if(r_pbxProject.ContainsFramework(r_targetGuid, r_framework) == false)
        {
            r_pbxProject.AddFrameworkToProject(r_targetGuid, r_framework, r_weak);
        }
    }

    private static void AddLib(PBXProject r_pbxProject, string r_targetGuid, string r_lib)
    {
        string fileGuid = r_pbxProject.AddFile($"usr/lib/{r_lib}", $"Frameworks/{r_lib}", PBXSourceTree.Sdk);
        r_pbxProject.AddFileToBuild(r_targetGuid, fileGuid);
    }

    private static bool ReplaceFile(string r_source, string r_to)
    {
        if(File.Exists(r_source))
        {
            byte[] bytes = File.ReadAllBytes(r_source);
            if(bytes != null)
            {
                if(File.Exists(r_to))
                {
                    File.Delete(r_to);
                }
                File.WriteAllBytes(r_to, bytes);
                return true;
            }
        }
        
        return false;
    }
}

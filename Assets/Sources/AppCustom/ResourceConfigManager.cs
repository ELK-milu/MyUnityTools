using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ResourceConfigManager
{
    public static Dictionary<string, string> ResourcePath;

    public static string s_UIManager = "UIManager";
    public static string s_DemoWindow = "DemoWindow";
    public static string s_Step2Window = "Step2Window";
    public static string s_VideoPlayerWindow = "VideoPlayerWindow";

    public static string s_TestUpdateWindow = "TestUpdateWindow";

    public static void Init()
    {
        ResourcePath = new Dictionary<string, string>();

        ResourcePath.Add(s_UIManager, "UI/UIManager");
        ResourcePath.Add(s_DemoWindow, "UI/DemoWindow");
        ResourcePath.Add(s_Step2Window, "UI/Step2Window");

        ResourcePath.Add(s_TestUpdateWindow, "UI/TestUpdateWindow/TestUpdateWindow");

        ResourcePath.Add("VideoPlayerWindow", "UI/Example/VideoPlayerWindow/VideoPlayerWindow");
        ResourcePath.Add("AVVideoPlayerWindow", "UI/Example/AVVideoPlayerWindow/AVVideoPlayerWindow");
        ResourcePath.Add("WelcomeWindow", "UI/Example/WelcomeWindow/WelcomeWindow");

        ResourcePath.Add("PicPlayerWindow", "UI/Example/PicPlayerWindow/PicPlayerWindow");

        ResourcePath.Add("NewVideoPlayerWindow", "UI/Example/NewVideoPlayerWindow/NewVideoPlayerWindow");

        Dictionary<string, string> temp = ResourceConfig.Instance.GetResource();
        foreach (string key in temp.Keys)
        {
            ResourcePath.Add(key, temp[key]);
        }

    }

    public static string GetPath(string uiName)
    {
        if (!ResourcePath.ContainsKey(uiName)) throw new Exception("Find Prefab error :" + uiName );
        return ResourcePath[uiName];
    }
}

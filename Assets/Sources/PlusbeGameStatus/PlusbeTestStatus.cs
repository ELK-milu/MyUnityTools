using Plusbe.AppManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class PlusbeTestStatus : IApplicationStatus
{
    public override void OnEnterStatus()
    {
        //OpenUI<DemoWindow>();

        //OpenUI<VideoPlayerWindow>(); 
        //OpenUI<AVVideoPlayerWindow>();
        OpenUI<NewVideoPlayerWindow>();
        OpenUI<PicPlayerWindow>();

    }
}


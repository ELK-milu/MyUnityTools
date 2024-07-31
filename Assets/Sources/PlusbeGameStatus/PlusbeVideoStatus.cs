using Plusbe.AppManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class PlusbeVideopStatus : IApplicationStatus
{
    public override void OnEnterStatus()
    {
        //OpenUI<AVVideoPlayerWindow>();
        OpenUI<AVVideoPlayerWindow>();
    }
}


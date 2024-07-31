using Plusbe.AppManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class FirstStatus : IApplicationStatus
{
    public override void OnEnterStatus()
    {
        OpenUI<DemoWindow>();
    }
}


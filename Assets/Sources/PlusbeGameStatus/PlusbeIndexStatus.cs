using Plusbe.AppManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class PlusbeIndexStatus : IApplicationStatus
{
    public override void OnEnterStatus()
    {
        //OpenUI<DemoWindow>();
        OpenUI<TestUpdateWindow>();
    }
}


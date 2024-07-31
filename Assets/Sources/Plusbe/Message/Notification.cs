using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Plusbe.Message
{
    public class Notification
    {
        public Component sender;

        public string name;

        public object data;

        public Notification(Component aSender, string aName)
        {
            this.sender = aSender;
            this.name = aName;
            this.data = null;
        }

        public Notification(Component aSender, string aName, object aData)
        {
            this.sender = aSender;
            this.name = aName;
            this.data = aData;
        }
    }
}

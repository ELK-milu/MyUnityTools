using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Plusbe.Helper
{
    public class ResourceGCHelper
    {
        public static void GC()
        {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
    }
}

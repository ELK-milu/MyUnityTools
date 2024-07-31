using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plusbe.Helper
{
    /// <summary>
    /// 按照系统文件排序顺序进行排序， 
    /// </summary>
    public class ChineseSortHelper
    {
        public static void Test()
        {
            //中文排序，系统排序
            string[] names = { "1", "3", "22", "4", "2", "中兴.png", "中电万维.png", "爱奇艺.png", "honor.png", "英普瑞生.png" };
            //names = PlusbeHelper.PathHelper.GetFileImage(GlobalSetting.NikonPath);

            names = SortArray(names);
        }
        private static string[] SortArray(string[] names)
        {
            Array.Sort(names, new MyDateSorter2());

            return names;
        }
    }

    #region IComparer Members   
    public class MyDateSorter2 : IComparer<string>
    {
        [System.Runtime.InteropServices.DllImport("Shlwapi.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern int StrCmpLogicalW(string param1, string param2);

        public int Compare(string x, string y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }

            return StrCmpLogicalW(x.ToString(), y.ToString());

            //return x.CompareTo(y);
        }
    }

    #endregion
}

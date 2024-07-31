/*
*┌────────────────────────────────────────────────┐
*│　描    述：CodeStatus                                                    
*│　作    者：Kimi                                          
*│　版    本：1.0                                              
*│　创建时间：2020/2/25 15:12:24                        
*└────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plusbe.Utils
{
    public class CodeStatus
    {
        public DateTime dateTime;
        public int code;
        public string message;
        public string brief;

        public CodeStatus(CodeCode code, string msg, string brief = "")
        {
            this.code = Convert.ToInt32(code);
            this.message = msg;
            this.brief = brief;
            this.dateTime = DateTime.Now;
        }
    }

    public enum CodeCode
    {
        Success = 0,

        Warning = 1,

        Error = 2,
    }
}

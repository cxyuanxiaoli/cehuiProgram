using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duanmian2
{
    class MyLog
    {
        static StringBuilder log = new StringBuilder();
        static int i = 1;
        public static string Log
        {
            get
            {
                return log.ToString();
            }
        }
        public static void Add(string info)
        {
            log.AppendLine((i++)+","+info);
        }


    }
}

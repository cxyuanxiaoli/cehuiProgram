using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dadixian
{
    class MyLog
    {
        //报告类
        static StringBuilder log=new StringBuilder();
        public static string Log
        {
            get
            {
                return log.ToString();
            }
        }

        public static void Add(string info)
        {
            log.AppendLine(info);
        }
    }
}

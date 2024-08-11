using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WudianNihe2
{
    //报告类
    class MyLog
    {
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
        public static void Clear()
        {
            log.Clear();
        }
    }
}

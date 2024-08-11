using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialExplore
{
    class MyLog
    {
        //报告类
        static int i = 1;
        static StringBuilder log = new StringBuilder("序号,说明,计算结果\n");
        public static string Log
        {
            get
            {
                return log.ToString();
            }
        }

        public static void Add(string info)
        {
            log.AppendLine((i++).ToString() + "," + info);
        }
    }
}

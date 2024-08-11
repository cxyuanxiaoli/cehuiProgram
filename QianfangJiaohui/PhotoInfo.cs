using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QianfangJiaohui
{
    class PhotoInfo
    {
        //内方位元素
        public double f;
        public double x;
        public double y;
        //外方位元素
        public double xs;
        public double ys;
        public double zs;
        public double _4;
        public double _w;
        public double _k;

        public PhotoInfo(double f,double x,double y,double xs,double ys,double zs,double _4,double _w,double _k)
        {
            this.f = f;
            this.x = x;
            this.y = y;
            this.xs = xs;
            this.ys = ys;
            this.zs = zs;
            this._4 = _4;
            this._w = _w;  
            this._k = _k;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FZYK.Com
{
    public  class YKParams<T,M>
    {
        public YKParams(T t, M m)
        {
            _t = t;
            _m = m;
        }
        private T _t;
        private M _m;
        public T GetT
        {
            get { return _t; }
            set { _t = value; }
        }
        public M GetM
        {
            get { return _m; }
            set { _m = value; }
        }
    }

  
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FZYK.Com.Log
{
    public interface IYKLog
    {
        void Info(string Content, string op_type, string P_KeyName, string P_KeyValue);
        void Error(Exception error);
    }
}

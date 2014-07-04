﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jinritoutiao.Core
{
    public class ReceiveResult
    {
        private string _message;                // 返回结果
        private List<ReceiveResult> _data;      // 返回数据
        private Next _next;
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public List<ReceiveResult> Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public Next Next
        {
            get { return _next; }
            set { _next = value; }
        }
    }
}

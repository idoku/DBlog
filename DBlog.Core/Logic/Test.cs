﻿using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBlog.Core.Logic
{
    public class Test : ITest
    {
        private static ITest _instance;
        public static ITest CreateInstance()
        {
            IApplicationContext context = ContextRegistry.GetContext();
            if (_instance == null && context != null)
                _instance = context.GetObject("ITest") as ITest;
            return _instance;
        }

        void ITest.Test(string a)
        {
            decimal b = 1 + 2;

        }
    }
}

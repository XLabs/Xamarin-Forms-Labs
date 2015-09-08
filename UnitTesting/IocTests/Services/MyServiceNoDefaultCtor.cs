using System;
using System.Collections.Generic;
using System.Text;

namespace IocTests
{
    public class MyServiceNoDefaultCtor : IMyService
    {
        public MyServiceNoDefaultCtor(IService service)
        {
            this.Service = service;
        }

        public IService Service
        {
            get;
            private set;
        }
    }
}

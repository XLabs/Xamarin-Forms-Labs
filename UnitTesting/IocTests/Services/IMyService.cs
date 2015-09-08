using System;
using System.Collections.Generic;
using System.Text;

namespace IocTests
{
    public interface IMyService
    {
        IService Service { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheritTest
{
    public class SubTestClass : BaseTestClass
    {
        public override void DoSomething()
        {
            base.DoSomething();
            Count--;
        }

        public override void DoSomethingAfter()
        {
            base.DoSomethingAfter();
        }
    }
}

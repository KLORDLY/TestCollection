using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheritTest
{
    public class BaseTestClass
    {

        int count = 0;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public virtual void DoSomething()
        {
            DoSomethingAfter();
        }

        public virtual void DoSomethingAfter()
        {
            count++;
        }
    }
}

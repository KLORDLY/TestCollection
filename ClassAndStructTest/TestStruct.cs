using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassAndStructTest
{
    public struct TestStruct
    {
        TestSubClass _testSubClass;

        public TestSubClass TestSubClass
        {
            get { return _testSubClass; }
            set { _testSubClass = value; }
        }

    }
}

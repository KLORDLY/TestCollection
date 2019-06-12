using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassAndStructTest
{
    public class TestClass
    {
        //TestSubClass _testSubClass;

        public TestSubClass TestSubClass;
        //{
        //    get { return _testSubClass; }
        //    set { _testSubClass = value; }
        //}

        public TestClass()
        {
            TestSubClass = new TestSubClass();


        }
    }
}

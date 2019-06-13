using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaticConstructorTest
{
    public class TestClassB
    {
        public static readonly int y = TestClassA.x + 1;
        static TestClassB()
        {
            //x = TestClassB.y + 2;
        }
    }
}

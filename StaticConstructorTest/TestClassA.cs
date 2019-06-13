using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaticConstructorTest
{
    public class TestClassA
    {
        public static readonly int x = 1;
        static TestClassA()
        {
            x = TestClassB.y + 2;
        }
    }
}

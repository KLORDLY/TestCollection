using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassAndStructTest
{
    public struct TestSubClass
    {
        int testField;

        public int TestField
        {
            get { return testField; }
            set { testField = value; }
        }

        public void TestFunc()
        {
            testField++;
        }

        public string Output()
        {
            return testField.ToString();
        }
    }
}

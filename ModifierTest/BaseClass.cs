using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModifierTest
{
    public abstract class BaseClass
    {
        string name = "张三";

        public string Name
        {
            protected get { return name; }
            set { name = value; }
        }

        public abstract string GetName(ClassB classB);

        int number = 001;

        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        public abstract int GetNumber();
    }
}

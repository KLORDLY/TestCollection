using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModifierTest
{
    public class ClassA : BaseClass
    {
        public override string GetName(ClassB classB)
        {
            //return classB.Name;  //反例1：无法调用
            return Name;
        }

        private int number=003;

        public new int Number
        {
            get { return number; }
            set { number = value; }
        }

        public override int GetNumber()
        {
            return number;
        }
    }
}

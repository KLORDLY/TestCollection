using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModifierTest
{
    public class ClassB : BaseClass
    {
        public override string GetName(ClassB classB)
        {
            return classB.Name;
        }

        public override int GetNumber()
        {
            return Number;
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public int G(BaseClass baseClass)//这里的baseClass必定是BaseClass子类的对象
        {
            //return baseClass.Number;  //这里的baseClass.Number无论子类是否覆盖，都是BassClass的属性
            return baseClass.GetNumber(); //这里的baseClass.GetNumber(),如果父类BaseClass没有实现，则为所传参数的方法，反之，为父类的方法。
        }
    }
}

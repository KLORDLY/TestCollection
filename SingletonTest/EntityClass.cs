using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingletonTest
{
    public class EntityClass
    {
        static EntityClass[] entity = null;

        public static EntityClass[] Entity
        {
            get
            {
                if (entity == null)
                {
                    entity = new EntityClass[] { new EntityClass(), new EntityClass() };
                }
                return EntityClass.entity;
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

    }
}

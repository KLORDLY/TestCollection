using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoTypeTest
{
    class Attachment
    {
        private String name; //附件名
        public void setName(String name)
        {
            this.name = name;
        }
        public String getName()
        {
            return this.name;
        }
        public void download()
        {
            //System.out.println("下载附件，文件名为" + name);
        }
    }
}

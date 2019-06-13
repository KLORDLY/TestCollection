using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProtoTypeTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            WeeklyLog log_previous, log_new;
            log_previous = new WeeklyLog(); //创建原型对象
            Attachment attachment = new Attachment(); //创建附件对象
            log_previous.setAttachment(attachment);  //将附件添加到周报中
            log_new = (WeeklyLog)log_previous.Clone(); //调用克隆方法创建克隆对象
            //比较周报
            MessageBox.Show(string.Format("周报是否相同？{0}", log_previous == log_new));
            //比较附件
            MessageBox.Show(string.Format("附件是否相同？{0}", log_previous.getAttachment() == log_new.getAttachment()));
        }
    }
}

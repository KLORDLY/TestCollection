using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepClone
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            WeeklyLog log_previous, log_new = null;
            log_previous = new WeeklyLog(); //创建原型对象
            Attachment attachment = new Attachment(); //创建附件对象
            log_previous.setAttachment(attachment);  //将附件添加到周报中
            try
            {
                //调用深克隆方法创建克隆对象 
                log_new = log_previous.DeepClone(); 
                //log_new = log_previous.DeepCopyWithReflection<WeeklyLog>(log_previous);
                //log_new = log_previous.DeepCopyWithXmlSerializer();
            }
            catch (Exception e)
            {
                //System.err.println("克隆失败！");
            }
            //比较周报
            MessageBox.Show(string.Format("周报是否相同？{0}", log_previous == log_new));
            //比较附件
            MessageBox.Show(string.Format("附件是否相同？{0}", log_previous.getAttachment() == log_new.getAttachment()));
        }
    }
}

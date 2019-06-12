using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassAndStructTest
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.ContextMenu contextMenu = new ContextMenu();
        public Form1()
        {
            InitializeComponent();

            //TestStruct testStruct = new TestStruct();
            //testStruct.TestSubClass = new TestSubClass();
            //ExecuteTest(testStruct);
            //this.label1.Text = testStruct.TestSubClass.Output();

            TestClass testClass = new TestClass();

            ExecuteTest(testClass);
            this.label2.Text = testClass.TestSubClass.Output();
        }

        public void ExecuteTest(TestStruct testStruct)
        {
            //testStruct.TestSubClass.TestField++;//.TestFunc();
            //testStruct.TestSubClass;//.TestFunc();
        }

        public void ExecuteTest(TestClass testClass)
        {
          TestSubClass t=  testClass.TestSubClass;
          t.TestFunc();
            testClass.TestSubClass.TestFunc();
        }

        private void Form1_ContextMenuStripChanged(object sender, EventArgs e)
        {
            contextMenu.Show((Control)sender, new Point(0, 0));
        }
    }
}

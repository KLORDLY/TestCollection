using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ObjRefTest
{
    public partial class Form2 : Form
    {
        public event OnTimeChangedEventHandler _OnTimeChangedEvent = null;
        public Form2(ref OnTimeChangedEventHandler OnTimeChangedEvent)
        {
            InitializeComponent();
            OnTimeChangedEvent += Form2__OnTimeChangedEvent;
        }

        void Form2__OnTimeChangedEvent()
        {
            //
            if (_OnTimeChangedEvent != null)
                _OnTimeChangedEvent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (Form3 f3 = new Form3(ref _OnTimeChangedEvent))
            {
                f3.ShowDialog();
            }
        }

    }
}

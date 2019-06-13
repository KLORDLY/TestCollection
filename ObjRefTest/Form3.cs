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
    public partial class Form3 : Form
    {
        public event OnTimeChangedEventHandler _OnTimeChangedEvent = null;
        public Form3()
        {
            InitializeComponent();
        }

        public Form3(ref OnTimeChangedEventHandler OnTimeChangedEvent)
        {
            InitializeComponent();
            _OnTimeChangedEvent += Form3__OnTimeChangedEvent;
            OnTimeChangedEvent += _OnTimeChangedEvent;
        }

        void Form3__OnTimeChangedEvent()
        {

        }
    }
}

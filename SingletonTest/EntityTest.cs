using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SingletonTest
{
    public partial class EntityTest : Form
    {
        public EntityTest()
        {
            InitializeComponent();
        }

        private void btnSetEntity1_Click(object sender, EventArgs e)
        {
            EntityClass.Entity[0].Name = "实例1";
        }

        private void btnEntity2_Click(object sender, EventArgs e)
        {
            EntityClass.Entity[1].Name = "实例2";
        }

        private void btnEntities_Click(object sender, EventArgs e)
        {
            EntityClass[] c = EntityClass.Entity;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SymmetricCryptoHelperTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string key = "secret key";
            string plainText = "Hello, readers";

            string encryptedText = SymmetricCryptoHelper.Encrypt(plainText, key);

            string clearText = SymmetricCryptoHelper.Decrypt(encryptedText, key);

            textBox1.Text = encryptedText + "\r\n" + clearText;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace RSACryptoHelperTest
{
    public partial class Form1 : Form
    {
        RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
        string privateKey = string.Empty;
        string publicKey = string.Empty;
        string plainText = string.Empty;
        public Form1()
        {
            InitializeComponent();
            privateKey = provider.ToXmlString(true);
            publicKey = provider.ToXmlString(false);
            plainText = "Hello, coders";
        }

        private void btnMode1_Click(object sender, EventArgs e)
        {
            string encrytedText = RSACryptoHelper.Encrypt(publicKey, plainText);
            txtBoxResult.Text = encrytedText + "\r\n";

            string clearText = RSACryptoHelper.Decrypt(privateKey, encrytedText);
            txtBoxResult.Text += clearText;
        }

        private void btnMode2_Click(object sender, EventArgs e)
        {
            string signedDigest = RSACryptoHelper.SignData(plainText, privateKey);
            txtBoxResult.Text = signedDigest + "\r\n";

            bool isCorrect = RSACryptoHelper.VerifyData(plainText, signedDigest, publicKey);
            txtBoxResult.Text += isCorrect.ToString();
        }
    }
}

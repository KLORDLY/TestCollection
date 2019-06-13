using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UTF_8Test
{
    /*
     *两个文本对比，
     * 反映利用UTF-16编码的英文字符不能利用UTF-8解码的原因：UTF-16编码的英文字符多余了高位0X00
     */
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string inputText = "Hello C#程序员";
        byte[] inputBuffer = new byte[0];
        private void btnUTF8FromString_Click(object sender, EventArgs e)
        {
            inputBuffer = System.Text.UTF8Encoding.UTF8.GetBytes(inputText);
            textBox1.Text = "UTF8编码完成";
        }

        private void btnUTF8ToString_Click(object sender, EventArgs e)
        {
            if (inputBuffer.Length != 0)
            {
                byte[] newBuffer = new byte[inputBuffer.Length];
                int j = 0;
                for (int i = 0; i < inputBuffer.Length; i++)
                {
                    if (inputBuffer[i] != 0)
                    {
                        newBuffer[j++] = inputBuffer[i];
                    }
                }
                textBox1.Text = System.Text.UTF8Encoding.UTF8.GetString(inputBuffer);
                
                //textBox1.Text += "\r\n" + System.Text.UTF8Encoding.UTF8.GetString(newBuffer);
            }
        }

        private void btnUTF16FromString_Click(object sender, EventArgs e)
        {
            inputBuffer = System.Text.UnicodeEncoding.Unicode.GetBytes(inputText);
            textBox1.Text = "UTF16编码完成";
        }

        private void btnUTF16ToString_Click(object sender, EventArgs e)
        {
            if (inputBuffer.Length != 0)
            {
                textBox1.Text = System.Text.UnicodeEncoding.Unicode.GetString(inputBuffer);
            }
        }
    }
}

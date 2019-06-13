using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Student stu = new Student();
            int studentSize = Marshal.SizeOf(stu);//2640
            Classs cla = new Classs();
            int classsSize = Marshal.SizeOf(cla);//13208
        }
    }
    public struct Student
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        char[] name;
        int age;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        double[] scores;
    };

    // Class中包含结构体数组类型  
    public struct Classs
    {
        int number;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        Student[] students;
    };
}

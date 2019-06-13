using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsPingTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //Ping ping = new Ping();
            //PingReply pingReply = ping.Send("192.168.40.106");
            //if (pingReply.Status == IPStatus.Success)
            //{
            //    MessageBox.Show("success!");
            //}
            //else
            //{
            //    MessageBox.Show("failed!");
            //}

            IPAddress ip = new IPAddress(new byte[] { 192, 168, 0, 106 });

            byte[] RequestData = Encoding.ASCII.GetBytes(new string('\0', 64));

            //Allocate ICMP_ECHO_REPLY structure
            ICMP_ECHO_REPLY reply = new ICMP_ECHO_REPLY(255);
            reply.DataSize = 255;
            IntPtr pData = LocalAlloc(LMEM_ZEROINIT, reply.DataSize);
            reply.Data = pData;
            IntPtr h = IcmpCreateFile();
            int dwErr1 = 0;
            dwErr1 = GetLastError();
            uint ipaddr = (uint)ip.Address;

            for (int i = 0; i < 4; i++)
            {
                uint ret = IcmpSendEcho(h, ipaddr, RequestData, (short)RequestData.Length, IntPtr.Zero, reply._Data, reply._Data.Length, 1000);
                int dwErr = 0;
                if (ret == 0)
                {
                    dwErr = GetLastError();
                    if (dwErr != 11010) // If error is other than timeout - display a message
                        MessageBox.Show("Failed to ping. Error code: " + dwErr.ToString());
                }
                if (dwErr != 11010)
                    MessageBox.Show(string.Format("RTT: {0}, Data Size: {1}; TTL: {2}", reply.RoundTripTime, reply.DataSize, reply.Ttl));
                    //liPing.TailText = string.Format("RTT: {0}, Data Size: {1}; TTL: {2}", reply.RoundTripTime, reply.DataSize, reply.Ttl);
                else
                    MessageBox.Show("Request timed out");
                    //liPing.TailText = "Request timed out";
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
            }
            bool b = IcmpCloseHandle(h);
            LocalFree(reply.Data);
        }


        #region Memory Management

        [DllImport("Kernel32.dll")]
        public static extern IntPtr LocalAlloc(int flags, int size);
        [DllImport("Kernel32.dll")]
        public static extern IntPtr LocalFree(IntPtr pMem);

        const int LMEM_ZEROINIT = 0x40;

        #endregion


        #region IPHLPAPI P/Invokes
        [DllImport("iphlpapi.dll", EntryPoint = "IcmpCreateFile", SetLastError = false)]
        public static extern IntPtr IcmpCreateFile();

        [DllImport("iphlpapi.dll", EntryPoint = "IcmpCloseHandle", SetLastError = false)]
        public static extern bool IcmpCloseHandle(IntPtr h);

        [DllImport("iphlpapi.dll", EntryPoint = "IcmpSendEcho", SetLastError = false)]
        public static extern uint IcmpSendEcho(
                         IntPtr IcmpHandle,
                         uint DestinationAddress,
                         byte[] RequestData,
                         short RequestSize,
                         IntPtr /*IP_OPTION_INFORMATION*/ RequestOptions,
                         byte[] ReplyBuffer,
                         int ReplySize,
                         int Timeout);

        #endregion

        [DllImport("Kernel32.dll")]
        public static extern int GetLastError();
    }
    public class ICMP_ECHO_REPLY
    {
        public ICMP_ECHO_REPLY(int size) { data = new byte[size]; }
        byte[] data;
        public byte[] _Data { get { return data; } }
        public int Address { get { return BitConverter.ToInt32(data, 0); } }
        public int Status { get { return BitConverter.ToInt32(data, 4); } }
        public int RoundTripTime { get { return BitConverter.ToInt32(data, 8); } }
        public short DataSize { get { return BitConverter.ToInt16(data, 0xc); } set { BitConverter.GetBytes(value).CopyTo(data, 0xc); } }
        public IntPtr Data { get { return new IntPtr(BitConverter.ToInt32(data, 0x10)); } set { BitConverter.GetBytes(value.ToInt32()).CopyTo(data, 0x10); } }
        public byte Ttl { get { return data[0x14]; } }
        public byte Tos { get { return data[0x15]; } }
        public byte Flags { get { return data[0x16]; } }
        public byte OptionsSize { get { return data[0x17]; } }
        public IntPtr OptionsData { get { return new IntPtr(BitConverter.ToInt32(data, 0x18)); } set { BitConverter.GetBytes(value.ToInt32()).CopyTo(data, 0x18); } }
    }
}

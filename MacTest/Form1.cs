using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MacTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGetMac_Click(object sender, EventArgs e)
        {
            uint ip = 0;
            string mac = string.Empty;
            IPAddress[] ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            if (ips != null && ips.Length > 0)
            {
                //获取本机IP
                byte[] ipp = ips[3].GetAddressBytes();
                ipp = IPAddress.Loopback.GetAddressBytes();
                ip = (uint)((ipp[0]) | (ipp[1] << 8) | (ipp[2] << 16) | (ipp[3] << 24));
                //MessageBox.Show(string.Format("{0}.{1}.{2}.{3}", ipp[0], ipp[1], ipp[2], ipp[3]));
                //获取MAC地址.
                byte[] MacAddr = new byte[6];
                uint PhyAddrLen = 6;
                uint arp = Test.SendARP(ip, 0, MacAddr, ref PhyAddrLen);
                //MessageBox.Show(string.Format("arp = {0}", arp));
                if (arp == 0)
                {
                    mac = string.Format("{0:X2}:{1:X2}:{2:X2}:{3:X2}:{4:X2}:{5:X2}", MacAddr[0], MacAddr[1], MacAddr[2], MacAddr[3], MacAddr[4], MacAddr[5]);
                }
            }
        }
        public static NetworkInterface[] NetCardInfo()
        {
            return NetworkInterface.GetAllNetworkInterfaces();
        }

        ///<summary>
        /// 通过NetworkInterface读取网卡Mac
        ///</summary>
        ///<returns></returns>
        public static List<string> GetMacByNetworkInterface()
        {
            List<string> macs = new List<string>();
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                macs.Add(ni.GetPhysicalAddress().ToString());
            }
            return macs;
        }
        
        
        private void btnGetMac2_Click(object sender, EventArgs e)
        {
            GetMacByNetworkInterface();
        }

        ///<summary>
        /// 通过WMI读取系统信息里的网卡MAC
        ///</summary>
        ///<returns></returns>
        public static List<string> GetMacByWMI()
        {
            List<string> macs = new List<string>();
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        mac = mo["MacAddress"].ToString();
                        macs.Add(mac);
                    }
                }
                moc = null;
                mc = null;
            }
            catch
            {
            }

            return macs;
        }

        private void btnGetMac3_Click(object sender, EventArgs e)
        {
            GetMacByWMI();
        }

    }

    public class Test
    {

        [DllImport("Iphlpapi.dll", EntryPoint = "SendARP")]
        public static extern uint SendARP(uint DestIP, uint SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);

        [DllImport("Iphlpapi.dll", EntryPoint = "SendARP")]
        public static extern UInt32 GetAdaptersInfo(byte[] pAdapterInfo, ref uint pOutBufLen);
    }

}

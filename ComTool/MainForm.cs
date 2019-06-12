using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Xml;

namespace ComTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        #region 构造窗口函数和实时获取串口线程

        Xmas11.Comm.Core.ICommClient ClientInstance = null;
        //iComm.IComm ClientInstance = null;

        Thread AutoSendCommandThread = null;               //自动发送单条指令线程
        Thread AutoSendAllCommandThread = null;            //自动发送多条指令线程 

        /// <summary>
        /// 窗体加载函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //通讯方式赋初值
            this.cboClientMode.Items.AddRange(GetResources.iResource.GetString("ClientMode").Split(','));
            this.cboClientMode.SelectedIndex = Properties.Settings.Default.CLIENTMODE;
           

            //串口通讯窗口赋初值
            this.cboPort.Items.AddRange(SerialPort.GetPortNames());
            this.cboBaud.Items.AddRange(new object[] { 4800, 9600, 14400, 19200, 38400, 56000, 576000, 115200, 128000, 256000 });
            this.cboDataBits.Items.AddRange(new object[] { 5, 6, 7, 8 });
            this.cboStopBits.Items.AddRange(new object[] { System.IO.Ports.StopBits.One, System.IO.Ports.StopBits.Two });
            this.cboParity.Items.AddRange(new object[] { System.IO.Ports.Parity.None, System.IO.Ports.Parity.Odd, System.IO.Ports.Parity.Even, System.IO.Ports.Parity.Mark });
            this.cboPort.SelectedItem = Properties.Settings.Default.PORT;
            this.cboBaud.SelectedItem = Properties.Settings.Default.BAUDRATE;
            this.cboDataBits.SelectedItem = Properties.Settings.Default.DATABITS;
            this.cboStopBits.SelectedItem = Properties.Settings.Default.STOPBITS;
            this.cboParity.SelectedItem = Properties.Settings.Default.PARITY;


            //网络通讯方式窗口赋初值
            this.txtIPAdress.Text = Properties.Settings.Default.IP;
            this.txtIport.Text = Properties.Settings.Default.iPORT.ToString();
            this.cboSocketMode.Items.AddRange(GetResources.iResource.GetString("SocketClientMode").Split(','));
            this.cboSocketMode.SelectedIndex = Properties.Settings.Default.SOCKETMODE;

            //USB通讯
            this.txtUSBVid.Text = Properties.Settings.Default.Vid;
            this.txtUSBPid.Text = Properties.Settings.Default.Pid;
            //this.txtUSBLocation.Text = Properties.Settings.Default.Location;


            //界面初始赋值
            txtAutoSend.Text = Properties.Settings.Default.AUTOSEND;
            this.cboEOF.Items.AddRange(new string[] {"Null", "\\0", "\\r", "\\n", "\\r\\n","\\n\\r" });
            this.cboEOF.SelectedItem = Properties.Settings.Default.SENDEOF;
			if (string.IsNullOrEmpty(Properties.Settings.Default.SENDEOF))
				this.cboEOF.SelectedIndex = 0;

            this.txtSendAll.Text = Properties.Settings.Default.SENDALL;
            this.txtSend.Text = Properties.Settings.Default.COMMAND;
            this.txtReceived.ReadOnly = true;
            this.txtReceived.BackColor = Color.White;
            this.cboShowEOF.Items.AddRange(GetResources.iResource.GetString("ReceivedInterval").Split(','));
			this.lblRemark.Visible = false;
            if (!ckbShowHex.Checked)
            {
                txtReceived.ScrollToCaret();

            }
            else
            {
                txtReceivedHex.ScrollToCaret();
            }

            cboShowTime.Items.AddRange(GetResources.iResource.GetString("ShowTime").Split(','));
            cboShowTime.SelectedIndex = 0;
            //默认无间隔
            this.cboShowEOF.SelectedIndex = 0;

            this.ckbAutoSend.Enabled = false;
            this.txtReceivedHex.Hide();
            this.txtReceivedHex.ReadOnly = true;
            this.txtReceivedHex.BackColor = Color.White;


            this.txtSendedCommand.BackColor = Color.White;

            if (Properties.Settings.Default.RECEIVED.Length > 0)
                txtReceived.Text = Properties.Settings.Default.RECEIVED;
            if (Properties.Settings.Default.RECEIVEDHEX.Length > 0)
                txtReceivedHex.Text = Properties.Settings.Default.RECEIVEDHEX;

            //默认隐藏扩展栏
            splitContainer1.SplitterDistance = splitContainer1.Width;

			try
			{
				//获取多条指令存储文件名
				DirectoryInfo dinfo = new DirectoryInfo(Cache.FilePath);
				if (dinfo.Exists)
				{
					FileSystemInfo[] fsinfos = dinfo.GetFileSystemInfos();
					foreach (FileSystemInfo fsinfo in fsinfos)
					{
						if (fsinfo is DirectoryInfo)
						{
							DirectoryInfo dirinfo = new DirectoryInfo(fsinfo.FullName);
							cboFileNames.Items.Add(dirinfo.Name);
						}
						else
						{
							FileInfo finfo = new FileInfo(fsinfo.FullName);
							cboFileNames.Items.Add(finfo.Name);
						}
					}
				}

				//读取配置文件
				if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "CommLabConfig.xml")))
				{
					File.Create(Path.Combine(Environment.CurrentDirectory, "CommLabConfig.xml"));
				}

				XmlDocument docConfig = new XmlDocument();
				docConfig.Load(Path.Combine(Environment.CurrentDirectory, "CommLabConfig.xml"));
				if (docConfig.ChildNodes.Count > 0 && docConfig.FirstChild.ChildNodes.Count > 0)
				{
					for (int i = 0; i < docConfig.FirstChild.ChildNodes.Count; i++)
					{
						string config = docConfig.FirstChild.ChildNodes[i].Attributes["Name"].Value.ToString() + ":" + docConfig.FirstChild.ChildNodes[i].Attributes["IPAdress"].Value.ToString()
							 + ":" + docConfig.FirstChild.ChildNodes[i].Attributes["Port"].Value.ToString() + ":" + docConfig.FirstChild.ChildNodes[i].Attributes["ClientMode"].Value.ToString();
						this.ConfigInfo.Add(config);
						this.cmbConfig.Items.Add(docConfig.FirstChild.ChildNodes[i].Attributes["Name"].Value.ToString());
					}
				}

				if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "CommLabCRC.xml")))
				{
					File.Create(Path.Combine(Environment.CurrentDirectory, "CommLabCRC.xml"));
				}

				XmlDocument docCRC = new XmlDocument();
				docCRC.Load(Path.Combine(Environment.CurrentDirectory, "CommLabCRC.xml"));
				if (docCRC.ChildNodes.Count > 0 && docCRC.FirstChild.ChildNodes.Count > 0)
				{
					if (docCRC.FirstChild.ChildNodes[0].Attributes["CurrentMode"].Value.ToString() == "Modbus")
					{
						isModbusCRC = true;
					}
				}

			}
			catch 
			{
				MessageBox.Show("配置文件格式错误,请检查.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}


        }

        /// <summary>
        /// 实时数据接收事件
        /// </summary>
        private void ClientInstance_DataReceived()
        {
            if (ClientInstance.IsOpen)
            {
                if (ClientInstance.Available > 0)
                {
                    byte[] data;
                    ClientInstance.Read(out data);
                    if (cboClientMode.SelectedIndex == 0)
                    {
                        if (data != null)
                        {
                            if (data.Length > 0)
                            {
                                for (int i = 0; i < data.Length; i++)
                                {
                                    if (data[i] == (byte)'\0')
                                    {

										txtReceivedHex.AppendText(string.Format("{0} ", data[i].ToString("X2")));

                                        //是否显示时间
                                        if (cboShowTime.SelectedIndex > 0)
                                        {

                                            if (cboShowTime.SelectedIndex == 1)
                                            {
                                                TimeSpan span = DateTime.Now - StartTime;
                                                txtReceived.AppendText(string.Format("  Time:{0}", span.TotalMilliseconds.ToString()));
                                                txtReceivedHex.AppendText(string.Format("  Time:{0}", span.TotalMilliseconds.ToString()));

                                            }

                                            if (cboShowTime.SelectedIndex == 2)
                                            {
                                                txtReceived.AppendText(string.Format("  Time:{0}:{1}:{2}:{3}", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString(), DateTime.Now.Millisecond.ToString()));
                                                txtReceivedHex.AppendText(string.Format("  Time:{0}:{1}:{2}:{3}", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString(), DateTime.Now.Millisecond.ToString()));
                                            }
                                        }


                                        if (cboShowEOF.SelectedIndex > 0)
                                        {
                                            // 1位是空格,2位是换行,3位是Tab格.之后添加在窗体构造函数加入后来这里添加if
                                            if (cboShowEOF.SelectedIndex == 1)
                                            {
                                                txtReceivedHex.AppendText(" ");
                                                txtReceived.AppendText(" ");
                                            }
                                            if (cboShowEOF.SelectedIndex == 2)
                                            {
                                                txtReceivedHex.AppendText("\r\n");
                                                txtReceived.AppendText("\r\n");
                                            }
                                            if (cboShowEOF.SelectedIndex == 3)
                                            {
                                                txtReceivedHex.AppendText("\t");
                                                txtReceived.AppendText("\t");
                                            }
                                        }
                                    }
                                    else
                                    {

                                        txtReceivedHex.AppendText(string.Format("{0} ", data[i].ToString("X2")));
                                        try
                                        {
                                            if (data[i] == (byte)'\r')
                                            {

                                            }
                                            //txtReceived.AppendText("\r");
                                            else
                                            txtReceived.AppendText(string.Format(System.Text.ASCIIEncoding.ASCII.GetString(data, i, 1)));
                                        }
                                        catch
                                        {
                                            txtReceived.AppendText(string.Format("{0} ", data[i].ToString("X2")));
                                        }
                                        //txtReceived.AppendText(string.Format(System.Text.ASCIIEncoding.ASCII.GetString(data, i, 1)));
                                        if (!ckbShowHex.Checked)
                                        {
                                            ReceivedCount = txtReceived.TextLength;
                                        }
                                        else
                                        {
                                            ReceivedCount = txtReceivedHex.TextLength;
                                        }
                                        lblReceived.Text = string.Format("Received:{0}", ReceivedCount.ToString());
                                    }
                                }
                            }
                        }
                    }

                    if (this.cboClientMode.SelectedIndex == 1)
                    {

                        if (data != null)
                        {
                            if (data.Length > 0)
                            {
								//接收文本指令 
                                txtReceived.AppendText(System.Text.ASCIIEncoding.ASCII.GetString(data).Replace("\0"," "));
                                //是否显示时间
                                if (cboShowTime.SelectedIndex > 0)
                                {

                                    if (cboShowTime.SelectedIndex == 1)
                                    {
                                        TimeSpan span = DateTime.Now - StartTime;
                                        txtReceived.AppendText(string.Format("  Time:{0}", span.TotalMilliseconds.ToString()));

                                    }

                                    if (cboShowTime.SelectedIndex == 2)
                                    {
                                        txtReceived.AppendText(string.Format("  Time:{0}:{1}:{2}:{3}", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString(), DateTime.Now.Millisecond.ToString()));
                                    }
                                }

								//接收十六进制指令
                                for (int i = 0; i < data.Length; i++)
                                {
                                    //try
                                    //{
                                    //    if (data[i] == (byte)'\r')
                                    //    {
                                    //    }
                                    //    else
                                    //        txtReceived.AppendText(System.Text.ASCIIEncoding.ASCII.GetString(data, i, 1));
                                    //}
                                    //catch
                                    //{
                                    //    txtReceived.AppendText(string.Format("{0} ", data[i].ToString("X2")));
                                    //}
                                        
                                    txtReceivedHex.AppendText(string.Format("{0} ", data[i].ToString("X2")));
                                    if (!ckbShowHex.Checked)
                                    {
                                        ReceivedCount = txtReceived.TextLength;
                                    }
                                    else
                                    {
                                        ReceivedCount = txtReceivedHex.TextLength;
                                    }
                                    lblReceived.Text = string.Format("Received:{0}", ReceivedCount.ToString());
                                }


                                //是否显示时间
                                if (cboShowTime.SelectedIndex > 0)
                                {

                                    if (cboShowTime.SelectedIndex == 1)
                                    {
                                        TimeSpan span = DateTime.Now - StartTime;
                                        txtReceivedHex.AppendText(string.Format("  Time:{0}", span.TotalMilliseconds.ToString()));

                                    }

                                    if (cboShowTime.SelectedIndex == 2)
                                    {
                                        txtReceivedHex.AppendText(string.Format("  Time:{0}:{1}:{2}:{3}", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString(), DateTime.Now.Millisecond.ToString()));
                                    }
                                }

                                if (cboShowEOF.SelectedIndex > 0)
                                {
                                    // 1位是空格,2位是换行,3位是Tab格.之后添加在窗体构造函数加入后来这里添加if
                                    if (cboShowEOF.SelectedIndex == 1)
                                    {
                                        txtReceivedHex.AppendText(" ");
                                        txtReceived.AppendText(" ");
                                    }
                                    if (cboShowEOF.SelectedIndex == 2)
                                    {
                                        txtReceivedHex.AppendText("\r\n");
                                        txtReceived.AppendText("\r\n");
                                    }
                                    if (cboShowEOF.SelectedIndex == 3)
                                    {
                                        txtReceivedHex.AppendText("\t");
                                        txtReceived.AppendText("\t");
                                    }
                                }
                            }
                        }
                    }

                    if (this.cboClientMode.SelectedIndex == 2)
                    {
                        if (data != null)
                        {
                            if (data.Length > 0)
                            {
                                //接收文本指令 
                                txtReceived.AppendText(System.Text.ASCIIEncoding.ASCII.GetString(data).Replace("\0", " "));
                                //是否显示时间
                                if (cboShowTime.SelectedIndex > 0)
                                {

                                    if (cboShowTime.SelectedIndex == 1)
                                    {
                                        TimeSpan span = DateTime.Now - StartTime;
                                        txtReceived.AppendText(string.Format("  Time:{0}", span.TotalMilliseconds.ToString()));

                                    }

                                    if (cboShowTime.SelectedIndex == 2)
                                    {
                                        txtReceived.AppendText(string.Format("  Time:{0}:{1}:{2}:{3}", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString(), DateTime.Now.Millisecond.ToString()));
                                    }
                                }

                                //接收十六进制指令
                                for (int i = 0; i < data.Length; i++)
                                {
                                    //try
                                    //{
                                    //    if (data[i] == (byte)'\r')
                                    //    {
                                    //    }
                                    //    else
                                    //        txtReceived.AppendText(System.Text.ASCIIEncoding.ASCII.GetString(data, i, 1));
                                    //}
                                    //catch
                                    //{
                                    //    txtReceived.AppendText(string.Format("{0} ", data[i].ToString("X2")));
                                    //}

                                    txtReceivedHex.AppendText(string.Format("{0} ", data[i].ToString("X2")));
                                    if (!ckbShowHex.Checked)
                                    {
                                        ReceivedCount = txtReceived.TextLength;
                                    }
                                    else
                                    {
                                        ReceivedCount = txtReceivedHex.TextLength;
                                    }
                                    lblReceived.Text = string.Format("Received:{0}", ReceivedCount.ToString());
                                }


                                //是否显示时间
                                if (cboShowTime.SelectedIndex > 0)
                                {

                                    if (cboShowTime.SelectedIndex == 1)
                                    {
                                        TimeSpan span = DateTime.Now - StartTime;
                                        txtReceivedHex.AppendText(string.Format("  Time:{0}", span.TotalMilliseconds.ToString()));

                                    }

                                    if (cboShowTime.SelectedIndex == 2)
                                    {
                                        txtReceivedHex.AppendText(string.Format("  Time:{0}:{1}:{2}:{3}", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString(), DateTime.Now.Millisecond.ToString()));
                                    }
                                }

                                if (cboShowEOF.SelectedIndex > 0)
                                {
                                    // 1位是空格,2位是换行,3位是Tab格.之后添加在窗体构造函数加入后来这里添加if
                                    if (cboShowEOF.SelectedIndex == 1)
                                    {
                                        txtReceivedHex.AppendText(" ");
                                        txtReceived.AppendText(" ");
                                    }
                                    if (cboShowEOF.SelectedIndex == 2)
                                    {
                                        txtReceivedHex.AppendText("\r\n");
                                        txtReceived.AppendText("\r\n");
                                    }
                                    if (cboShowEOF.SelectedIndex == 3)
                                    {
                                        txtReceivedHex.AppendText("\t");
                                        txtReceived.AppendText("\t");
                                    }
                                }
                            }
                        }

                    }


					//文本最多输入2147483647个字节,判断超过2147000000个字节时便保存窗口内容,并更新界面
                    if (txtReceived.TextLength >= 2147000000 || txtReceivedHex.TextLength >= 2147000000)
                    {
                        string time = DateTime.Now.ToLongDateString() + DateTime.Now.ToShortTimeString();
                        time = time.Replace(":", "时") + "分";
                        FileStream fs = new FileStream(string.Format(@"{0}\\{1}(字符串文本).txt", System.Environment.CurrentDirectory, time), FileMode.Append, FileAccess.Write);
                        StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                        sw.WriteLine(txtReceived.Text);
                        sw.Close();
                        fs.Close();
                        FileStream fshex = new FileStream(string.Format(@"{0}\\{1}(16进制文本).txt", System.Environment.CurrentDirectory, time), FileMode.Append, FileAccess.Write);
                        StreamWriter swhex = new StreamWriter(fshex, System.Text.Encoding.Default);
                        swhex.WriteLine(txtReceivedHex.Text);
                        swhex.Close();
                        fshex.Close();
                        lblMessage.ForeColor = Color.Red;
                        lblMessage.Text = "注意:数据读取过大,已自动保存到当前目录下,名称为保存时间 + 显示方式";
                        txtReceived.Text = string.Empty;
                        txtReceivedHex.Text = string.Empty;
                    }
                }
            }
        }


        #endregion

        #region 窗体控件事件

        /// <summary>
        /// 打开通讯方式函数
        /// </summary>
        private void OpenClient()
        {
            if (this.cboClientMode.SelectedIndex == 0)
            {
                if (cboPort.SelectedIndex >= 0 && cboBaud.SelectedIndex >= 0 && cboDataBits.SelectedIndex >= 0 && cboStopBits.SelectedIndex >= 0 && cboParity.SelectedIndex >= 0)
                {
                    this.Port = cboPort.SelectedItem.ToString();
                    this.Baud = (int)cboBaud.SelectedItem;
                    this.DataBits = (int)cboDataBits.SelectedItem;
                    this.StopBits = (System.IO.Ports.StopBits)cboStopBits.SelectedItem;
                    this.Parity = (System.IO.Ports.Parity)cboParity.SelectedItem;
                    Xmas11.Comm.Core.CommSettings settings = new Xmas11.Comm.Core.SerialPortCommSettings(this.Port, this.Baud, this.DataBits, this.StopBits, this.Parity);
                    settings.ReadBufferSize = Properties.Settings.Default.READBUFFERSIZE;
                    ClientInstance = Xmas11.Comm.Core.CommBuilder.CreateClient(settings) as Xmas11.Comm.Core.ICommClient;
                    ClientInstance.Connect();
                }
            }
            if (this.cboClientMode.SelectedIndex == 1)
            {
                IPAddress ipResult;
                int iportResult;
                if (IPAddress.TryParse(txtIPAdress.Text, out ipResult) && int.TryParse(txtIport.Text, out iportResult))
                {
                    this.Ip = ipResult;
                    this.IPort = iportResult;
                    //UDP通讯
                    if (cboSocketMode.SelectedIndex == 0)
                    {
                        Xmas11.Comm.Core.CommSettings settings = new Xmas11.Comm.Core.SocketCommSettings(this.Ip, this.IPort, false);
                        settings.ReadBufferSize = Properties.Settings.Default.READBUFFERSIZE;
                        ClientInstance = Xmas11.Comm.Core.CommBuilder.CreateClient(settings) as Xmas11.Comm.Core.ICommClient;
                        ClientInstance.Connect();
                    }

                    //TCP通讯
                    if (cboSocketMode.SelectedIndex == 1)
                    {
                        Xmas11.Comm.Core.CommSettings settings = new Xmas11.Comm.Core.SocketCommSettings(this.Ip, this.IPort, true);
                        settings.ReadBufferSize = Properties.Settings.Default.READBUFFERSIZE;
                        ClientInstance = Xmas11.Comm.Core.CommBuilder.CreateClient(settings) as Xmas11.Comm.Core.ICommClient;
                        ClientInstance.Connect();
                    }
                }

            }

            //添加USB
            if (this.cboClientMode.SelectedIndex == 2)
            {
                ushort vid;
                ushort pid;
                if (ushort.TryParse(txtUSBVid.Text, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out vid) && ushort.TryParse(txtUSBPid.Text, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out pid))
                {
                    List<Xmas11.IO.USB.DeviceProperties> usbDeviceList = new List<Xmas11.IO.USB.DeviceProperties>();
                    bool isFound = false;
                    if (!Xmas11.IO.USB.USBDevice.Find(vid, pid, ref usbDeviceList))
                    {
                        System.Windows.Forms.MessageBox.Show(GetResources.iResource.GetString("ErrorUSBStartClient"), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    foreach (Xmas11.IO.USB.DeviceProperties usbDevice in usbDeviceList)
                    {
                        if (!cboUSBLocation.Items.Contains(usbDevice.DeviceLocation))
                            cboUSBLocation.Items.Add(usbDevice.DeviceLocation);
                        if (usbDevice.DeviceLocation == cboUSBLocation.Text)
                        {
                            isFound = true;
                        }
                    }
                    if (!isFound)
                    {
                        cboUSBLocation.Text = usbDeviceList[0].DeviceLocation;
                    }
                    Xmas11.Comm.Core.CommSettings settings = new Xmas11.Comm.Core.UsbCommSettings(vid, pid, cboUSBLocation.Text);
                    settings.ReadBufferSize = Properties.Settings.Default.READBUFFERSIZE;
                    ClientInstance = Xmas11.Comm.Core.CommBuilder.CreateClient(settings) as Xmas11.Comm.Core.ICommClient;
                    ClientInstance.Connect();
                }
            }

            if (ClientInstance != null)
            {
                //打开成功
                if (ClientInstance.IsOpen)
                {
                    ClientInstance.DataReceived += ClientInstance_DataReceived;
                    //ClientInstance.DataReceived += new iComm.DataReceivedHandler(ClientInstance_DataReceived);
                    this.btnConnect.Text = GetResources.iResource.GetString("btnCloseText");
                    this.cboClientMode.Enabled = false;
                    this.ckbAutoSend.Enabled = true;

                    this.StartTime = DateTime.Now;

					//cboPort.Enabled = false;
					//cboBaud.Enabled = false;
					//cboDataBits.Enabled = false;
					//cboStopBits.Enabled = false;
					//cboParity.Enabled = false;
                    cboSocketMode.Enabled = false;
					cmbConfig.Enabled = false;
                    txtIPAdress.ReadOnly = true;
                    txtIport.ReadOnly = true;
                    cboUSBLocation.Enabled = false;
                    txtUSBVid.ReadOnly = true;
                    txtUSBPid.ReadOnly = true;
                    btnUSBLocation.Enabled = false;
                }
                else//打开失败
                {
                    MessageBox.Show(GetResources.iResource.GetString("ErrorStartClient"), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ClientInstance = null;
                }
            }
            else
                MessageBox.Show(GetResources.iResource.GetString("ErrorStartClient"), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 关闭通讯方式函数
        /// </summary>
        private void CloseClient()
        {
            //自动发送选项关闭
            if (ckbAutoSend.Checked)
            {
                ckbAutoSend.Checked = false;
                AutoThreadFlag = false;
                AutoSendCommandThread = null;
                txtAutoSend.ReadOnly = false;

            }

            //自动发送多条指令选项关闭
            if (ckbAutoSendAll.Checked)
            {
                ckbAutoSendAll.Checked = false;
                AutoSendAllThreadFlag = false;
                AutoSendAllCommandThread = null;
                txtSendAll.ReadOnly = false;
            }

            if (ClientInstance.IsOpen)
            {
                //ClientInstance.DataReceived -= ClientInstance_DataReceived;
                ClientInstance.DataReceived -= new Xmas11.Comm.Core.DataReceivedEventHandler(ClientInstance_DataReceived);
                //ClientInstance.DataReceived -= new iComm.DataReceivedHandler(ClientInstance_DataReceived);
                //ClientInstance.Close();
                ClientInstance.Dispose();
            }
            ClientInstance = null;
            this.btnConnect.Text = GetResources.iResource.GetString("btnOpenText");
            this.cboClientMode.Enabled = true;
            this.ckbAutoSend.Enabled = false;


            //串口和网络面板选项全部禁用.
			//cboPort.Enabled = true;
			//cboBaud.Enabled = true;
			//cboDataBits.Enabled = true;
			//cboStopBits.Enabled = true;
			//cboParity.Enabled = true;
			cmbConfig.Enabled = true;
            txtIPAdress.ReadOnly = false;
            txtIport.ReadOnly = false;
            cboSocketMode.Enabled = true;
            cboUSBLocation.Enabled = true;
            txtUSBVid.ReadOnly = false;
            txtUSBPid.ReadOnly = false;
            btnUSBLocation.Enabled = true;
        }

        /// <summary>
        /// 打开连接(串口通讯和网络通讯和USB通讯)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (this.btnConnect.Text == GetResources.iResource.GetString("btnOpenText"))
            {
                OpenClient();
            }
            //关闭连接
            else if (ClientInstance != null)
            {
                CloseClient();
            }
        }

        /// <summary>
        /// 扩展按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommand_Click(object sender, EventArgs e)
        {
            if (btnCommand.Text == GetResources.iResource.GetString("ExpandOpen"))
            {
                this.splitContainer1.SplitterDistance = splitContainer1.Width - 260;
                this.btnCommand.Text = GetResources.iResource.GetString("ExpandClose");
            }
            else
            {
                this.splitContainer1.SplitterDistance = splitContainer1.Width;
                this.btnCommand.Text = GetResources.iResource.GetString("ExpandOpen");
            }

        }

        /// <summary>
        /// 发送按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!ckbShowHex.Checked)
            {
                txtReceived.ScrollToCaret();
            }
            else
            {
                txtReceivedHex.ScrollToCaret();
            }

            if (ClientInstance == null)
                OpenClient();
            if (ClientInstance != null)
            {
                if (ClientInstance.IsOpen)
                {
                    string cmdString;
                    if (ckbHexSend.Checked)
                    {

						//16进制加结束符
						//1代表\0,2代表\r,3代表\n,4代表\r\n,5代表\n\r
						if (cboEOF.SelectedIndex == 1)
							cmdString = txtSend.Text.Trim() + " 00";
						else if (cboEOF.SelectedIndex == 2)
							cmdString = txtSend.Text.Trim() + " 0D";
						else if (cboEOF.SelectedIndex == 3)
							cmdString = txtSend.Text.Trim() + " 0A";
						else if (cboEOF.SelectedIndex == 4)
							cmdString = txtSend.Text.Trim() + " 0D 0A";
						else if (cboEOF.SelectedIndex == 5)
							cmdString = txtSend.Text.Trim() + " 0A 0D";
						else
							cmdString = txtSend.Text.Trim();


						string[] items = cmdString.Split(' ');
                        byte[] Bitems = new byte[items.Length];
                        // 16进制转2进制,然后发送
                        for (int i = 0; i < items.Length; i++)
                        {
                            byte item = byte.MaxValue;
                            byte.TryParse(items[i], System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out item);
                            Bitems[i] = item;
                        }

                        ClientInstance.ExetuteNonQuery(new Xmas11.Comm.Commander.iBinaryCommand(Bitems));

                        
                        //是否显示时间
                        if (cboShowTime.SelectedIndex > 0)
                        {
                            if (cboShowTime.SelectedIndex == 1)
                            {
                                TimeSpan span = DateTime.Now - StartTime;
								txtSendedCommand.AppendText(cmdString + string.Format("  Time:{0}", span.TotalMilliseconds.ToString()) + "\r\n");
                            }
                            if (cboShowTime.SelectedIndex == 2)
                            {
								txtSendedCommand.AppendText(cmdString + string.Format("  Time:{0}:{1}:{2}:{3}", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString(), DateTime.Now.Millisecond.ToString()) + "\r\n");

                            }
                        }
                        else
                        {
							txtSendedCommand.AppendText(cmdString + "\r\n");
                        }
                    }
                    else
                    {
                        //1代表\0,2代表\r,3代表\n,4代表\r\n,5代表\n\r
						if (cboEOF.SelectedIndex == 1)
							cmdString = txtSend.Text + "\0";
						else if (cboEOF.SelectedIndex == 2)
							cmdString = txtSend.Text + "\r";
						else if (cboEOF.SelectedIndex == 3)
							cmdString = txtSend.Text + "\n";
						else if (cboEOF.SelectedIndex == 4)
							cmdString = txtSend.Text + "\r\n";
						else if (cboEOF.SelectedIndex == 5)
							cmdString = txtSend.Text + "\n\r";
						else
                            cmdString = txtSend.Text;

                        ClientInstance.ExetuteNonQuery(new Xmas11.Comm.Commander.iTextCommand(cmdString));
                        //是否显示时间
                        if (cboShowTime.SelectedIndex > 0)
                        {
                            if (cboShowTime.SelectedIndex == 1)
                            {
                                TimeSpan span = DateTime.Now - StartTime;
                                txtSendedCommand.AppendText(txtSend.Text + string.Format("  Time:{0}", span.TotalMilliseconds.ToString()) + "\r\n");
                            }
                            if (cboShowTime.SelectedIndex == 2)
                            {
                                txtSendedCommand.AppendText(txtSend.Text + string.Format("  Time:{0}:{1}:{2}:{3}", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString(), DateTime.Now.Millisecond.ToString()) + "\r\n");
                            }
                        }
                        else
                        {
                            txtSendedCommand.AppendText(txtSend.Text + "\r\n");
                        }
                    }

                    this.SendCount += txtSend.TextLength;
                    lblSend.Text = string.Format("Send:{0}", this.SendCount.ToString());
                }
                lblMessage.Text = string.Empty;

            }
        }

        /// <summary>
        /// 查询USB Location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUSBLocation_Click(object sender, EventArgs e)
        {
            ushort vid;
            ushort pid;
            if (ushort.TryParse(txtUSBVid.Text, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out vid) && ushort.TryParse(txtUSBPid.Text, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out pid))
            {
                List<Xmas11.IO.USB.DeviceProperties> usbDeviceList = new List<Xmas11.IO.USB.DeviceProperties>();
                bool isFound = false;
                if (!Xmas11.IO.USB.USBDevice.Find(vid, pid, ref usbDeviceList))
                {
                    MessageBox.Show(GetResources.iResource.GetString("ErrorUSBStartClient"), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                cboUSBLocation.Items.Clear();
                foreach (Xmas11.IO.USB.DeviceProperties usbDevice in usbDeviceList)
                {
                    if (!cboUSBLocation.Items.Contains(usbDevice.DeviceLocation))
                    {

                        cboUSBLocation.Items.Add(usbDevice.DeviceLocation);
                    }
                    if (usbDevice.DeviceLocation == cboUSBLocation.Text)
                    {
                        isFound = true;
                    }
                }
                if (!isFound)
                {
                    cboUSBLocation.Text = usbDeviceList[0].DeviceLocation;
                }
            }
        }
         
        /// <summary>
        /// byte类型显示文本框内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckbShowHex_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbShowHex.Checked)
            {
                txtReceived.Visible = false;
                txtReceivedHex.Show();
            }
            else
            {
                txtReceivedHex.Hide();
                txtReceived.Visible = true;
            }

        }

        /// <summary>
        /// 16进制数组发送事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckbHexSend_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbHexSend.Checked && txtSend.TextLength > 0)
            {
                byte[] items = System.Text.ASCIIEncoding.ASCII.GetBytes(txtSend.Text.Replace("\n", string.Empty).Replace("\r", string.Empty));
                txtSend.Clear();
                txtSend.AppendText(string.Format(items[0].ToString("X2")));
                for (int i = 1; i < items.Length; i++)
                {
                    txtSend.AppendText(string.Format(" " + items[i].ToString("X2")));
                }
            }
            else if (!ckbHexSend.Checked && txtSend.TextLength > 0) 
            {
                for (int i = txtSend.TextLength - 1; i >= 0; i--)
                {
                    if (txtSend.Text[i] == ' ')
                        txtSend.Text = txtSend.Text.Remove(i, 1);
                    else
                        break;
                }
                string[] items = txtSend.Text.Split(' ');
                byte[] Bitems = new byte[items.Length];
                for (int i = 0; i < items.Length; i++)
                {

                    //NumberStyles
                    byte item = byte.MaxValue;
                    byte.TryParse(items[i], System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out item);
                    Bitems[i] = item;
                }
                string Result = System.Text.ASCIIEncoding.ASCII.GetString(Bitems);
                txtSend.Clear();
                txtSend.AppendText(Result);
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtReceived.Clear();
            txtReceivedHex.Clear();
            txtSendedCommand.Clear();
            this.SendCount = 0;
            this.ReceivedCount = 0;
            this.lblSend.Text = string.Format("Send:{0}", this.SendCount.ToString());
            this.lblReceived.Text = string.Format("Received:{0}", this.ReceivedCount.ToString());
            lblMessage.Text = string.Empty;
        }

        /// <summary>
        /// 串口下拉框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Port = cboPort.SelectedItem.ToString();
			if (ClientInstance != null && ClientInstance.IsOpen)
			{
				bool isAutoSend = ckbAutoSend.Checked;

				this.CloseClient();
				this.OpenClient();
				if (ClientInstance!=null && ClientInstance.IsOpen)
					this.ckbAutoSend.Checked = isAutoSend;
			}
        }

		/// <summary>
		/// 波特率下拉框事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboBaud_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ClientInstance != null && ClientInstance.IsOpen)
			{
				bool isAutoSend = ckbAutoSend.Checked;

				this.CloseClient();
				this.OpenClient();

				if (ClientInstance != null && ClientInstance.IsOpen)
					this.ckbAutoSend.Checked = isAutoSend;
			}
		}

		/// <summary>
		/// 数据位下拉框事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboDataBits_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ClientInstance != null && ClientInstance.IsOpen)
			{
				bool isAutoSend = ckbAutoSend.Checked;

				this.CloseClient();
				this.OpenClient();

				if (ClientInstance != null && ClientInstance.IsOpen)
					this.ckbAutoSend.Checked = isAutoSend;
			}
		}

		/// <summary>
		/// 停止位下拉框事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboStopBits_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ClientInstance != null && ClientInstance.IsOpen)
			{
				bool isAutoSend = ckbAutoSend.Checked;

				this.CloseClient();
				this.OpenClient();

				if (ClientInstance != null && ClientInstance.IsOpen)
					this.ckbAutoSend.Checked = isAutoSend;
			}
		}

		/// <summary>
		/// 校验位下拉框事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboParity_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ClientInstance != null && ClientInstance.IsOpen)
			{
				bool isAutoSend = ckbAutoSend.Checked;

				this.CloseClient();
				this.OpenClient();

				if (ClientInstance != null && ClientInstance.IsOpen)
					this.ckbAutoSend.Checked = isAutoSend;
			}
		}

        /// <summary>
        /// USB Location下拉框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboUSBLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ushort vid;
            //ushort pid;
            ////cboUSBLocation.Items.Clear();
            //this.CloseClient();
            //this.OpenClient();
            //if (ushort.TryParse(txtUSBVid.Text, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out vid) && ushort.TryParse(txtUSBPid.Text, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out pid))
            //{
            //    List<Xmas11.IO.USB.DeviceProperties> usbDevice = new List<Xmas11.IO.USB.DeviceProperties>();
            //    bool isFound = IO.USB.USBDevice.Find(vid, pid, ref usbDevice);
            //    foreach (IO.USB.DeviceProperties _usbList in usbDevice)
            //    {
            //        cboUSBLocation.Items.Add(_usbList.DeviceLocation);
            //    }
            //    Xmas11.Comm.Core.CommSettings settings = new Xmas11.Comm.Core.UsbCommSettings(vid, pid, cboUSBLocation.Text);
            //    settings.ReadBufferSize = Properties.Settings.Default.READBUFFERSIZE;
            //    ClientInstance = Xmas11.Comm.Core.CommBuilder.CreateClient(settings) as Comm.Core.ICommClient;
            //    ClientInstance.Connect();
            //}
        }

        /// <summary>
        /// 发送多行选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckbMultLine_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbMultLine.Checked)
            {
                MainForm.ActiveForm.AcceptButton = null;
                //this.ParentForm.AcceptButton
            }
            else
            {
                MainForm.ActiveForm.AcceptButton = btnSend;
            }
        }


        /// <summary>
        /// 自动发送按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckbAutoSend_CheckedChanged(object sender, EventArgs e)
        {
            if (ClientInstance != null)
            {
                if (ckbAutoSend.Checked)
                {
                    AutoThreadFlag = true;
                    AutoSendCommandThread = new Thread(new ThreadStart(AutoSendThreadStart));
                    AutoSendCommandThread.IsBackground = true;
                    AutoSendCommandThread.Start();
                    txtAutoSend.ReadOnly = true;
                }
                else
                {
                    AutoThreadFlag = false;
                    AutoSendCommandThread = null;
                    txtAutoSend.ReadOnly = false;
                }
            }
        }

        /// <summary>
        /// 自动发送线程
        /// </summary>
        private void AutoSendThreadStart()
        {
            while(AutoThreadFlag)
            {
                //默认睡眠时间100
                int timeSpan = 0;
                if (int.TryParse(txtAutoSend.Text, out timeSpan))
                    System.Threading.Thread.Sleep(timeSpan);
                else
                    System.Threading.Thread.Sleep(100);

                //判断iComm实例化是非为空
                if (ClientInstance == null || !ClientInstance.IsOpen)
                    break;

                //发送
                string cmdString;
                if (ckbHexSend.Checked)
                {
                    for (int i = txtSend.TextLength - 1; i >= 0; i--)
                    {
                        if (txtSend.Text[i] == ' ')
                            txtSend.Text = txtSend.Text.Remove(i, 1);
                        else
                            break;
                    }

                    string[] items = txtSend.Text.Split(' ');
                    byte[] Bitems = new byte[items.Length];
                    // 16进制转2进制,然后发送
                    for (int i = 0; i < items.Length; i++)
                    {
                        byte item = byte.MaxValue;
                        byte.TryParse(items[i], System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out item);
                        Bitems[i] = item;
                    }
                    //iComm.iOneWayBinaryCommand cmd = new iComm.iOneWayBinaryCommand(ClientInstance, Bitems);
                    //cmd.Execute();
                    ClientInstance.ExetuteNonQuery(new Xmas11.Comm.Commander.iBinaryCommand(Bitems));
                    //是否显示时间
                    if (cboShowTime.SelectedIndex > 0)
                    {
                        if (cboShowTime.SelectedIndex == 1)
                        {
                            TimeSpan span = DateTime.Now - StartTime;
                            txtSendedCommand.AppendText(txtSend.Text + string.Format("  Time:{0}", span.TotalMilliseconds.ToString()) + "\r\n");
                        }
                        if (cboShowTime.SelectedIndex == 2)
                        {
                            txtSendedCommand.AppendText(txtSend.Text + string.Format("  Time:{0}:{1}:{2}:{3}", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString(), DateTime.Now.Millisecond.ToString()) + "\r\n");

                        }
                    }
                    else
                    {
                        txtSendedCommand.AppendText(txtSend.Text + "\r\n");
                    }

                }
                else
                {
                    //1代表\0,2代表\r,3代表\n,4代表\r\n,5代表\n\r
					if (cboEOF.SelectedIndex == 1)
						cmdString = txtSend.Text + "\0";
					else if (cboEOF.SelectedIndex == 2)
						cmdString = txtSend.Text + "\r";
					else if (cboEOF.SelectedIndex == 3)
						cmdString = txtSend.Text + "\n";
					else if (cboEOF.SelectedIndex == 4)
						cmdString = txtSend.Text + "\r\n";
					else if (cboEOF.SelectedIndex == 5)
						cmdString = txtSend.Text + "\n\r";
					else
						cmdString = txtSend.Text;
                    if (ClientInstance != null)
                        ClientInstance.ExetuteNonQuery(new Xmas11.Comm.Commander.iTextCommand(cmdString));
                    //iComm.iOneWayTextCommand cmd = new iComm.iOneWayTextCommand(ClientInstance, cmdString);
                    //cmd.Execute();
                    //是否显示时间
                    if (cboShowTime.SelectedIndex > 0)
                    {
                        if (cboShowTime.SelectedIndex == 1)
                        {
                            TimeSpan span = DateTime.Now - StartTime;
                            txtSendedCommand.AppendText(txtSend.Text + string.Format("  Time:{0}", span.TotalMilliseconds.ToString()) + "\r\n");
                        }
                        if (cboShowTime.SelectedIndex == 2)
                        {
                            txtSendedCommand.AppendText(txtSend.Text + string.Format("  Time:{0}:{1}:{2}:{3}", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString(), DateTime.Now.Millisecond.ToString()) + "\r\n");

                        }
                    }
                    else
                    {
                        txtSendedCommand.AppendText(txtSend.Text + "\r\n");
                    }
                }
                this.SendCount += txtSend.TextLength;
                lblSend.Text = string.Format("Send:{0}", this.SendCount.ToString());
            }
        }

        /// <summary>
        /// 窗口关闭函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (AutoSendCommandThread != null)
            {
                AutoThreadFlag = false;
                AutoSendCommandThread = null;
            }

            if (AutoSendAllCommandThread != null)
            {
                AutoSendAllThreadFlag = false;
                AutoSendAllCommandThread = null;
            }

            if (ClientInstance != null)
            {
                if (ClientInstance.IsOpen)
                {
                    //ClientInstance.DataReceived -= new iComm.DataReceivedHandler(ClientInstance_DataReceived);
                    ClientInstance.DataReceived -= ClientInstance_DataReceived;
                    ClientInstance.Close();
                }
                ClientInstance = null;
            }

            //存储默认值
            if(cboPort.SelectedIndex>=0)
                Properties.Settings.Default.PORT = cboPort.SelectedItem.ToString();
            Properties.Settings.Default.BAUDRATE = (int)cboBaud.SelectedItem;
            Properties.Settings.Default.DATABITS = (int)cboDataBits.SelectedItem;
            Properties.Settings.Default.STOPBITS = (System.IO.Ports.StopBits)cboStopBits.SelectedItem;
            Properties.Settings.Default.PARITY = (System.IO.Ports.Parity)cboParity.SelectedItem;

            Properties.Settings.Default.IP = txtIPAdress.Text;
            Properties.Settings.Default.iPORT = txtIport.Text;

            Properties.Settings.Default.Vid = txtUSBVid.Text;
            Properties.Settings.Default.Pid = txtUSBPid.Text;
            //Properties.Settings.Default.Location = txtUSBLocation.Text;

            Properties.Settings.Default.AUTOSEND = txtAutoSend.Text;
            Properties.Settings.Default.SENDEOF = (string)cboEOF.SelectedItem;
            Properties.Settings.Default.COMMAND = txtSend.Text;
            Properties.Settings.Default.CLIENTMODE = cboClientMode.SelectedIndex;
            Properties.Settings.Default.SOCKETMODE = cboSocketMode.SelectedIndex;

            Properties.Settings.Default.RECEIVED = txtReceived.Text;
            Properties.Settings.Default.RECEIVEDHEX = txtReceivedHex.Text;

            Properties.Settings.Default.Save();
        }

		/// <summary>
		/// 配置下拉框切换
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmbConfig_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbConfig.SelectedIndex != -1)
			{
				string[] items = this.ConfigInfo[cmbConfig.SelectedIndex].Split(':');
				this.txtIPAdress.Text = items[1];
				this.txtIport.Text = items[2];
				if (items[3].Contains("UDP"))
					this.cboSocketMode.SelectedIndex = 0;
				else
					this.cboSocketMode.SelectedIndex = 1;
			}
		}

		/// <summary>
		/// 16进制获取CRC校验按钮事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGetCRC_Click(object sender, EventArgs e)
		{
			if (ckbHexSend.Checked)
			{
				string[] cmd = txtSend.Text.Trim().Split(' ');

				byte[] crcBuf = new byte[cmd.Length];

				for (int i = 0; i < cmd.Length; i++)
				{
					if (!byte.TryParse(cmd[i].Replace("0X", string.Empty), System.Globalization.NumberStyles.HexNumber, null, out crcBuf[i]))
						return;
				}

				if (isModbusCRC)
				{
					int crcResult = CRC_M.CRC16(crcBuf, crcBuf.Length);

					if (txtSend.Text[txtSend.TextLength - 1] == ' ')
					{
						txtSend.Text += ((byte)crcResult).ToString("X2");
						txtSend.Text += " ";
						txtSend.Text += ((byte)(((ushort)crcResult) >> 8)).ToString("X2");
					}
					else
					{
						txtSend.Text += " ";
						txtSend.Text += ((byte)crcResult).ToString("X2");
						txtSend.Text += " ";
						txtSend.Text += ((byte)(((ushort)crcResult) >> 8)).ToString("X2");
					}


				}
				else
				{
					CRC.CRCEntity crc = new CRC.CRCEntity(CRC.CRCCoding.CRC16CCITT);
					//CRC16CCITT校验方法

					int crcResult = (ushort)crc.Sum(crcBuf);

					if (txtSend.Text[txtSend.TextLength - 1] == ' ')
					{
						txtSend.Text += ((byte)(((ushort)crcResult) >> 8)).ToString("X2");
						txtSend.Text += " ";
						txtSend.Text += ((byte)crcResult).ToString("X2");
					}
					else
					{
						txtSend.Text += " ";
						txtSend.Text += ((byte)(((ushort)crcResult) >> 8)).ToString("X2");
						txtSend.Text += " ";
						txtSend.Text += ((byte)crcResult).ToString("X2");
					}
				}
			}
		}

        #endregion

        #region 属性

        bool _autoThreadFlag;
        /// <summary>
        /// 自动发送线程标识
        /// </summary>
        public bool AutoThreadFlag
        {
            get { return _autoThreadFlag; }
            set { _autoThreadFlag = value; }
        }

        bool _autoSendAllThreadFlag;
        /// <summary>
        /// 多条指令自动发送线程标识
        /// </summary>
        public bool AutoSendAllThreadFlag
        {
            get { return _autoSendAllThreadFlag; }
            set { _autoSendAllThreadFlag = value; }
        }

        private DateTime _startTime;
        /// <summary>
        /// 连接开始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }


        string _port = Properties.Settings.Default.PORT;
        /// <summary>
        /// 串口号
        /// </summary>
        public string Port
        {
            get { return _port; }
            set { _port = value; }
        }

        int _baud;
        /// <summary>
        /// 波特率
        /// </summary>
        public int Baud
        {
            get { return _baud; }
            set { _baud = value; }
        }

        int _dataBits;
        /// <summary>
        /// 数据位
        /// </summary>
        public int DataBits
        {
            get { return _dataBits; }
            set { _dataBits = value; }
        }

        System.IO.Ports.StopBits _stopBits;
        /// <summary>
        /// 停止位
        /// </summary>
        public System.IO.Ports.StopBits StopBits
        {
            get { return _stopBits; }
            set { _stopBits = value; }
        }

        System.IO.Ports.Parity _parity;
        /// <summary>
        /// 校验位
        /// </summary>
        public System.IO.Ports.Parity Parity
        {
            get { return _parity; }
            set { _parity = value; }
        }

        System.Net.IPAddress _ip;
        /// <summary>
        /// IP地址
        /// </summary>
        public System.Net.IPAddress Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

        int _iPort;
        /// <summary>
        /// 网络通讯端口号
        /// </summary>
        public int IPort
        {
            get { return _iPort; }
            set { _iPort = value; }
        }

        int _sendCount;
        /// <summary>
        /// 发送数据长度
        /// </summary>
        public int SendCount
        {
            get { return _sendCount; }
            set { _sendCount = value; }
        }

        int _receivedCount;
        /// <summary>
        /// 接收数据长度
        /// </summary>
        public int ReceivedCount
        {
            get { return _receivedCount; }
            set { _receivedCount = value; }
        }

        List<string> _bigbuffer = new List<string>();
        /// <summary>
        /// 存储多命令集合
        /// </summary>
        public List<string> Bigbuffer
        {
            get { return _bigbuffer; }
            set { _bigbuffer = value; }
        }

        bool _showReceivedMessage;
        /// <summary>
        /// 接收数据显示线程控制
        /// </summary>
        public bool ShowReceivedMessage
        {
            get { return _showReceivedMessage; }
            set { _showReceivedMessage = value; }
        }

        object _bigdataLock = new object();
        List<byte> _bigdata = new List<byte>();
        /// <summary>
        /// 接收数据
        /// </summary>
        public List<byte> Bigdata
        {
            get 
            {
                lock (_bigdataLock)
                {
                    return _bigdata;
                }
            }
            set { _bigdata = value; }
        }

        bool _showThreadFlag;
        /// <summary>
        /// 接收数据标识
        /// </summary>
        public bool ShowThreadFlag
        {
            get { return _showThreadFlag; }
            set { _showThreadFlag = value; }
        }
        /// <summary>
        /// 控制文本Enable背景颜色不变
        /// </summary>
        public class RichTextBoxEx : RichTextBox
        {
            private Color backColor = Color.FromArgb(240, 240, 240);
            /// <summary>
            /// Enable的背景颜色
            /// </summary>
            public new Color BackColor
            {
                get { return backColor; }
                set { backColor = value; base.BackColor = value; }
            }

            private Color foreColor = Color.FromKnownColor(KnownColor.WindowText);
            /// <summary>
            /// Enable的字体颜色
            /// </summary>
            public new Color ForeColor
            {
                get { return foreColor; }
                set { foreColor = value; base.ForeColor = value; }
            }

            protected override void OnEnabledChanged(EventArgs e)
            {
                base.OnEnabledChanged(e);
                //设置Enable为false时
                if (this.Enabled)
                {
                    this.SetStyle(ControlStyles.UserPaint, false);
                }
                else
                    this.SetStyle(ControlStyles.UserPaint, true);
                this.Invalidate();
            }
            //描绘TextBox
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                System.Drawing.Brush b = new System.Drawing.SolidBrush(this.ForeColor);
                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;

                //绘制背景
                e.Graphics.Clear(BackColor);
                //绘制字符串
                e.Graphics.DrawString(this.Text, this.Font, b, -1, 1);
                //e.Graphics.DrawString(this.Text, this.Font, b, this.ClientRectangle, sf);
                b.Dispose();
            }

        }

		List<string> _configInfo = new List<string>();
		/// <summary>
		/// 配置文件内容集合
		/// </summary>
		public List<string> ConfigInfo
		{
			get { return _configInfo; }
			set { _configInfo = value; }
		}

		/// <summary>
		/// 是否ModbusCRC校验
		/// </summary>
		private bool isModbusCRC = false;

        #endregion

        #region 扩展命令框按钮事件

        /// <summary>
        /// 扩展矿按钮发送函数
        /// </summary>
        /// <param name="ckbIsHex"></param>
        /// <param name="txtCommandNum"></param>
        public void SendCommand(CheckBox ckbIsHex, TextBox txtCommandNum)
        {
            if (txtCommandNum.Text != "" && this.ClientInstance != null)
            {
                string cmdString;
                if (ckbIsHex.Checked)
                {
                    for (int i = txtCommandNum.TextLength - 1; i >= 0; i--)
                    {
                        if (txtCommandNum.Text[i] == ' ')
                            txtCommandNum.Text = txtCommandNum.Text.Remove(i, 1);
                        else
                            break;
                    }

                    string[] items = txtCommandNum.Text.Split(' ');
                    byte[] Bitems = new byte[items.Length];
                    // 16进制转2进制,然后发送
                    for (int i = 0; i < items.Length; i++)
                    {
                        byte item = byte.MaxValue;
                        byte.TryParse(items[i], System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out item);
                        Bitems[i] = item;
                    }
                    //iComm.iOneWayBinaryCommand cmd = new iComm.iOneWayBinaryCommand(ClientInstance, Bitems);
                    //cmd.Execute();
                    ClientInstance.ExetuteNonQuery(new Xmas11.Comm.Commander.iBinaryCommand(Bitems));
                    //是否显示时间
                    if (cboShowTime.SelectedIndex > 0)
                    {
                        if (cboShowTime.SelectedIndex == 1)
                        {
                            TimeSpan span = DateTime.Now - StartTime;
                            txtSendedCommand.AppendText(txtCommandNum.Text + string.Format("  Time:{0}", span.TotalMilliseconds.ToString()) + "\r\n");
                        }
                        if (cboShowTime.SelectedIndex == 2)
                        {
                            txtSendedCommand.AppendText(txtCommandNum.Text + string.Format("  Time:{0}:{1}:{2}:{3}", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString(), DateTime.Now.Millisecond.ToString()) + "\r\n");

                        }
                    }
                    else
                    {
                        txtSendedCommand.AppendText(txtSend.Text + "\r\n");
                    }
                    ckbHexSend.Checked = true;
                    txtSend.Text = txtCommandNum.Text;
                }
                else
                {
                    //1代表\0,2代表\r,3代表\n,4代表\r\n
					if (cboEOF.SelectedIndex == 1)
						cmdString = txtCommandNum.Text + "\0";
					else if (cboEOF.SelectedIndex == 2)
						cmdString = txtCommandNum.Text + "\r";
					else if (cboEOF.SelectedIndex == 3)
						cmdString = txtCommandNum.Text + "\n";
					else if (cboEOF.SelectedIndex == 4)
						cmdString = txtCommandNum.Text + "\r\n";
					else if (cboEOF.SelectedIndex == 5)
						cmdString = txtCommandNum.Text + "\n\r";
					else
						cmdString = txtCommandNum.Text;

                    //iComm.iOneWayTextCommand cmd = new iComm.iOneWayTextCommand(ClientInstance, cmdString);
                    //cmd.Execute();
                    ClientInstance.ExetuteNonQuery(new Xmas11.Comm.Commander.iTextCommand(cmdString));
                    //是否显示时间
                    if (cboShowTime.SelectedIndex > 0)
                    {
                        if (cboShowTime.SelectedIndex == 1)
                        {
                            TimeSpan span = DateTime.Now - StartTime;
                            txtSendedCommand.AppendText(txtCommandNum.Text + string.Format("  Time:{0}", span.TotalMilliseconds.ToString()) + "\r\n");
                        }
                        if (cboShowTime.SelectedIndex == 2)
                        {
                            txtSendedCommand.AppendText(txtCommandNum.Text + string.Format("  Time:{0}:{1}:{2}:{3}", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString(), DateTime.Now.Millisecond.ToString()) + "\r\n");

                        }
                    }
                    else
                    {
                        txtSendedCommand.AppendText(txtCommandNum.Text + "\r\n");
                    }
                    ckbHexSend.Checked = false;
                    txtSend.Text = txtCommandNum.Text;
                }
                this.SendCount += txtCommandNum.TextLength;
                lblSend.Text = string.Format("Send:{0}", this.SendCount.ToString());
            }
        }

        private void btnSend1_Click(object sender, EventArgs e)
        {
            SendCommand(ckbIsHex1, txtCommand1);
        }

        private void btnSend2_Click(object sender, EventArgs e)
        {
            SendCommand(ckbIsHex2, txtCommand2);
        }

        private void btnSend3_Click(object sender, EventArgs e)
        {
            SendCommand(ckbIsHex3, txtCommand3);
        }

        private void btnSend4_Click(object sender, EventArgs e)
        {
            SendCommand(ckbIsHex4, txtCommand4);
        }

        private void btnSend5_Click(object sender, EventArgs e)
        {
            SendCommand(ckbIsHex5, txtCommand5);
        }

        private void btnSend6_Click(object sender, EventArgs e)
        {
            SendCommand(ckbIsHex6, txtCommand6);
        }

        private void btnSend7_Click(object sender, EventArgs e)
        {
            SendCommand(ckbIsHex7, txtCommand7);
        }

        private void btnSend8_Click(object sender, EventArgs e)
        {
            SendCommand(ckbIsHex8, txtCommand8);
        }

        private void btnSend9_Click(object sender, EventArgs e)
        {
            SendCommand(ckbIsHex9, txtCommand9);
        }

        private void btnSend10_Click(object sender, EventArgs e)
        {
            SendCommand(ckbIsHex10, txtCommand10);
        }

		/// <summary>
		/// 扩展栏备注是否显示选项
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ckbShowRemarks_CheckedChanged(object sender, EventArgs e)
		{
			if (this.ckbShowRemarks.Checked)
			{
				this.lblRemark.Visible = true;

				this.txtRemark1.Visible = true;
				this.txtRemark2.Visible = true;
				this.txtRemark3.Visible = true;
				this.txtRemark4.Visible = true;
				this.txtRemark5.Visible = true;
				this.txtRemark6.Visible = true;
				this.txtRemark7.Visible = true;
				this.txtRemark8.Visible = true;
				this.txtRemark9.Visible = true;
				this.txtRemark10.Visible = true;

				this.txtCommand1.Width = this.txtCommand1.Width - 75;
				this.txtCommand2.Width = this.txtCommand2.Width - 75;
				this.txtCommand3.Width = this.txtCommand3.Width - 75;
				this.txtCommand4.Width = this.txtCommand4.Width - 75;
				this.txtCommand5.Width = this.txtCommand5.Width - 75;
				this.txtCommand6.Width = this.txtCommand6.Width - 75;
				this.txtCommand7.Width = this.txtCommand7.Width - 75;
				this.txtCommand8.Width = this.txtCommand8.Width - 75;
				this.txtCommand9.Width = this.txtCommand9.Width - 75;
				this.txtCommand10.Width = this.txtCommand10.Width - 75;
			}
			else
			{
				this.lblRemark.Visible = false;

				this.txtRemark1.Visible = false;
				this.txtRemark2.Visible = false;
				this.txtRemark3.Visible = false;
				this.txtRemark4.Visible = false;
				this.txtRemark5.Visible = false;
				this.txtRemark6.Visible = false;
				this.txtRemark7.Visible = false;
				this.txtRemark8.Visible = false;
				this.txtRemark9.Visible = false;
				this.txtRemark10.Visible = false;
				
				this.txtCommand1.Width = this.txtCommand1.Width + 75;
				this.txtCommand2.Width = this.txtCommand2.Width + 75;
				this.txtCommand3.Width = this.txtCommand3.Width + 75;
				this.txtCommand4.Width = this.txtCommand4.Width + 75;
				this.txtCommand5.Width = this.txtCommand5.Width + 75;
				this.txtCommand6.Width = this.txtCommand6.Width + 75;
				this.txtCommand7.Width = this.txtCommand7.Width + 75;
				this.txtCommand8.Width = this.txtCommand8.Width + 75;
				this.txtCommand9.Width = this.txtCommand9.Width + 75;
				this.txtCommand10.Width = this.txtCommand10.Width + 75;
			}
		}

        #endregion

        #region 扩展命令框选择事件
        /// <summary>
        /// 扩展文本16进制显示函数
        /// </summary>
        /// <param name="txtCommand"></param>
        /// <param name="check"></param>
        private void ChangeTXTShwo(TextBox txtCommand, bool check)
        {
            if (check && txtCommand.TextLength > 0)
            {
                byte[] items = System.Text.ASCIIEncoding.ASCII.GetBytes(txtCommand.Text.Replace("\n", string.Empty).Replace("\r", string.Empty));
                txtCommand.Clear();
                txtCommand.AppendText(string.Format(items[0].ToString("X2")));
                for (int i = 1; i < items.Length; i++)
                {
                    txtCommand.AppendText(string.Format(" " + items[i].ToString("X2")));
                }
            }
            else if (!check && txtCommand.TextLength > 0)
            {
                for (int i = txtCommand.TextLength - 1; i >= 0; i--)
                {
                    if (txtCommand.Text[i] == ' ')
                        txtCommand.Text = txtCommand.Text.Remove(i, 1);
                    else
                        break;
                }
                string[] items = txtCommand.Text.Split(' ');
                byte[] Bitems = new byte[items.Length];
                for (int i = 0; i < items.Length; i++)
                {

                    //NumberStyles
                    byte item = byte.MaxValue;
                    byte.TryParse(items[i], System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out item);
                    Bitems[i] = item;
                }
                string Result = System.Text.ASCIIEncoding.ASCII.GetString(Bitems);
                txtCommand.Clear();
                txtCommand.AppendText(Result);
            }
        }


        private void ckbIsHex1_CheckedChanged(object sender, EventArgs e)
        {
            ChangeTXTShwo(txtCommand1, ckbIsHex1.Checked);
        }

        private void ckbIsHex2_CheckedChanged(object sender, EventArgs e)
        {
            ChangeTXTShwo(txtCommand2, ckbIsHex2.Checked);
        }

        private void ckbIsHex3_CheckedChanged(object sender, EventArgs e)
        {
            ChangeTXTShwo(txtCommand3, ckbIsHex3.Checked);
        }

        private void ckbIsHex4_CheckedChanged(object sender, EventArgs e)
        {
            ChangeTXTShwo(txtCommand4, ckbIsHex4.Checked);
        }

        private void ckbIsHex5_CheckedChanged(object sender, EventArgs e)
        {
            ChangeTXTShwo(txtCommand5, ckbIsHex5.Checked);
        }

        private void ckbIsHex6_CheckedChanged(object sender, EventArgs e)
        {
            ChangeTXTShwo(txtCommand6, ckbIsHex6.Checked);
        }

        private void ckbIsHex7_CheckedChanged(object sender, EventArgs e)
        {
            ChangeTXTShwo(txtCommand7, ckbIsHex7.Checked);
        }

        private void ckbIsHex8_CheckedChanged(object sender, EventArgs e)
        {
            ChangeTXTShwo(txtCommand8, ckbIsHex8.Checked);
        }

        private void ckbIsHex9_CheckedChanged(object sender, EventArgs e)
        {
            ChangeTXTShwo(txtCommand9, ckbIsHex9.Checked);
        }

        private void ckbIsHex10_CheckedChanged(object sender, EventArgs e)
        {
            ChangeTXTShwo(txtCommand10, ckbIsHex10.Checked);
        }

        #endregion

        #region 多条指令自动发送



        /// <summary>
        /// 多条指令自动循环发送事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckbAutoSendAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbAutoSendAll.Checked)
            {
                AutoSendAllThreadFlag = true;
                AutoSendAllCommandThread = new Thread(new ThreadStart(AutoSendAllThreadStart));
                AutoSendAllCommandThread.IsBackground = true;
                AutoSendAllCommandThread.Start();
                txtSendAll.ReadOnly = true;
            }
            else
            {
                AutoSendAllThreadFlag = false;
                AutoSendAllCommandThread = null;
                txtSendAll.ReadOnly = false;
            }
        }

        /// <summary>
        /// 自动发送多条指令线程,25个指令循环判断一遍
        /// </summary>
        private void AutoSendAllThreadStart()
        {
            int AlltimeSpan = 0;
            while (AutoSendAllThreadFlag)
            {
                //默认睡眠时间1000毫秒
                if (!int.TryParse(txtSendAll.Text, out AlltimeSpan))
                {
                    AlltimeSpan = 1000;
                }
                //判断25条命令是否为空并且是否为HEX发送
                if (txtCommand1.TextLength > 0)
                {
                    SendCommand(ckbIsHex1, txtCommand1);
					if (!AutoSendAllThreadFlag)
						break;
                    System.Threading.Thread.Sleep(AlltimeSpan);
                }
                if (txtCommand2.TextLength > 0)
                {
                    SendCommand(ckbIsHex2, txtCommand2);
					if (!AutoSendAllThreadFlag)
						break;
                    System.Threading.Thread.Sleep(AlltimeSpan);
                }
                if (txtCommand3.TextLength > 0)
                {
                    SendCommand(ckbIsHex3, txtCommand3);
					if (!AutoSendAllThreadFlag)
						break;
                    System.Threading.Thread.Sleep(AlltimeSpan);
                }
                if (txtCommand4.TextLength > 0)
                {
                    SendCommand(ckbIsHex4, txtCommand4);
					if (!AutoSendAllThreadFlag)
						break;
                    System.Threading.Thread.Sleep(AlltimeSpan);
                }
                if (txtCommand5.TextLength > 0)
                {
                    SendCommand(ckbIsHex5, txtCommand5);
					if (!AutoSendAllThreadFlag)
						break;
                    System.Threading.Thread.Sleep(AlltimeSpan);
                }
                if (txtCommand6.TextLength > 0)
                {
                    SendCommand(ckbIsHex6, txtCommand6);
					if (!AutoSendAllThreadFlag)
						break;
                    System.Threading.Thread.Sleep(AlltimeSpan);
                }
                if (txtCommand7.TextLength > 0)
                {
                    SendCommand(ckbIsHex7, txtCommand7);
					if (!AutoSendAllThreadFlag)
						break;
                    System.Threading.Thread.Sleep(AlltimeSpan);
                }
                if (txtCommand8.TextLength > 0)
                {
                    SendCommand(ckbIsHex8, txtCommand8);
					if (!AutoSendAllThreadFlag)
						break;
                    System.Threading.Thread.Sleep(AlltimeSpan);
                }
                if (txtCommand9.TextLength > 0)
                {
                    SendCommand(ckbIsHex9, txtCommand9);
					if (!AutoSendAllThreadFlag)
						break;
                    System.Threading.Thread.Sleep(AlltimeSpan);
                }
                if (txtCommand10.TextLength > 0)
                {
                    SendCommand(ckbIsHex10, txtCommand10);
					if (!AutoSendAllThreadFlag)
						break;
                    System.Threading.Thread.Sleep(AlltimeSpan);
                }
            }
        }
        #endregion

        #region 保存所有配置
        /// <summary>
        /// 保存多条指令文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtSaveName.TextLength <= 0)
                MessageBox.Show(GetResources.iResource.GetString("ErrorSaveFile"));
            else
            {
                for (int i = 0; i < cboFileNames.Items.Count; i++)
                {
                    if (cboFileNames.Items[i].ToString() == txtSaveName.Text)
                    {
                        if (File.Exists(string.Format(Cache.FilePath + txtSaveName.Text)))
                        {
                            File.Delete(string.Format(Cache.FilePath + txtSaveName.Text));
                            cboFileNames.Items.RemoveAt(i);
                        }
                        break;
                    }
                }

                #region 25条指令合并一个XML文档
                //定义XML文档并添加根节点,根节点属性NAME表示文件名称
                XmlDocument DocSaveCommand = new XmlDocument();
                XmlElement root = DocSaveCommand.CreateElement("root");
                root.SetAttribute("name", txtSaveName.Text);
                DocSaveCommand.AppendChild(root);

                //第一条命令
                XmlElement Command1 = DocSaveCommand.CreateElement("Command1");
                Command1.SetAttribute("IsHex", ckbIsHex1.Checked.ToString());
                Command1.SetAttribute("Command", txtCommand1.Text);
				Command1.SetAttribute("Remark", txtRemark1.Text);
                root.AppendChild(Command1);

                //第二条命令
                XmlElement Command2 = DocSaveCommand.CreateElement("Command2");
                Command2.SetAttribute("IsHex", ckbIsHex2.Checked.ToString());
                Command2.SetAttribute("Command", txtCommand2.Text);
				Command2.SetAttribute("Remark", txtRemark2.Text);
                root.AppendChild(Command2);

                //第三条命令
                XmlElement Command3 = DocSaveCommand.CreateElement("Command3");
                Command3.SetAttribute("IsHex", ckbIsHex3.Checked.ToString());
                Command3.SetAttribute("Command", txtCommand3.Text);
				Command3.SetAttribute("Remark", txtRemark3.Text);
                root.AppendChild(Command3);

                //第四条命令
                XmlElement Command4 = DocSaveCommand.CreateElement("Command4");
                Command4.SetAttribute("IsHex", ckbIsHex4.Checked.ToString());
                Command4.SetAttribute("Command", txtCommand4.Text);
				Command4.SetAttribute("Remaek", txtRemark4.Text);
                root.AppendChild(Command4);

                //第五条命令
                XmlElement Command5 = DocSaveCommand.CreateElement("Command5");
                Command5.SetAttribute("IsHex", ckbIsHex5.Checked.ToString());
                Command5.SetAttribute("Command", txtCommand5.Text);
				Command5.SetAttribute("Remark", txtRemark5.Text);
                root.AppendChild(Command5);

                //第六条命令
                XmlElement Command6 = DocSaveCommand.CreateElement("Command6");
                Command6.SetAttribute("IsHex", ckbIsHex6.Checked.ToString());
                Command6.SetAttribute("Command", txtCommand6.Text);
				Command6.SetAttribute("Remark", txtRemark6.Text);
                root.AppendChild(Command6);

                //第七条命令
                XmlElement Command7 = DocSaveCommand.CreateElement("Command7");
                Command7.SetAttribute("IsHex", ckbIsHex7.Checked.ToString());
                Command7.SetAttribute("Command", txtCommand7.Text);
				Command7.SetAttribute("Remark", txtRemark7.Text);
                root.AppendChild(Command7);

                //第八条命令
                XmlElement Command8 = DocSaveCommand.CreateElement("Command8");
                Command8.SetAttribute("IsHex", ckbIsHex8.Checked.ToString());
                Command8.SetAttribute("Command", txtCommand8.Text);
				Command8.SetAttribute("Remark", txtRemark8.Text);
                root.AppendChild(Command8);

                //第九条命令
                XmlElement Command9 = DocSaveCommand.CreateElement("Command9");
                Command9.SetAttribute("IsHex", ckbIsHex9.Checked.ToString());
                Command9.SetAttribute("Command", txtCommand9.Text);
				Command9.SetAttribute("Remark", txtRemark9.Text);
                root.AppendChild(Command9);

                //第十条命令
                XmlElement Command10 = DocSaveCommand.CreateElement("Command10");
                Command10.SetAttribute("IsHex", ckbIsHex10.Checked.ToString());
                Command10.SetAttribute("Command", txtCommand10.Text);
				Command10.SetAttribute("Remark", txtRemark10.Text);
                root.AppendChild(Command10);

                string Bigstring = DocSaveCommand.InnerXml;


                #endregion 



                Cache.Save(Bigstring, txtSaveName.Text);
                cboFileNames.Items.Add(txtSaveName.Text);
                cboFileNames.SelectedItem = txtSaveName.Text;
            }
        }

        /// <summary>
        /// 删除当前多条指令文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cboFileNames.SelectedItem != null)
            {
                if (File.Exists(string.Format(Cache.FilePath + cboFileNames.SelectedItem.ToString())))
                {
                    File.Delete(string.Format(Cache.FilePath + cboFileNames.SelectedItem.ToString()));
                }
                else
                    MessageBox.Show(GetResources.iResource.GetString("ErrorDeleteFileExist"));
                txtCommand1.Clear();
				txtRemark1.Clear();
                ckbIsHex1.Checked = false;

                txtCommand2.Clear();
				txtRemark2.Clear();
                ckbIsHex2.Checked = false;

                txtCommand3.Clear();
				txtRemark3.Clear();
                ckbIsHex3.Checked = false;

                txtCommand4.Clear();
				txtRemark4.Clear();
                ckbIsHex4.Checked = false;

                txtCommand5.Clear();
				txtRemark5.Clear();
                ckbIsHex5.Checked = false;

                txtCommand6.Clear();
				txtRemark6.Clear();
                ckbIsHex6.Checked = false;

                txtCommand7.Clear();
				txtRemark7.Clear();
                ckbIsHex7.Checked = false;

                txtCommand8.Clear();
				txtRemark8.Clear();
                ckbIsHex8.Checked = false;

                txtCommand9.Clear();
				txtRemark9.Clear();
                ckbIsHex9.Checked = false;

                txtCommand10.Clear();
				txtRemark10.Clear();
                ckbIsHex10.Checked = false;

                cboFileNames.Items.RemoveAt(cboFileNames.SelectedIndex);
            }
            else
                MessageBox.Show(GetResources.iResource.GetString("ErrorDeleteFile"), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        /// <summary>
        /// 读取当前多条指令文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboFileNames_SelectedIndexChanged(object sender, EventArgs e)
        {
			try
			{
				if (cboFileNames.SelectedIndex >= 0)
				{
					string fileName = cboFileNames.SelectedItem.ToString();
					string Bigstring = null;
					try
					{
						Bigstring = (string)Cache.Load(fileName);
					}
					catch (Exception err)
					{
						MessageBox.Show(GetResources.iResource.GetString("ErrorReadFile") + err.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					if (Bigstring != null && Bigstring.Length > 0)
					{
						XmlDocument DocReadCommand = new XmlDocument();
						DocReadCommand.LoadXml(Bigstring);

						XmlNode root = DocReadCommand.FirstChild;
						if (root.ChildNodes.Count >= 25)
						{
							//文件名
							txtSaveName.Text = root.Attributes[0].Value;

							//第一条命令
							XmlNode Command1 = root.SelectSingleNode("Command1");
							if (Command1.Attributes[0].Value == "True")
								ckbIsHex1.Checked = true;
							else
								ckbIsHex1.Checked = false;
							txtCommand1.Text = Command1.Attributes[1].Value;
							txtRemark1.Text = Command1.Attributes[2].Value;

							//第二条命令
							XmlNode Command2 = root.SelectSingleNode("Command2");
							if (Command2.Attributes[0].Value == "True")
								ckbIsHex2.Checked = true;
							else
								ckbIsHex2.Checked = false;
							txtCommand2.Text = Command2.Attributes[1].Value;
							txtRemark2.Text = Command2.Attributes[2].Value;

							//第三条命令
							XmlNode Command3 = root.SelectSingleNode("Command3");
							if (Command3.Attributes[0].Value == "True")
								ckbIsHex3.Checked = true;
							else
								ckbIsHex3.Checked = false;
							txtCommand3.Text = Command3.Attributes[1].Value;
							txtRemark3.Text = Command3.Attributes[2].Value;

							//第四条命令
							XmlNode Command4 = root.SelectSingleNode("Command4");
							if (Command4.Attributes[0].Value == "True")
								ckbIsHex4.Checked = true;
							else
								ckbIsHex4.Checked = false;
							txtCommand4.Text = Command4.Attributes[1].Value;
							txtRemark4.Text = Command4.Attributes[2].Value;

							//第五条命令
							XmlNode Command5 = root.SelectSingleNode("Command5");
							if (Command5.Attributes[0].Value == "True")
								ckbIsHex5.Checked = true;
							else
								ckbIsHex5.Checked = false;
							txtCommand5.Text = Command5.Attributes[1].Value;
							txtRemark5.Text = Command5.Attributes[2].Value;

							//第六条命令
							XmlNode Command6 = root.SelectSingleNode("Command6");
							if (Command6.Attributes[0].Value == "True")
								ckbIsHex6.Checked = true;
							else
								ckbIsHex6.Checked = false;
							txtCommand6.Text = Command6.Attributes[1].Value;
							txtRemark6.Text = Command6.Attributes[2].Value;

							//第七条命令
							XmlNode Command7 = root.SelectSingleNode("Command7");
							if (Command7.Attributes[0].Value == "True")
								ckbIsHex7.Checked = true;
							else
								ckbIsHex7.Checked = false;
							txtCommand7.Text = Command7.Attributes[1].Value;
							txtRemark7.Text = Command7.Attributes[2].Value;

							//第八条命令
							XmlNode Command8 = root.SelectSingleNode("Command8");
							if (Command8.Attributes[0].Value == "True")
								ckbIsHex8.Checked = true;
							else
								ckbIsHex8.Checked = false;
							txtCommand8.Text = Command8.Attributes[1].Value;
							txtRemark8.Text = Command8.Attributes[2].Value;

							//第九条命令
							XmlNode Command9 = root.SelectSingleNode("Command9");
							if (Command9.Attributes[0].Value == "True")
								ckbIsHex9.Checked = true;
							else
								ckbIsHex9.Checked = false;
							txtCommand9.Text = Command9.Attributes[1].Value;
							txtRemark9.Text = Command9.Attributes[2].Value;

							//第十条命令
							XmlNode Command10 = root.SelectSingleNode("Command10");
							if (Command10.Attributes[0].Value == "True")
								ckbIsHex10.Checked = true;
							else
								ckbIsHex10.Checked = false;
							txtCommand10.Text = Command10.Attributes[1].Value;
							txtRemark10.Text = Command10.Attributes[2].Value;

						}
						else
							MessageBox.Show(GetResources.iResource.GetString("ErrorReadFile"), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
				}
			}
			catch 
			{
				MessageBox.Show(GetResources.iResource.GetString("ErrorReadFile"), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

        }

        /// <summary>
        /// 存储文本框内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveReceived_Click(object sender, EventArgs e)
        {
            if (txtReceived.Text == string.Empty)
            {
                MessageBox.Show(GetResources.iResource.GetString("ErrorSaveDataText"), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                saveFileDialog1.Filter = "文本文件(*.txt)|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, true);
                    sw.WriteLine(string.Format("接受文本\r\n\r\n") + txtReceived.Text);
                    sw.WriteLine(string.Format("发送文本\r\n\r\n") + txtSendedCommand.Text);
                    sw.Close();
                }
            }
        }

        /// <summary>
        /// 读取文本框内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenReceived_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "文本文件(*.txt)|*.txt|二进制文件(*.bin)|*.bin|十六进制文件(*.hex)|*.hex|所有文件(*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtReceived.Text = string.Empty;
                StreamReader sr = new StreamReader(openFileDialog1.FileName);
                txtReceived.Text = sr.ReadToEnd();
                sr.Close();

                txtReceivedHex.Text = string.Empty;
                Stream ss = new FileStream(openFileDialog1.FileName, FileMode.Open);
                txtReceivedHex.AppendText(string.Format("文件大小为{0}B,以下为预览4KB内容\r\n",ss.Length.ToString()));
                byte[] array = new byte[4096];
                ss.Read(array, 0, 4096);
                ss.Close();
                ss.Dispose();
                for (int i = 0; i < 4096; i++) 
                {
                    txtReceivedHex.AppendText(string.Format("{0} ", array[i]));
                }
                lblMessage.ForeColor = Color.Black;
                lblMessage.Text =string.Format("打开文件:" + openFileDialog1.FileName);
            }
        }

        /// <summary>
        /// 发送文件按钮函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendFile_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "文本文件(*.txt)|*.txt|二进制文件(*.bin)|*.bin|十六进制文件(*.hex)|*.hex|所有文件(*.*)|*.*";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                
                Stream stream = new FileStream(openFileDialog2.FileName, FileMode.Open);
                byte[] array = new byte[stream.Length];
                stream.Read(array, 0, array.Length);
                stream.Close();
                stream.Dispose();

                if (ClientInstance!=null)
                {
                    if (ClientInstance.IsOpen)
                    {
                        //Comm.Commander.iComm.iOneWayBinaryCommand cmd = new iComm.iOneWayBinaryCommand(ClientInstance, array);
                        //cmd.Execute();
                        ClientInstance.ExetuteNonQuery(new Xmas11.Comm.Commander.iBinaryCommand(array));
                        

                        txtSendedCommand.AppendText(string.Format("文件大小为{0}B,以下为预览4KB内容\r\n", array.Length.ToString()));
                        for (int i = 0; i < 4096; i++)
                        {
                            txtSendedCommand.AppendText(string.Format("{0} ", array[i].ToString()));
                            this.SendCount += 2;
                            lblSend.Text = string.Format("Send:{0}", this.SendCount.ToString());
                        }

                        //是否显示时间
                        if (cboShowTime.SelectedIndex > 0)
                        {
                            if (cboShowTime.SelectedIndex == 1)
                            {
                                TimeSpan span = DateTime.Now - StartTime;
                                txtSendedCommand.AppendText( string.Format("  Time:{0}", span.TotalMilliseconds.ToString()) + "\r\n");
                            }
                            if (cboShowTime.SelectedIndex == 2)
                            {
                                txtSendedCommand.AppendText( string.Format("  Time:{0}:{1}:{2}:{3}", DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), DateTime.Now.Second.ToString(), DateTime.Now.Millisecond.ToString()) + "\r\n");

                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show(GetResources.iResource.GetString("ErrorNoClient"), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
 
                    }
                }
                else
                {
                    MessageBox.Show(GetResources.iResource.GetString("ErrorNoClient"), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// 清空多条指令但不删除文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            txtCommand1.Clear();
			txtRemark1.Clear();
            ckbIsHex1.Checked = false;

            txtCommand2.Clear();
			txtRemark2.Clear();
            ckbIsHex2.Checked = false;

            txtCommand3.Clear();
			txtRemark3.Clear();
            ckbIsHex3.Checked = false;

            txtCommand4.Clear();
			txtRemark4.Clear();
            ckbIsHex4.Checked = false;

            txtCommand5.Clear();
			txtRemark5.Clear();
            ckbIsHex5.Checked = false;

            txtCommand6.Clear();
			txtRemark6.Clear();
            ckbIsHex6.Checked = false;

            txtCommand7.Clear();
			txtRemark7.Clear();
            ckbIsHex7.Checked = false;

            txtCommand8.Clear();
			txtRemark8.Clear();
            ckbIsHex8.Checked = false;

            txtCommand9.Clear();
			txtRemark9.Clear();
            ckbIsHex9.Checked = false;

            txtCommand10.Clear();
			txtRemark10.Clear();
            ckbIsHex10.Checked = false;

            this.cboFileNames.SelectedIndex = -1;

            txtSaveName.Clear();
        }

        #endregion

        #region 帮助文本
        /// <summary>
        /// 帮助文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHelpDevice_Click(object sender, EventArgs e)
        {
            txtReceived.Clear();

            #region 帮助文本已放在资源文件里
            //string HelpDevice = string.Format
            //    (
            //        "实现功能:  \r\n\r\n1.可根据需要选取串口通讯,网络通讯和USB通讯三种通讯方式.\r\n2.接收串口，网络和USB线传来的数据并进行显示.\r\n3.所接收到的数据显示方式可以"
            //+"选择字符方式或者HEX方式.\r\n4.中文显示无乱码,且不影响速度.\r\n5.串口通讯可以选择各个参数,并且串口设置和字符串操作等设置在程序关闭时会自动保存,打开时自动载入.网络通讯可以写入IP和端口,如果格式错误连接会出错\r\n"
            //+"6.可以发送HEX命令,但需要写好发送格式并勾选HEX发送.\r\n7.可以定时重复发送数据,并可以设置发送时间间隔.\r\n8.可以在发送字符串时选择发送新行,即自动加上回车换行.\r\n"
            //+"9.可以将接收文本框中的内容存储起来,也可以打开一个TEXT文档将内容读取到接收文本框.\r\n10.可以即时显示发送的字节数和接收的字节数,按清除窗口会清零.\r\n"
            //+"11.扩展功能,多条命令字符串定义,每条字符串定义为字符串方式,可以以HEX发送.\r\n12.点击字符串右边的编号即可发送这条字符串.\r\n13.可以设置为循环发送你定义过的"
            //+"字符串,并且可以设置发送时间间隔.\r\n14.可以存储多条字符串命令,在扩展框下方的文本框中输入文件名点保存即可存储,存好的文件名会出现在顶端的下拉框中.\r\n"
            //+"15.选择顶端下拉框中的某一文件名会将文件中的内容按照格式转化为多条字符串命令,利于下次操作.\r\n16.可以选择接受数据间隔,提供空格,回车,Tab格3种间隔方式\r\n17.存储多条命令时如果文件名存在将默认更新文件.\r\n"
            //+"\r\n存在问题:\r\n1.由于处理机制有点慢,在自动发送间隔低于50ms时,发送结束后会有一部分数据尚未显示,需要一定缓冲时间.\r\n2.由于TextBox有显示上限,所以在工作时为了保证软件运行,在TextBox将要达到上限时"
            //+"采取了自动保存并删除文本框内容的方法,保存文件路径为当前程序目录下,名字为保存时间和显示方式,请注意.");
            #endregion

            string HelpDevice = GetResources.iResource.GetString("HelpInfomation");
            txtReceived.Text = HelpDevice;
        }
        #endregion







		





		

    }
}
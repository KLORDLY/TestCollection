using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpServerApp
{
    public struct FileProtocol
    {
        private readonly FileRequestMode mode;
        private readonly int port;
        private readonly string flieName;

        public FileProtocol(FileRequestMode mode, int port, string fileName)
        {
            this.mode = mode;
            this.port = port;
            this.flieName = fileName;
        }

        public FileRequestMode Mode
        {
            get { return mode; }
        }

        public int Port
        {
            get { return port; }
        }

        public string FileName
        {
            get { return flieName; }
        }

        public override string ToString()
        {
            return String.Format("<protocol><file name=\"{0}\" mode=\"{1}\" port=\"{2}\" /></protocol>", flieName, mode, port);
        }
    }

    public enum FileRequestMode
    {
        Send = 0,
        Receive
    }
}

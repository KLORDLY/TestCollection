using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceTest
{
    public delegate void OnCommandEventHandler();
    public class StringCommand
    {
        uint index = 0;

        public uint Index
        {
            get { return index; }
            set { index = value; }
        }

        string command = string.Empty;
        public string Command
        {
            get { return command; }
        }
        OnCommandEventHandler handler = null;
        public OnCommandEventHandler Handler
        {
            get { return handler; }
        }
        public StringCommand(uint idx,string cmd, OnCommandEventHandler eventHandler)
        {
            index = idx;
            command = cmd;
            handler = eventHandler;
        }
    }
}

    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceTest
{
    public partial class StringCmd
    {
        private static List<StringCommand> CommandHandler = new List<StringCommand>();
        public StringCmd()
        {
            CommandHandler.Add(new StringCommand(0, "R:OTYPE", new OnCommandEventHandler(ReadDeviceType)));
        }

        public static void DealCommands(string cmd)
        {
            if (cmd == CommandHandler[0].Command)
            {
                CommandHandler[0].Handler();
            }
        }
    }
}

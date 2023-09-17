using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faulhaber.Core.Common
{
    public class CompositeMessage : IMessage
    {
        private List<IMessage> commands = new List<IMessage>();

        public CompositeMessage(ISerialClient receiver = null)
        {
        }

        public void Add(IMessage c)
        {
            if (c != null)
            {
                commands.Add(c);
            }
        }

        public void Execute()
        {
            foreach (var c in commands)
            {
                c.Execute();
            }
        }
        public  int Index()
        {
            return -1;
        }
        public byte SubIndex()
        {
            return 0x0;
        }

        public object Value()
        {
            return null;
        }
    }
}

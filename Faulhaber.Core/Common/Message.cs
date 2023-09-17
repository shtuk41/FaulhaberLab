using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faulhaber.Core.Common
{
    public abstract class Message<T> : IMessage
    {
        private ISerialClient rec;
        private bool canExecute = true;

        private int index;
        private byte subIndex;
        private T val;

        public Message(int index, byte subIndex, T val)
        {
            canExecute = false;

            this.index = index;
            this.subIndex = subIndex;
            this.val = val;
        }

        public Message(ISerialClient receiver, int index, byte subIndex)
        {
            this.rec = receiver;
            this.index = index;
            this.subIndex = subIndex;
        }

        protected ISerialClient Receiver
        {
            get
            {
                return this.rec;
            }
        }

        protected bool CanExecute
        {
            get
            {
                return canExecute;
            }
         }

        public int Index()
        {
            return index;
        }

        public byte SubIndex()
        {
            return subIndex;
        }

        public object Value()
        {
            return val;
        }

        public abstract void Execute();

        public byte ComputeCRC(byte u8byte, byte u8CRC)
        {
            byte polynomial = 0xD5;
            byte i;
            u8CRC = (byte)(u8CRC ^ u8byte);

            for (i = 0; i < 8; i++)
            {
                if ((u8CRC & 0x01) != 0)
                {
                    u8CRC = (byte)((u8CRC >> 1) ^ polynomial);
                }
                else
                {
                    u8CRC >>= 1;
                }
            }

            return u8CRC;
        }
    }
}

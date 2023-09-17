using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faulhaber.Core.Common;

namespace Faulhaber.Core.Messages
{
    public class W1018Identity : Message<int>
    {
        byte byteSOF = (byte)'S';
        byte byteUserDataLength = 7;
        byte byteNodeNumber = 0;
        byte byteCommandCode = 0x01;
        byte byteIndexLB = 0x18;
        byte byteIndexHB = 0x10;
        byte byteEOF = (byte)'E';

        byte[] message = new byte[9];

        public W1018Identity(byte subIndex, int val) : base(0x1018, subIndex, val)
        {
        }

        public W1018Identity(ISerialClient receiver, byte subIndex) : base(receiver, 0x1018, subIndex)
        {
            message[0] = byteSOF;
            message[1] = byteUserDataLength;
            message[2] = byteNodeNumber;
            message[3] = byteCommandCode;
            message[4] = byteIndexLB;
            message[5] = byteIndexHB;
            message[6] = subIndex;
            message[8] = byteEOF;

            byte y8CRC = 0xFF;

            for (int ii = 1; ii < 7; ii++)
            {
                y8CRC = ComputeCRC(message[ii], y8CRC);
            }

            message[7] = y8CRC;
        }

        public override void Execute()
        {
            if (CanExecute)
            {
                Receiver.SendByteBuffer(message);
            }
        }
    }

    public static class Identity1018Subindex
    {
        public static byte NumberOfEntries = 0x00;
        public static byte VendorID = 0x01;
        public static byte ProductCode = 0x02;
        public static byte RevisionNumber = 0x03;
        public static byte SerialNumber = 0x04;
    }
}

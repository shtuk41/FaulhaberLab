using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faulhaber.Core.Common;

namespace Faulhaber.Core.Messages
{
    public class W2311DigitalIOStatus : Message<byte>
    {
        byte byteSOF = (byte)'S';
        byte byteUserDataLength = 7;
        byte byteNodeNumber = 0;
        byte byteCommandCode = 0x01;
        byte byteIndexLB = 0x11;
        byte byteIndexHB = 0x23;
        byte byteEOF = (byte)'E';

        byte[] message = new byte[9];

        public W2311DigitalIOStatus(byte subIndex, byte val) : base(0x2311, subIndex, val)
        {
        }

        public W2311DigitalIOStatus(ISerialClient receiver, byte subIndex) : base(receiver, 0x2311, subIndex)
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

    public static class W2311DigialIOStatusSubIndexes
    {
        public const byte NumberOfEntries = 0x00;
        public const byte DigitalInputLogicalState = 0x01;
        public const byte DigitalInputPhysicalState = 0x02;
        public const byte DigitalOutputStatus = 0x03;
    }
}

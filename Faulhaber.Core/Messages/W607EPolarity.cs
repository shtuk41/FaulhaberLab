using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faulhaber.Core.Common;

namespace Faulhaber.Core.Messages
{
    public class W607EPolarity : Message<byte>
    {
        byte byteSOF = (byte)'S';
        byte byteUserDataLength = 8;
        byte byteNodeNumber = 0;
        byte byteCommandCode = 0x02;
        byte byteIndexLB = 0x7E;
        byte byteIndexHB = 0x60;
        byte byteEOF = (byte)'E';

        byte[] message = new byte[10];

        public W607EPolarity(byte val) : base(0x6060, 0x00, val)
        {
        }

        public W607EPolarity(ISerialClient receiver, byte value) : base(receiver, 0x6060, 0x00)
        {
            message[0] = byteSOF;
            message[1] = byteUserDataLength;
            message[2] = byteNodeNumber;
            message[3] = byteCommandCode;
            message[4] = byteIndexLB;
            message[5] = byteIndexHB;
            message[6] = 0x00; //subindex
            message[7] = value;
            message[9] = byteEOF;

            byte y8CRC = 0xFF;

            for (int ii = 1; ii < 8; ii++)
            {
                y8CRC = ComputeCRC(message[ii], y8CRC);
            }

            message[8] = y8CRC;
        }

        public override void Execute()
        {
            if (CanExecute)
            {
                Receiver.SendByteBuffer(message);
            }
        }
    }

    public static class W607EPolarityValues
    {
        public static byte InvertPosition = 0x80;
        public static byte InvertSpeed = 0x40;
    }
}

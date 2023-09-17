using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faulhaber.Core.Common;

namespace Faulhaber.Core.Messages 
{
    public class W60E1NegativeTorqueLimitValue : Message<ushort>
    {
        byte byteSOF = (byte)'S';
        byte byteUserDataLength = 9;
        byte byteNodeNumber = 0;
        byte byteCommandCode = 0x02;
        byte byteIndexLB = 0xE1;
        byte byteIndexHB = 0x60;
        byte byteEOF = (byte)'E';

        byte[] message = new byte[11];

        public W60E1NegativeTorqueLimitValue(ushort val) : base(0x60E1, 0x00, val)
        {
        }

        public W60E1NegativeTorqueLimitValue(ISerialClient receiver, ushort val) : base(receiver, 0x60E1, 0)
        {
            message[0] = byteSOF;
            message[1] = byteUserDataLength;
            message[2] = byteNodeNumber;
            message[3] = byteCommandCode;
            message[4] = byteIndexLB;
            message[5] = byteIndexHB;
            message[6] = 0x00; //subindex
            message[7] = (byte)(val & 0x00FF);
            message[8] = (byte)((val & 0xFF00) >> 8);
            message[10] = byteEOF;

            byte y8CRC = 0xFF;

            for (int ii = 1; ii < 9; ii++)
            {
                y8CRC = ComputeCRC(message[ii], y8CRC);
            }

            message[9] = y8CRC;
        }

        public override void Execute()
        {
            Receiver.SendByteBuffer(message);
        }
    }
}

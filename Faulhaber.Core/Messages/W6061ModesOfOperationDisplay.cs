using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faulhaber.Core.Common;

namespace Faulhaber.Core.Messages
{
    public class W6061ModesOfOperationDisplay : Message<sbyte>
    {
        byte byteSOF = (byte)'S';
        byte byteUserDataLength = 7;
        byte byteNodeNumber = 0;
        byte byteCommandCode = 0x01;
        byte byteIndexLB = 0x61;
        byte byteIndexHB = 0x60;
        byte byteEOF = (byte)'E';

        byte[] message = new byte[9];

        public W6061ModesOfOperationDisplay(sbyte val) : base(0x6061, 0x00, val)
        {
        }

        public W6061ModesOfOperationDisplay(ISerialClient receiver) : base(receiver, 0x6061, 0x00)
        {
            message[0] = byteSOF;
            message[1] = byteUserDataLength;
            message[2] = byteNodeNumber;
            message[3] = byteCommandCode;
            message[4] = byteIndexLB;
            message[5] = byteIndexHB;
            message[6] = 0x00;//subindex
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
}


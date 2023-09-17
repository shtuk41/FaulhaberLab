using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faulhaber.Core.Common;

namespace Faulhaber.Core.Messages
{
    public class W6085QuickStopDeceleration : Message<uint>
    {
        byte byteSOF = (byte)'S';
        byte byteUserDataLength = 11;
        byte byteNodeNumber = 0;
        byte byteCommandCode = 0x02;
        byte byteIndexLB = 0x85;
        byte byteIndexHB = 0x60;
        byte byteEOF = (byte)'E';

        byte[] message = new byte[13];

        public W6085QuickStopDeceleration(ISerialClient receiver, uint value) : base(receiver, 0x6085, 0x00)
        {
            message[0] = byteSOF;
            message[1] = byteUserDataLength;
            message[2] = byteNodeNumber;
            message[3] = byteCommandCode;
            message[4] = byteIndexLB;
            message[5] = byteIndexHB;
            message[6] = 0x00; //subindex
            message[7] = (byte)(value & 0x000000FF);
            message[8] = (byte)((value & 0x0000FF00) >> 8);
            message[9] = (byte)((value & 0x00FF00FF) >> 16);
            message[10] = (byte)((value & 0xFF000000) >> 24);

            message[12] = byteEOF;

            byte y8CRC = 0xFF;

            for (int ii = 1; ii < 11; ii++)
            {
                y8CRC = ComputeCRC(message[ii], y8CRC);
            }

            message[11] = y8CRC;
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

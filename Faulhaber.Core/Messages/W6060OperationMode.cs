using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faulhaber.Core.Common;

namespace Faulhaber.Core.Messages
{
    public class W6060OperationMode : Message<sbyte>
    {
        byte byteSOF = (byte)'S';
        byte byteUserDataLength = 8;
        byte byteNodeNumber = 0;
        byte byteCommandCode = 0x02;
        byte byteIndexLB = 0x60;
        byte byteIndexHB = 0x60;
        byte byteEOF = (byte)'E';

        byte[] message = new byte[10];

        public W6060OperationMode(sbyte val) : base(0x6060, 0x00, val)
        {
        }

        public W6060OperationMode(ISerialClient receiver, sbyte value) : base(receiver, 0x6060, 0x00)
        {
            message[0] = byteSOF;
            message[1] = byteUserDataLength;
            message[2] = byteNodeNumber;
            message[3] = byteCommandCode;
            message[4] = byteIndexLB;
            message[5] = byteIndexHB;
            message[6] = 0x00; //subindex
            message[7] = (byte)value;
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

    public static class OperationMode6060VaLues
    {
        public static sbyte ATC = -4;
        public static sbyte AVC = -3;
        public static sbyte APC = -2;
        public static sbyte VoltageMode = -1;
        public static sbyte ControllerNotActivated = 0;
        public static sbyte PP = 1;
        public static sbyte PV = 3;
        public static sbyte Homing = 6;
        public static sbyte CSP = 8;
        public static sbyte CSV = 9;
        public static sbyte CST = 10;

    }
}

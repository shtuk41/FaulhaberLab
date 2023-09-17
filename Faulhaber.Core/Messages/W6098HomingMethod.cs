using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faulhaber.Core.Common;

namespace Faulhaber.Core.Messages
{
    public class W6098HomingMethod : Message<sbyte>
    {
        byte byteSOF = (byte)'S';
        byte byteUserDataLength = 8;
        byte byteNodeNumber = 0;
        byte byteCommandCode = 0x02;
        byte byteIndexLB = 0x98;
        byte byteIndexHB = 0x60;
        byte byteEOF = (byte)'E';

        byte[] message = new byte[10];

        public W6098HomingMethod(sbyte val) : base(0x6098, 0x00, val)
        {
        }

        public W6098HomingMethod(ISerialClient receiver, sbyte val) : base(receiver, 0x6098, 0x00)
        {
            message[0] = byteSOF;
            message[1] = byteUserDataLength;
            message[2] = byteNodeNumber;
            message[3] = byteCommandCode;
            message[4] = byteIndexLB;
            message[5] = byteIndexHB;
            message[6] = 0x00; //subindex
            message[7] = (byte)val;
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

    public static class OperationMode6098Values
    {
        public static sbyte Method4Neg = -4;
        public static sbyte Method3Neg = -3;
        public static sbyte Method2Neg = -2;
        public static sbyte Method1Neg = -1;
        
        public static sbyte Method1 = 1;
        public static sbyte Method2 = 2;
        public static sbyte Method3 = 3;
        public static sbyte Method4 = 4;
        public static sbyte Method5 = 5;
        public static sbyte Method6 = 6;
        public static sbyte Method7 = 7;
        public static sbyte Method8 = 8;
        public static sbyte Method9 = 9;
        public static sbyte Method10 = 10;
        public static sbyte Method11 = 11;
        public static sbyte Method12 = 12;
        public static sbyte Method13 = 13;
        public static sbyte Method14 = 14;
        public static sbyte Method15 = 15;
        public static sbyte Method16 = 16;
        public static sbyte Method17 = 17;
        public static sbyte Method18 = 18;
        public static sbyte Method19 = 19;
        public static sbyte Method20 = 20;
        public static sbyte Method21 = 21;
        public static sbyte Method22 = 22;
        public static sbyte Method23 = 23;
        public static sbyte Method24 = 24;
        public static sbyte Method25 = 25;
        public static sbyte Method26 = 26;
        public static sbyte Method27 = 27;
        public static sbyte Method28 = 28;
        public static sbyte Method29 = 29;
        public static sbyte Method30 = 30;
        public static sbyte Method31 = 31;
        public static sbyte Method32 = 32;
        public static sbyte Method33 = 33;
        public static sbyte Method34 = 34;

        public static sbyte Method37 = 37;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faulhaber.Core.Common;

namespace Faulhaber.Core.Messages
{
    public class W2310Inputs : Message<int>
    {
        byte byteSOF = (byte)'S';
        byte byteUserDataLength = 8;
        byte byteNodeNumber = 0;
        byte byteCommandCode = 0x02;
        byte byteIndexLB = 0x10;
        byte byteIndexHB = 0x23;
        byte byteEOF = (byte)'E';

        byte[] message;

        public W2310Inputs(int val) : base(0x2310, 0x00, val)
        {
        }

        public W2310Inputs(ISerialClient receiver, byte subIndex, byte value) : base(receiver, 0x2310, subIndex)
        {
            message = new byte[10];

            message[0] = byteSOF;
            message[1] = byteUserDataLength;
            message[2] = byteNodeNumber;
            message[3] = byteCommandCode;
            message[4] = byteIndexLB;
            message[5] = byteIndexHB;
            message[6] = subIndex;
            message[7] = (byte)value;
            message[9] = byteEOF;

            byte y8CRC = 0xFF;

            for (int ii = 1; ii < 8; ii++)
            {
                y8CRC = ComputeCRC(message[ii], y8CRC);
            }

            message[8] = y8CRC;
        }

        public W2310Inputs(ISerialClient receiver, short val) : base(receiver, 2310, 0x03)
        {
            message = new byte[11];

            message[0] = byteSOF;
            message[1] = byteUserDataLength;
            message[2] = byteNodeNumber;
            message[3] = byteCommandCode;
            message[4] = byteIndexLB;
            message[5] = byteIndexHB;
            message[6] = 0x06;
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
            if (CanExecute)
            {
                Receiver.SendByteBuffer(message);
            }
        }
    }

    public static class HomingLimitSwitch2310SubIndex
    {
        public static byte SelectLowerLimitSwitch = 0x01;
        public static byte SelectUpperLimitSwitchInputs = 0x02;
        public static byte LimitSwitchOptionCode = 0x03;
        public static byte SelectReferenceSwitchInput = 0x04;
        public static byte EmulatedInputThresholdLevel = 0x05;
        public static byte InputPolarity = 0x10;
        public static byte InputThresholdLevel = 0x11;
        public static byte InputFilterActive = 0x12;
    }

    public static class HomingLimitSwitch2310Inputs
    {
        public static byte In8 = 0x80;
        public static byte In7 = 0x40;
        public static byte In6 = 0x20;
        public static byte In5 = 0x10;
        public static byte In4 = 0x08;
        public static byte In3 = 0x04;
        public static byte In2 = 0x02;
        public static byte In1 = 0x01;
    }

    public static class SubIndex0x03
    {
        public static short DriveComesToStandstillPowerlessly = 0;
        public static short BrakeARamp = 1;
        public static short QuickStop = 2;
        public static short StopAtMaxCurrent = 3;
        public static short StopWithVoltage0 = 4;
    }


}

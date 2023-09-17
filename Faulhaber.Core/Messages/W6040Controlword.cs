using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Faulhaber.Core.Common;

namespace Faulhaber.Core.Messages
{
    public class W6040Controlword : Message<int>
    {
        byte byteSOF = (byte)'S';
        byte byteUserDataLength = 6;
        byte byteNodeNumber = 0;
        byte byteCommandCode = 0x04;
        byte byteEOF = (byte)'E';

        byte[] command = new byte[8];

        public W6040Controlword(int val) : base(0x6040, 0x00, val)
        {
        }

        public W6040Controlword(ISerialClient receiver, ushort controlValue) : base(receiver, 0x6040, 0)
        {
            command[0] = byteSOF;
            command[1] = byteUserDataLength;
            command[2] = byteNodeNumber;
            command[3] = byteCommandCode;
            command[4] = (byte)(controlValue & 0x00FF);
            command[5] = (byte)((controlValue & 0xFF00) >> 8);
            command[7] = byteEOF;

            byte y8CRC = 0xFF;

            for (int ii = 1; ii < 6; ii++)
            {
                y8CRC = ComputeCRC(command[ii], y8CRC);
            }

            command[6] = y8CRC;
        }

        public override void Execute()
        {
            Receiver.SendByteBuffer(command);
        }
    }

    public static class Controlword6040Commands
    {
        //0 - no voltage present
        //1 - power supply activated
        public static ushort SwitchOnBit = 0x01;

        //0 - drive switched off
        //1 - drive ready
        public static ushort EnableVoltage = 0x02;

        //0 - quickstop enabled
        //1 - quick stop disabled
        public static ushort QuickStop = 0x04;

        //0 - operation disabled
        //1 - operation enabled
        public static ushort EnableOperation = 0x08;

        //0 -> 1 - fault resets on raising edge
        public static ushort FaultReset = 0x80;

        //0 - movement can be executed
        //1 -stop drive
        public static ushort Halt = 0x100;
 
        public static ushort ShutdownCommand = (ushort)(EnableVoltage | QuickStop);
        public static ushort SwitchOnCommand = (ushort)(SwitchOnBit | EnableVoltage | QuickStop);
        public static ushort DisableVoltageCommand = 0x0000;
        public static ushort QuickStopCommand = EnableVoltage;
        public static ushort DisableOperationCommand = (ushort)(SwitchOnBit | EnableVoltage | QuickStop);
        public static ushort EnableOperationCommand = (ushort)(EnableOperation | SwitchOnBit | EnableVoltage | QuickStop);
        public static ushort FaultResetTransition0Command = 0x0000;
        public static ushort FaultResetTransition1Command = FaultReset;
        public static ushort HaltCommand = Halt;
    }
}

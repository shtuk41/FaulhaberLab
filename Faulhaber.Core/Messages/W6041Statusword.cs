using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Faulhaber.Core.Common;

namespace Faulhaber.Core.Messages
{
    public class W6041Statusword : Message<ushort>
    {
        #region fields for writing

        private byte byteSOF = (byte)'S';
        private byte byteUserDataLength = 7;
        private byte byteNodeNumber = 0;
        private byte byteCommandCode = 0x01;
        private byte byteIndexLB = 0x41;
        private byte byteIndexHB = 0x60;
        private byte byteEOF = (byte)'E';

        private byte[] command = new byte[9];

        #endregion

        #region fields for reading

        private bool readyToSwitchOn;
        private bool switchedOn;
        private bool operationEnabled;
        private bool fault;
        private bool voltageEnabledNotImplemented;
        private bool quickStop;
        private bool switchOnDisabled;
        private bool warning;
        private bool operationModeSpecificBit10;
        private bool internalLimitActive;
        private bool operationModeSpecificBit12;
        private bool operationModeSpecificBit13;
        private bool configurableBit14;
        private bool configurableBit15;

        #endregion

        public W6041Statusword(ushort val) : base(0x6041, 0x00, val)
        {
            readyToSwitchOn = (val & 0x0001) == 0x0001;
            switchedOn = (val & 0x0002) == 0x0002;
            operationEnabled = (val & 0x0004) == 0x0004;
            fault = (val & 0x0008) == 0x0008;
            voltageEnabledNotImplemented = (val & 0x0010) == 0x0010;
            quickStop = (val & 0x0020) == 0x0020;
            switchOnDisabled = (val & 0x0040) == 0x0040;
            warning = (val & 0x0080) == 0x0080;
            operationModeSpecificBit10 = (val & 0x0400) == 0x0400;
            internalLimitActive = (val & 0x0800) == 0x0800;
            operationModeSpecificBit12 = (val & 0x1000) == 0x1000;
            operationModeSpecificBit13 = (val & 0x2000) == 0x2000;
            configurableBit14 = (val & 0x4000) == 0x4000;
            configurableBit15 = (val & 0x8000) == 0x8000;
        }

        public W6041Statusword(ISerialClient receiver, byte subIndex) : base(receiver, 0x6041, subIndex)
        {
            command[0] = byteSOF;
            command[1] = byteUserDataLength;
            command[2] = byteNodeNumber;
            command[3] = byteCommandCode;
            command[4] = byteIndexLB;
            command[5] = byteIndexHB;
            command[6] = subIndex;
            command[8] = byteEOF;

            byte y8CRC = 0xFF;

            for (int ii = 1; ii < 7; ii++)
            {
                y8CRC = ComputeCRC(command[ii], y8CRC);
            }

            command[7] = y8CRC;
        }

        public bool ReadyToSwitchOn
        {
            get
            {
                return readyToSwitchOn;
            }
        }

        public bool SwitchedOn
        {
            get
            {
                return switchedOn;
            }
        }
        public bool OperationEnabled
        {
            get
            {
                return operationEnabled;
            }
        }

        public bool Fault
        {
            get
            {
                return fault;
            }
        }

        public bool VoltageEnabledNotImplemented
        {
            get
            {
                return voltageEnabledNotImplemented;
            }
        }

        public bool QuickStop
        {
            get
            {
                return quickStop;
            }
        }

        public bool SwitchOnDisabled
        {
            get
            {
                return switchOnDisabled;
            }
        }

        public bool Warning
        {
            get
            {
                return warning;
            }
        }

        public bool OperationModeSpecificBit10
        {
            get
            {
                return operationModeSpecificBit10;
            }
        }

        public bool InternalLimitActive
        {
            get
            {
                return internalLimitActive;
            }
        }

        public bool OperationModeSpecificBit12
        {
            get
            {
                return operationModeSpecificBit12;
            }
        }

        public bool OperationModeSpecificBit13
        {
            get
            {
                return operationModeSpecificBit13;
            }
        }

        public bool ConfigurableBit14
        {
            get
            {
                return configurableBit14;
            }
        }

        public bool ConfigurableBit15
        {
            get
            {
                return configurableBit15;
            }
        }

        public override void Execute()
        {
            Receiver.SendByteBuffer(command);
        }

        public bool NotReadyToSwitchOnState()
        {
            return !ReadyToSwitchOn && !SwitchedOn && !OperationEnabled && !Fault && !SwitchOnDisabled;
        }

        public bool SwitchOnDisabledState()
        {
            return !ReadyToSwitchOn && !SwitchedOn && !OperationEnabled && !Fault && SwitchOnDisabled;
        }

        public bool ReadyToSwitchOnState()
        {
            return ReadyToSwitchOn && !SwitchedOn && !OperationEnabled && !Fault && QuickStop && !SwitchOnDisabled;
        }

        public bool SwitchedOnState()
        {
            return ReadyToSwitchOn && SwitchedOn && !OperationEnabled && !Fault && QuickStop && !SwitchOnDisabled;
        }

        public bool OperationEnabledState()
        {
            return ReadyToSwitchOn && SwitchedOn && OperationEnabled && !Fault && QuickStop && !SwitchOnDisabled;
        }

        public bool QuickStopActiveState()
        {
            return ReadyToSwitchOn && SwitchedOn && OperationEnabled && !Fault && !QuickStop && !SwitchOnDisabled;
        }

        public bool FaultReactionActiveState()
        {
            return ReadyToSwitchOn && SwitchedOn && OperationEnabled && Fault && !SwitchOnDisabled;
        }

        public bool FaultState()
        {
            return !ReadyToSwitchOn && !SwitchedOn && !OperationEnabled && Fault && !SwitchOnDisabled;
        }




    }
}

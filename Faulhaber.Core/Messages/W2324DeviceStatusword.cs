using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faulhaber.Core.Common;

namespace Faulhaber.Core.Messages
{
    public class W2324_01_DeviceStatusword : Message<int>
    {
        byte byteSOF = (byte)'S';
        byte byteUserDataLength = 7;
        byte byteNodeNumber = 0;
        byte byteCommandCode = 0x01;
        byte byteIndexLB = 0x24;
        byte byteIndexHB = 0x23;
        byte byteEOF = (byte)'E';

        byte[] message = new byte[9];

        public W2324_01_DeviceStatusword(int val) : base(0x2324, 0x01, val)
        {
            
        }
        
        public W2324_01_DeviceStatusword(ISerialClient receiver) : base(receiver, 0x2324, 0x01)
        {
            message[0] = byteSOF;
            message[1] = byteUserDataLength;
            message[2] = byteNodeNumber;
            message[3] = byteCommandCode;
            message[4] = byteIndexLB;
            message[5] = byteIndexHB;
            message[6] = 0x01;
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

    public static class W2324_DeviceStatusMask
    {
        //Drive functions 7.1.1 Tab 70
        public const int neq0Monitoring = 0x00000001;
        public const int tragetSpeedReached = 0x00000002;
        public const int speedDeviationLiesOutsideTheCorridor = 0x00000004;
        public const int targetPositionReached = 0x00000008;
        public const int positionHasCrossedTheSetPoint = 0x00000010;
        public const int followingErrorOutsideTheCorridor = 0x00000020;
        public const int positiveLimitSwitchActive = 0x00000040;
        public const int negativeLimitSwitchActive = 0x00000080;
        public const int positiveSoftwarePositionLimitReached = 0x00000100;
        public const int negativeSoftwarePositionLimitReached = 0x00000200;
        public const int referenceInputDetected = 0x00000400;
        public const int encoderIndexDetected = 0x00000800;
        public const int theDriveIsReferenced = 0x00001000;
        public const int voltageLimited = 0x00002000;
        public const int torqueLimited = 0x00004000;
        public const int speedLimited = 0x00008000;
        public const int temperatureWarningLimitReached = 0x00010000;
        public const int temperatureSwitchOffLimitReached = 0x00020000;
        public const int powerSupplyTooHigh = 0x00040000;
        public const int electrnoicsPowerSupplyTooLow = 0x00080000;
        public const int motorSupplyTooLow = 0x00100000;
        public const int speedError = 0x00200000;
        public const int safetyMonitor = 0x00400000;
        public const int digIn01 = 01000000;
        public const int digIn02 = 02000000;
        public const int digIn03 = 04000000;
        public const int digIn04 = 08000000;
        public const int digIn05 = 10000000;
        public const int digIn06 = 20000000;
        public const int digIn07 = 40000000;
        public const int digIn08 = 80000000;

        public static int Neq0Monitoring
        {
            get
            {
                return neq0Monitoring;
            }
        }

        public static int TragetSpeedReached
        {
            get
            {
                return tragetSpeedReached;
            }
        }

        public static int SpeedDeviationLiesOutsideTheCorridor
        {
            get
            {
                return speedDeviationLiesOutsideTheCorridor;
            }
        }

        public static int TargetPositionReached
        {
            get
            {
                return targetPositionReached;
            }
        }
        public static int PositionHasCrossedTheSetPoint
        {
            get
            {
                return positionHasCrossedTheSetPoint;
            }
        }

        public static int FollowingErrorOutsideTheCorridor
        {
            get
            {
                return followingErrorOutsideTheCorridor;
            }
        }

        public static int PositiveLimitSwitchActive
        {
            get
            {
                return positiveLimitSwitchActive;
            }
        }

        public static int NegativeLimitSwitchActive
        {
            get
            {
                return negativeLimitSwitchActive;
            }
        }

        public static int PositiveSoftwarePositionLimitReached
        {
            get
            {
                return positiveSoftwarePositionLimitReached;
            }
        }

        public static int NegativeSoftwarePositionLimitReached
        {
            get
            {
                return negativeSoftwarePositionLimitReached;
            }
        }

        public static int ReferenceInputDetected
        {
            get
            {
                return referenceInputDetected;
            }
        }

        public static int EncoderIndexDetected
        {
            get
            {
                return encoderIndexDetected;
            }
        }

        public static int TheDriveIsReferenced
        {
            get
            {
                return theDriveIsReferenced;
            }
        }

        public static int VoltageLimited
        {
            get 
            {
                return voltageLimited;
            }
        }

        public static int TorqueLimited
        {
            get
            {
                return torqueLimited;
            }
        }
        public static int SpeedLimited
        {
            get
            {
                return speedLimited;
            }
        }

        public static int TemperatureWarningLimitReached
        {
            get
            {
                return temperatureWarningLimitReached;
            }
        }

        public static int TemperatureSwitchOffLimitReached
        {
            get
            {
                return temperatureSwitchOffLimitReached;
            }
        }

        public static int PowerSupplyTooHigh
        {
            get
            {
                return powerSupplyTooHigh;
            }
        }

        public static int ElectrnoicsPowerSupplyTooLow
        {
            get
            {
                return electrnoicsPowerSupplyTooLow;
            }
        }

        public static int MotorSupplyTooLow
        {
            get
            {
                return motorSupplyTooLow;
            }
        }
        public static int SpeedError
        {
            get
            {
                return speedError;
            }
        }

        public static int SafetyMonitor
        {
            get
            {
                return safetyMonitor;
            }
        }

        public static int DigIn01
        {
            get
            {
                return digIn01;
            }
        }

        public static int DigIn02
        {
            get
            {
                return digIn02;
            }
        }

        public static int DigIn03
        {
            get
            {
                return digIn03;
            }
        }

        public static int DigIn04
        {
            get
            {
                return digIn04;
            }
        }

        public static int DigIn05
        {
            get
            {
                return digIn05;
            }
        }

        public static int DigIn06
        {
            get
            {
                return digIn05;
            }
        }

        public static int DigIn07
        {
            get
            {
                return digIn07;
            }
        }

        public static int DigIn08
        {
            get
            {
                return digIn08;
            }
        }

    }
}

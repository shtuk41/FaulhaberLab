using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Faulhaber.Core;
using Faulhaber.Core.Common;
using Faulhaber.Core.Messages;

namespace FaulhaberPlayer
{
    public class FaulhaberPlayerOperator : INotifyPropertyChanged, IDisposable
    {
        #region fields

        private bool disposed = false;

        private SerialPortOperator spOperator;
        private string comPort = string.Empty;
        private string bufferRx = string.Empty;
        private ProfilePositionPage ppPage = new ProfilePositionPage();
        private OperationStates state = OperationStates.Neutral;
        private bool statusReadyToSwitchOn = false;
        private bool statusSwitchedOn = false;
        private bool statusOperationEnabled = false;
        private bool statusFault = false;
        private bool statusQuickStop = false;
        private bool statusSwitchOnDisabled = false;
        private bool statusWarning = false;
        private bool statusBit10 = false;
        private bool statusBit12 = false;
        private bool statusBit13 = false;
        private sbyte homingMethod;
        private uint  motorShaftRevolutions = 26;
        private uint drivingShaftRevolutions = 7;
        private uint feed = 360000;
        private uint shaftRevolutions = 1;
        private uint velocityFactorNumerator = 1;
        private uint velocityFactorDivisor = 256;
        private uint maximumMotorSpeed = 100000;
        private uint profileAcceleration = 200;
        private uint profileDeceleration = 200;
        private bool reversePolarityPosition;
        private bool reversePolaritySpeed;
        private int homingOffset = 6539;
        private byte digitalInputLogicalState = 0;
        private byte digitalInputPhysicalState = 0;
        private byte digitalOutputStatus = 0;

        #endregion

        public FaulhaberPlayerOperator()
        {
            spOperator = new SerialPortOperator(OperatorResponse);

        }

        #region events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region properties


        public byte DigitalInputLogicalState
        {
            get
            {
                return digitalInputLogicalState;
            }

            set
            {
                if (value != DigitalInputLogicalState)
                {
                    digitalInputLogicalState = value;
                    OnPropertyChanged("DigitalInputLogicalState");
                }
            }
        }

        public byte DigitalInputPhysicalState
        {
            get
            {
                return digitalInputPhysicalState;
            }

            set
            {
                if (value != digitalInputPhysicalState)
                {
                    digitalInputPhysicalState = value;
                    OnPropertyChanged("DigitalInputPhysicalState");
                }
            }
        }

        public byte DigitalOutputStatus
        {
            get
            {
                return digitalOutputStatus;
            }

            set
            {
                if (value != digitalOutputStatus)
                {
                    digitalOutputStatus = value;
                    OnPropertyChanged("DigitalOutputStatus");
                }
            }
        }

        public int HomingOffset
        {
            get
            {
                return homingOffset;
            }

            set
            {
                if (value != homingOffset)
                {
                    homingOffset = value;
                    OnPropertyChanged("HomingOffset");
                }
            }
        }

        public bool ReversePolarityPosition
        {
            get
            {
                return reversePolarityPosition;
            }

            set
            {
                if (value != reversePolarityPosition)
                {
                    reversePolarityPosition = value;
                    OnPropertyChanged("ReversePolarityPosition");
                }
            }
        }

        public bool ReversePolaritySpeed
        {
            get
            {
                return reversePolaritySpeed;
            }

            set
            {
                if (value != reversePolaritySpeed)
                {
                    reversePolaritySpeed = value;
                    OnPropertyChanged("ReversePolaritySpeed");
                }
            }
        }

        public uint MaximumMotorSpeed
        {
            get
            {
                return maximumMotorSpeed;
            }

            set
            {
                if (value != maximumMotorSpeed)
                {
                    maximumMotorSpeed = value;
                    OnPropertyChanged("MaximumMotorSpeed");
                }
            }
        }

        public uint ProfileAcceleration
        {
            get
            {
                return profileAcceleration;
            }

            set
            {
                if (value != ProfileAcceleration)
                {
                    profileAcceleration = value;
                    OnPropertyChanged("ProfileAcceleration");
                }
            }
        }

        public uint ProfileDeceleration
        {
            get
            {
                return profileDeceleration;
            }

            set
            {
                if (value != profileDeceleration)
                {
                    profileDeceleration = value;
                    OnPropertyChanged("ProfileDeceleration");
                }
            }
        }



        public uint VelocityFactorNumerator
        {
            get
            {
                return velocityFactorNumerator;
            }

            set
            {
                if (value != velocityFactorNumerator)
                {
                    velocityFactorNumerator = value;
                    OnPropertyChanged("VelocityFacotrNumerator");
                }
            }
        }

        public uint VelocityFactorDivisor
        {
            get
            {
                return velocityFactorDivisor;
            }

            set
            {
                if (value != velocityFactorDivisor)
                {
                    velocityFactorDivisor = value;
                    OnPropertyChanged("VelocityFactorDivisor");
                }
            }
        }

        public uint Feed
        {
            get
            {
                return feed;
            }

            set
            {
                if (value != feed)
                {
                    feed = value;
                    OnPropertyChanged("Feed");
                }
            }
        }

        public uint ShaftRevolutions
        {
            get
            {
                return shaftRevolutions;
            }

            set
            {
                if (value != shaftRevolutions)
                {
                    shaftRevolutions = value;
                    OnPropertyChanged("ShaftRevolutions");
                }
            }
        }

        public uint MotorShaftRevolutions
        {
            get
            {
                return motorShaftRevolutions;
            }

            set
            {
                if (value != motorShaftRevolutions)
                {
                    motorShaftRevolutions = value;
                    OnPropertyChanged("MotorShaftRevolutions");
                }
            }
        }

        public uint DrivingShaftRevolutions
        {
            get
            {
                return drivingShaftRevolutions;
            }

            set
            {
                if (value != drivingShaftRevolutions)
                {
                    drivingShaftRevolutions = value;
                    OnPropertyChanged("DrivingShaftRevolutions");
                }
            }
        }

        public sbyte HomingMethod
        {
            get
            {
                return homingMethod;
            }

            set
            {
                if (value != homingMethod)
                {
                    homingMethod = value;
                    OnPropertyChanged("HomingMethod");
                }
            }
        }

        public bool StatusReadyToSwitchOn
        {
            get
            {
                return statusReadyToSwitchOn;
            }

            private set
            {
                if (value != statusReadyToSwitchOn)
                {
                    statusReadyToSwitchOn = value;
                    OnPropertyChanged("StatusReadyToSwitchOn");
                }
            }
        }

        public bool StatusSwitchedOn
        {
            get
            {
                return statusSwitchedOn;
            }

            private set
            {
                if (value != statusSwitchedOn)
                {
                    statusSwitchedOn = value;
                    OnPropertyChanged("StatusSwitchedOn");
                }
            }
        }

        public bool StatusOperationEnabled
        {
            get
            {
                return statusOperationEnabled;
            }

            private set
            {
                if (value != statusOperationEnabled)
                {
                    statusOperationEnabled = value;
                    OnPropertyChanged("StatusOperationEnabled");
                }
            }
        }

        public bool StatusFault
        {
            get
            {
                return statusFault;
            }

            private set
            {
                if (value != statusFault)
                {
                    statusFault = value;
                    OnPropertyChanged("StatusFault");
                }
            }
        }

        public bool StatusQuickStop
        {
            get
            {
                return statusQuickStop;
            }

            private set
            {
                if (value != statusQuickStop)
                {
                    statusQuickStop = value;
                    OnPropertyChanged("StatusQuickStop");
                }
            }
        }

        public bool StatusSwitchOnDisabled
        {
            get
            {
                return statusSwitchOnDisabled;
            }

            private set
            {
                if (value != statusSwitchOnDisabled)
                {
                    statusSwitchOnDisabled = value;
                    OnPropertyChanged("StatusSwitchOnDisabled");
                }
            }
        }

        public bool StatusWarning
        {
            get
            {
                return statusWarning;
            }

            private set
            {
                if (value != statusWarning)
                {
                    statusWarning = value;
                    OnPropertyChanged("StatusWarning");
                }
            }
        }

        public bool StatusBit10
        {
            get
            {
                return statusBit10;
            }

            private set
            {
                //if (value != statusBit10)
                //{
                    statusBit10 = value;
                    OnPropertyChanged("StatusBit10");
                //}
            }
        }
        public bool StatusBit12
        {
            get
            {
                return statusBit12;
            }

            private set
            {
                //if (value != statusBit12)
                //{
                    statusBit12 = value;
                    OnPropertyChanged("StatusBit12");
                //}
            }
        }
        public bool StatusBit13
        {
            get
            {
                return statusBit13;
            }

            private set
            {
                //if (value != statusBit13)
                //{
                    statusBit13 = value;
                    OnPropertyChanged("StatusBit13");
                //}
            }
        }

        public OperationStates State
        {
            get
            {
                return state;
            }

            private set
            {
                state = value;
            }
        }

        /// <summary>
        /// Sets the COM port.
        /// </summary>
        /// <value>
        /// The COM port.
        /// </value>
        public string ComPort
        {
            get
            {
                return comPort;
            }

            set
            {
                if (value != comPort)
                {
                    comPort = value;
                }
            }
        }

        public string BufferRx
        {
            get
            {
                return bufferRx;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    bufferRx = string.Empty;
                }
                else
                {
                    if (bufferRx.Length > 10000)
                    {
                        bufferRx = bufferRx.Remove(0, 5000);
                    }

                    bufferRx += value;
                    bufferRx += "\n";
                }

                OnPropertyChanged("BufferRx");
            }
        }

        public ProfilePositionPage PPPage
        {
            get
            {
                return ppPage;
            }
        }

        #endregion

        public void Shutdown()
        {
            W6040Controlword controlWord = new W6040Controlword(spOperator, Controlword6040Commands.ShutdownCommand);
            controlWord.Execute();
        }

        public void FaultReset()
        {
            W6040Controlword controlWord = new W6040Controlword(spOperator, Controlword6040Commands.FaultReset);
            controlWord.Execute();
        }

        public void SwitichOn()
        {
            W6040Controlword controlWord = new W6040Controlword(spOperator, Controlword6040Commands.SwitchOnCommand);
            controlWord.Execute();
        }

        public void EnableOp()
        {
            W6040Controlword controlWord = new W6040Controlword(spOperator, Controlword6040Commands.EnableOperationCommand);
            controlWord.Execute();
        }

        public void ReadModeOfOperationDisplay6061()
        {
            W6061ModesOfOperationDisplay word6061 = new W6061ModesOfOperationDisplay(spOperator);
            word6061.Execute();
        }

        public void ReadPositionActualValue6064()
        {
            W6064PositionActualValue word6064 = new W6064PositionActualValue(spOperator);
            word6064.Execute();
        }

        public void TryConnect()
        {
            spOperator.TryConnect(ComPort);
        }

        public void OperatorResponse(string source, byte [] buffer)
        {
            BufferRx = source;

            if (source == "Serial")
            {
                IMessage msg = MessageDecoder.Decode(buffer);

                var statusWord = msg as W6041Statusword;
                var controlWord = msg as W6040Controlword;
                var positionActualvalue = msg as W6064PositionActualValue;
                var velocityActualValue = msg as W606CVelocityActualValue;
                var torqueActualValue = msg as W6077TorqueActualValue;
                var deviceStatus = msg as W2324_01_DeviceStatusword;
                var gearRatio = msg as W6091GearRatio;
                var feedConstant = msg as W6092FeedConstant;
                var velocityFactor = msg as W6096VelocityFactor;
                var maxiumSpeed = msg as W6080MaximumSpeed;
                var profileAcceleration = msg as W6083Acceleration;
                var profileDeceleration = msg as W6084Deceleration;
                var polarity = msg as W607EPolarity;
                var digitalIoStatus = msg as W2311DigitalIOStatus;

                if (statusWord != null)
                {
                    BufferRx = "Status updated\n";

                    /*BufferRx = string.Format("ReadyToSwitchOn: {0}", statusWord.ReadyToSwitchOn ? "1" : "0");
                    BufferRx = string.Format("SwitchedOn: {0}", statusWord.SwitchedOn ? "1" : "0");
                    BufferRx = string.Format("OperationEnabled: {0}", statusWord.OperationEnabled ? "1" : "0");
                    BufferRx = string.Format("Fault: {0}", statusWord.Fault ? "1" : "0");
                    BufferRx = string.Format("QuickStop: {0}", statusWord.QuickStop ? "1" : "0");
                    BufferRx = string.Format("SwitchOnDisabled: {0}", statusWord.SwitchOnDisabled ? "1" : "0");
                    BufferRx = string.Format("Warning: {0}", statusWord.Warning ? "1" : "0");
                    BufferRx = string.Format("Bit10: {0}", statusWord.OperationModeSpecificBit10 ? "1" : "0");
                    BufferRx = "\n";
                    */

                    StatusReadyToSwitchOn = statusWord.ReadyToSwitchOn;
                    StatusSwitchedOn = statusWord.SwitchedOn;
                    StatusOperationEnabled = statusWord.OperationEnabled;
                    StatusFault = statusWord.Fault;
                    StatusQuickStop = statusWord.QuickStop;
                    StatusSwitchOnDisabled = statusWord.SwitchOnDisabled;
                    StatusWarning = statusWord.Warning;
                    StatusBit10 = statusWord.OperationModeSpecificBit10;
                    StatusBit12 = statusWord.OperationModeSpecificBit12;
                    StatusBit13 = statusWord.OperationModeSpecificBit13;

                    if (State == OperationStates.MotionRequested && !statusWord.OperationModeSpecificBit10)
                    {
                        State = OperationStates.MotionStarted;
                    }
                    else if (State == OperationStates.MotionStarted && statusWord.OperationModeSpecificBit10)
                    {
                        State = OperationStates.TargetReached;
                    }
                    else if (State == OperationStates.HomingRequested && statusWord.OperationModeSpecificBit12)
                    {
                        BufferRx = "Homing set";
                        State = OperationStates.Neutral;
                    }
                }
                else if (deviceStatus != null)
                {
                    int deviceStatusValue = (int)deviceStatus.Value();

                    Type type = typeof(W2324_DeviceStatusMask);

                    PropertyInfo[] properties = type.GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        int val = (int)property.GetValue(null, null);
                        if ((deviceStatusValue & val) > 0)
                        {
                            BufferRx = property.Name;
                        }
                    }
                }
                else if (digitalIoStatus != null)
                {

                    switch (digitalIoStatus.SubIndex())
                    {
                        case W2311DigialIOStatusSubIndexes.DigitalInputLogicalState:
                            DigitalInputLogicalState = (byte)digitalIoStatus.Value();
                            BufferRx = "Digital I/O logical: " + DigitalInputLogicalState;
                            break;
                        case W2311DigialIOStatusSubIndexes.DigitalInputPhysicalState:
                            DigitalInputPhysicalState = (byte)digitalIoStatus.Value();
                            BufferRx = "Digital I/O physical: " + DigitalInputPhysicalState;
                            break;
                        case W2311DigialIOStatusSubIndexes.DigitalOutputStatus:
                            DigitalOutputStatus = (byte)digitalIoStatus.Value();
                            BufferRx = "Digital I/O output: " + DigitalOutputStatus;
                            break;
                    }

                    BufferRx = digitalIoStatus.Value().ToString();

                    if (State == OperationStates.DigitalIOStatusRequested)
                    {
                        State = OperationStates.DigitalIOStatusProvided;
                    }
                }
                else if (positionActualvalue != null)
                {
                    PPPage.ActualValue = (int)positionActualvalue.Value();
                    BufferRx = positionActualvalue.Value().ToString();
                }
                else if (controlWord != null)
                {

                }
                else if (velocityActualValue != null)
                {
                    BufferRx = "Velocity: " + msg.Value().ToString();
                }
                else if (torqueActualValue != null)
                {
                    BufferRx = "Torque: " + msg.Value().ToString();
                }
                else if (profileAcceleration != null)
                {
                    string profileAccelerationValue = string.Empty;

                    if (profileAcceleration.SubIndex() == 0x00)
                    {
                        profileAccelerationValue = "Profile Acceleration: Set";
                    }
                    else
                    {
                        profileAccelerationValue = "Profile Acceleration: unknown subIndex";
                    }

                    BufferRx = profileAccelerationValue;
                }
                else if (profileDeceleration != null)
                {
                    string profileDeclerationValue = string.Empty;

                    if (profileDeceleration.SubIndex() == 0x00)
                    {
                        profileDeclerationValue = "Profile Deceleration Set";
                    }
                    else
                    {
                        profileDeclerationValue = "Profile Deceleration: unknown subIndex";
                    }

                    BufferRx = profileDeclerationValue;
                }
                else if (maxiumSpeed != null)
                {
                    string maxiumSpeedValue = string.Empty;

                    if (maxiumSpeed.SubIndex() == 0x00)
                    {
                        maxiumSpeedValue = "Maximum: Motor: Set";
                    }
                    else
                    {
                        maxiumSpeedValue = "Maximum: Motor: unknown subIndex";
                    }

                    BufferRx = maxiumSpeedValue;
                }
                else if (gearRatio != null)
                {
                    string gearRatioValue = string.Empty;

                    if (gearRatio.SubIndex() == 0x01)
                    {
                        gearRatioValue = "Gear Ratio: Motor Shaft Revolutions Set";
                    }
                    else if (gearRatio.SubIndex() == 0x02)
                    {
                        gearRatioValue = "Gear Ratio: Driving Shaft Revolutions Set";
                    }
                    else
                    {
                        gearRatioValue = "Gear Ratio: unknown subIndex";
                    }

                    BufferRx = gearRatioValue;
                }
                else if (feedConstant != null)
                {
                    string feedConstantValue = string.Empty;

                    if (feedConstant.SubIndex() == 0x01)
                    {
                        feedConstantValue = "Feed Constant: Feed set";
                    }
                    else if (feedConstant.SubIndex() == 0x02)
                    {
                        feedConstantValue = "Feed Constant: Shaft Revolutions set";
                    }
                    else
                    {
                        feedConstantValue = "Feed Constant: unknown index";
                    }

                    BufferRx = feedConstantValue;
                }
                else if (velocityFactor != null)
                {
                    string velocityFactorValue = string.Empty;

                    if (velocityFactor.SubIndex() == 0x01)
                    {
                        velocityFactorValue = "Velocity Factor: Numerator set";
                    }
                    else if (velocityFactor.SubIndex() == 0x02)
                    {
                        velocityFactorValue = "Velocity Factor: Divisor set";
                    }
                    else
                    {
                        velocityFactorValue = "Velocity Facotr: unknown index";
                    }

                    BufferRx = velocityFactorValue;
                }
                if (polarity != null)
                {
                    BufferRx = "Polarity Set";
                }
                else if (msg != null)
                {
                    BufferRx = msg.Value().ToString();
                }
            }
        }

        public void ReadIdentity1018()
        {
            W1018Identity vendorId = new W1018Identity(spOperator, Identity1018Subindex.VendorID);
            vendorId.Execute();
        }

        public void ReadDeviceStatus2324()
        {
            W2324_01_DeviceStatusword deviceStatus = new W2324_01_DeviceStatusword(spOperator);
            deviceStatus.Execute();
        }

        public void ReadStatusword6041()
        {
            W6041Statusword statusword = new W6041Statusword(spOperator, 0x00);
            statusword.Execute();
        }

        public void SetOperationModeProfilePosition()
        {
            W6060OperationMode operationMode = new W6060OperationMode(spOperator, OperationMode6060VaLues.PP);
            operationMode.Execute();
        }

        public void SetOperationModeHoming()
        {
            W6060OperationMode operationMode = new W6060OperationMode(spOperator, OperationMode6060VaLues.Homing);
            operationMode.Execute();
        }

        public void SetHomngMethod()
        {
            W6098HomingMethod homingMethod = new W6098HomingMethod(spOperator, HomingMethod);
            homingMethod.Execute();
        }

        public async void StartHoming()
        {
            await RequestDigitalIOStatus();

            if (DigitalInputLogicalState == 0x01)
            {
                W6060OperationMode opModePP = new W6060OperationMode(spOperator, OperationMode6060VaLues.PP);
                opModePP.Execute();

                int targetPositionSetPoint = 6000;

                await PerformAbsoluteMotion(false, targetPositionSetPoint);
            }

            State = OperationStates.HomingRequested;

            W6040Controlword controlWord = new W6040Controlword(spOperator, 0x000F);
            controlWord.Execute();

            W6060OperationMode operationMode = new W6060OperationMode(spOperator, OperationMode6060VaLues.Homing);
            operationMode.Execute();

            //2310.04
            W2310Inputs inputsWord = new W2310Inputs(spOperator, 0x04, 1);
            inputsWord.Execute();

            W6099HomingSpeed homingSpeedSwitchSeekSpeed = new W6099HomingSpeed(spOperator, 0x01, 800);
            homingSpeedSwitchSeekSpeed.Execute();

            W6099HomingSpeed homingSpeed  = new W6099HomingSpeed(spOperator, 0x02, 800);
            homingSpeed.Execute();

            W609AHomingAcceleration homingAcceleration = new W609AHomingAcceleration(spOperator, 25);
            homingAcceleration.Execute();

            controlWord = new W6040Controlword(spOperator, 0x001F);
            controlWord.Execute();
        }

        public void ControlWordResetBit4()
        {

        }

        public void SetOperationModeProfileVelocity()
        {
            W6060OperationMode operationMode = new W6060OperationMode(spOperator, OperationMode6060VaLues.PV);
            operationMode.Execute();
        }

        public void ReadPositionDemandValue6062()
        {
            W6062PositionDemandValue word6062 = new W6062PositionDemandValue(spOperator);
            word6062.Execute();
        }

        public void ReadTorqueActualValue6077()
        {
            W6077TorqueActualValue word6077 = new W6077TorqueActualValue(spOperator);
            word6077.Execute();
        }

        public void ReadVelocityActualValue606C()
        {
            W606CVelocityActualValue word606C = new W606CVelocityActualValue(spOperator);
            word606C.Execute();
        }

        public void SetGearRatio6091()
        {
            W6091GearRatio word6091 = new W6091GearRatio(spOperator, motorShaftRevolutions, W6091GearRatioSubIndexes.MotorShaftRevolutions);
            word6091.Execute();

            word6091 = new W6091GearRatio(spOperator, drivingShaftRevolutions, W6091GearRatioSubIndexes.DrivingShaftRevolutions);
            word6091.Execute();
        }

        public void SetFeedConstant6092()
        {
            W6092FeedConstant word6092 = new W6092FeedConstant(spOperator, feed, W6092FeedConstantSubIndexes.Feed);
            word6092.Execute();

            word6092 = new W6092FeedConstant(spOperator, shaftRevolutions, W6092FeedConstantSubIndexes.ShaftRevolutions);
            word6092.Execute();
        }

        public void SetVelocityFactor6096()
        {
            W6096VelocityFactor word6096 = new W6096VelocityFactor(spOperator, velocityFactorNumerator, W6096VelocityFactorSubIndexes.Numerator);
            word6096.Execute();

            word6096 = new W6096VelocityFactor(spOperator, velocityFactorDivisor, W6096VelocityFactorSubIndexes.Divisor);
            word6096.Execute();
        }

        public void SetMaxSpeedAccelerationDeceleration()
        {
            W6080MaximumSpeed maxSpeed = new W6080MaximumSpeed(spOperator, MaximumMotorSpeed);
            maxSpeed.Execute();
            W6083Acceleration acceleration = new W6083Acceleration(spOperator, ProfileAcceleration);
            acceleration.Execute();
            W6084Deceleration deceleration = new W6084Deceleration(spOperator, ProfileDeceleration);
            deceleration.Execute();
        }

        public void ReversePolarity607E()
        {
            byte polarity = 0x00;

            if (ReversePolarityPosition)
                polarity |= W607EPolarityValues.InvertPosition;
            if (ReversePolaritySpeed)
                polarity |= W607EPolarityValues.InvertSpeed;

            W607EPolarity word607E = new W607EPolarity(spOperator, polarity);
            word607E.Execute();
        }

        public void SetHomingOffset607C()
        {
            W607CHomingOffset offset = new W607CHomingOffset(spOperator, HomingOffset);
            offset.Execute();
        }

        public async void  PPSP1Go(bool absolutePos)
        {
            await PerformAbsoluteMotion(absolutePos, ppPage.SetPoint1);
        }

        public async void PPSP2Go(bool absolutePos)
        {
            await PerformAbsoluteMotion(absolutePos, ppPage.SetPoint2);
        }

        public async Task RequestDigitalIOStatus()
        {
            DigitalInputLogicalState = 0;
            DigitalInputPhysicalState = 0;
            DigitalOutputStatus = 0;
    
            W2311DigitalIOStatus digitalIostatus = new W2311DigitalIOStatus(spOperator, W2311DigialIOStatusSubIndexes.DigitalInputLogicalState);

            digitalIostatus.Execute();

            State = OperationStates.DigitalIOStatusRequested;

            while (State != OperationStates.DigitalIOStatusProvided)
            {
                await Task.Delay(500);
            }
        }

        public async Task PerformAbsoluteMotion(bool absPos, int setPoint)
        {
            W6040Controlword controlWord2 = new W6040Controlword(spOperator, 0x0F);
            controlWord2.Execute();
            W607ATargetPosition tposition = new W607ATargetPosition(spOperator, setPoint);
            tposition.Execute();
            /*W6080MaximumSpeed maxSpeed = new W6080MaximumSpeed(spOperator, 100000);
            maxSpeed.Execute();
            W6083Acceleration acceleration = new W6083Acceleration(spOperator, 200);
            acceleration.Execute();
            W6084Deceleration deceleration = new W6084Deceleration(spOperator, 200);
            deceleration.Execute();*/
            W6040Controlword controlWord = new W6040Controlword(spOperator, (absPos ? (ushort)0x1F : (ushort)0x5F));
            controlWord.Execute();

            State = OperationStates.MotionRequested;

            W6064PositionActualValue position = new W6064PositionActualValue(spOperator);

            while (State == OperationStates.MotionRequested || State != OperationStates.TargetReached)
            {
                await Task.Delay(500);

                position.Execute();

                if (State == OperationStates.TargetReached)
                    break;

               
            }

            State = OperationStates.Neutral;

            position.Execute();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                spOperator.Dispose();
            }

            disposed = true;
        }

        #region PropertyChanged

        /// <summary>
        /// Called when a property is changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}

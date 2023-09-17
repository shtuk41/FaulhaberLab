using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;

using Faulhaber.Core;
using Faulhaber.Core.Common;
using Faulhaber.Core.Messages;

namespace FaulhaberMonitorService
{
    public class Monitor
    {
        #region fields

        private SerialPortOperator spOperator;
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
        private OperationStates state = OperationStates.Neutral;
        private byte digitalInputLogicalState = 0;
        private byte digitalInputPhysicalState = 0;
        private byte digitalOutputStatus = 0;
        private System.Timers.Timer controllerStatusTimer;
        private int absolutePosition = 0;
        private int absolutePositionTarget = 20162;
        private int absolutePositionTargetTolerance = 2016;



        #endregion

        #region constructor
        public Monitor()
        {
            Logger.ConfigureLogger();
        }

        #endregion

        #region properties

        public string BufferRx
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Logger.PrintLog(value);
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
                statusBit10 = value;
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
                statusBit12 = value;
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
                statusBit13 = value;
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
                }
            }
        }

        #endregion

        #region methods

        public void Run()
        {
            Logger.PrintLog("Starting Service...");

            int updatedPeriodSec = Options.UpdatePeriodSec;
            string port = Options.SerialPort;
            int position = Options.TargetPosition;
            int tolerance = Options.TargetPositionTolerance;
            string logDirectory = Options.LogDirectory;
            string address = Options.Address;
            string url = Options.Url;

            Logger.PrintLog(string.Format("updatedPeriodSec {0}", updatedPeriodSec));
            Logger.PrintLog(string.Format("port {0}", port));
            Logger.PrintLog(string.Format("position {0}", position));
            Logger.PrintLog(string.Format("tolerance {0}", tolerance));
            Logger.PrintLog(string.Format("logDirectory {0}", logDirectory));
            Logger.PrintLog(string.Format("address: {0}", address));
            Logger.PrintLog(string.Format("url: {0}", url));

            absolutePositionTarget = position;
            absolutePositionTargetTolerance = tolerance;


            spOperator = new SerialPortOperator(OperatorResponse);

            //connect to controller

            spOperator.TryConnect(port);

            Thread.Sleep(2000);

            if (!spOperator.IsActive())
            {
                Logger.PrintLog("operator is not active");
                spOperator.Dispose();
                return;
            }

            //setup timer to call controller status update
            controllerStatusTimer = new System.Timers.Timer();
            controllerStatusTimer.Interval = updatedPeriodSec * 1000;
            controllerStatusTimer.Elapsed += OnControllerStatusTimerEvent;
            controllerStatusTimer.AutoReset = false;
            controllerStatusTimer.Enabled = true;

            Console.WriteLine("Enter S to stop...");

            ConsoleKeyInfo input;

            do
            {
                input = Console.ReadKey();
            } while (input.Key != ConsoleKey.S);

            if (spOperator.IsActive())
            {
                Logger.PrintLog("Diposeding operator...");
                spOperator.Dispose();
                return;
            }
        }

        #endregion

        #region functions

        private void OperatorResponse(string source, byte[] buffer)
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
                    BufferRx = positionActualvalue.Value().ToString();

                    if (State == OperationStates.AbsolutePositionRequested)
                    {
                        State = OperationStates.AbsolutePositionProvided;
                    }
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

        private async void OnControllerStatusTimerEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            controllerStatusTimer.Stop();

            if (State == OperationStates.Neutral)
            {

                State = OperationStates.AbsolutePositionRequested;

                W6064PositionActualValue position = new W6064PositionActualValue(spOperator);

                while (State == OperationStates.AbsolutePositionRequested || State != OperationStates.AbsolutePositionProvided)
                {
                    await Task.Delay(500);
                }

                State = OperationStates.Neutral;

                UpdateASIS();
            }

            controllerStatusTimer.Start();
        }

        private void UpdateASIS()
        {
            bool positionAlarm = (absolutePosition > (absolutePositionTarget + absolutePositionTargetTolerance)) ||
                                 (absolutePosition < (absolutePositionTarget - absolutePositionTargetTolerance));

            HttpHandler httpClient = new HttpHandler();

            httpClient.UpdateStatus(absolutePosition, positionAlarm);
        }

        #endregion
    }
}

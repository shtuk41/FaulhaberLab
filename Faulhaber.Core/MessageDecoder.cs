using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Faulhaber.Core.Common;
using Faulhaber.Core.Messages;

namespace Faulhaber.Core
{
    public static class MessageDecoder
    {
        static MessageDecoder() { }

        public static IMessage Decode(byte [] msg)
        {
            IMessage retMessage = null;

            if (msg[0] == (byte)'S' && msg[msg.Count() - 1] == (byte)'E')
            {
                int length = (int)msg[1];

                /*if (msg[3] == 0x01)
                {
                    BufferRx = "Read";
                }
                else if (msg[3] == 0x02)
                {
                    BufferRx = "Write";
                }*/

                if (msg[3] == 0x04)
                {
                    retMessage = new W6040Controlword(4);
                }
                else if (msg[3] == 0x05)
                {
                    ushort val = (ushort)(msg[4] | msg[5] << 8);
                    retMessage = new W6041Statusword(val);
                }
                else if (msg[3] == 0x07)
                {
                    retMessage = new W6041Statusword(7);
                }
                else
                {
                    string index = string.Format("{0:X2}{1:X2}", msg[5], msg[4]);
                    byte subindex = msg[6];         

                    if (index == "1018")
                    {
                        int size = (length - 7);
                        int valueHolder = 0;

                        for (int ii = 0; ii < size; ii++)
                        {
                            valueHolder |= (msg[7 + ii]) << (ii * 8);
                        }

                        retMessage = new W1018Identity(msg[6], valueHolder);
                    }
                    else if (index == "6091")
                    {
                        if (msg[3] == 0x01) //read
                        {
                            uint val = (uint)(msg[7] | msg[8] << 8 | msg[9] << 16 | msg[10] << 24);
                            retMessage = new W6091GearRatio(val, subindex);
                        }
                        else
                        {
                            retMessage = new W6091GearRatio(0, subindex);
                        }
                    }
                    else if (index == "6092")
                    {
                        if (msg[3] == 0x01) //read
                        {
                            uint val = (uint)(msg[7] | msg[8] << 8 | msg[9] << 16 | msg[10] << 24);
                            retMessage = new W6091GearRatio(val, subindex);
                        }
                        else
                        {
                            retMessage = new W6092FeedConstant(0, subindex);
                        }
                    }
                    else if (index == "6096")
                    {
                        if (msg[3] == 0x01) //read
                        {
                            uint val = (uint)(msg[7] | msg[8] << 8 | msg[9] << 16 | msg[10] << 24);
                            retMessage = new W6096VelocityFactor(val, subindex);
                        }
                        else
                        {
                            retMessage = new W6096VelocityFactor(0, subindex);
                        }
                    }
                    else if (index == "607E")
                    {
                        if (msg[3] == 0x01)
                        {
                            byte val = msg[6];
                            retMessage = new W607EPolarity(val);
                        }
                        else
                        {
                            retMessage = new W607EPolarity(0);
                        }
                    }
                    else if (index == "2324" && subindex == 0x01)
                    {
                        int val = msg[7] | msg[8] << 8 | msg[9] << 16 | msg[10] << 24;
                        retMessage = new W2324_01_DeviceStatusword(val);
                    }
                    else if (index == "6041")
                    {
                        ushort value = (ushort)(msg[7] | msg[8] << 8);
                        retMessage = new W6041Statusword(value);
                    }
                    else if (index == "6060")
                    {
                        sbyte val = (sbyte)msg[6];
                        retMessage = new W6060OperationMode(val);
                    }
                    else if (index == "6061")
                    {
                        sbyte val = (sbyte)msg[7];
                        retMessage = new W6061ModesOfOperationDisplay(val);
                    }
                    else if (index == "607A")
                    {
                        int val = msg[6];
                        retMessage = new W607ATargetPosition(val);
                    }
                    else if (index == "6083")
                    {
                        if (msg[3] == 0x01) //read
                        {
                            uint val = (uint)(msg[7] | msg[8] << 8 | msg[9] << 16 | msg[10] << 24);
                            retMessage = new W6083Acceleration(val);
                        }
                        else
                        {
                            retMessage = new W6083Acceleration(0);
                        }
                    }
                    else if (index == "6084")
                    {
                        if (msg[3] == 0x01) //read
                        {
                            uint val = (uint)(msg[7] | msg[8] << 8 | msg[9] << 16 | msg[10] << 24);
                            retMessage = new W6084Deceleration(val);
                        }
                        else
                        {
                            retMessage = new W6084Deceleration(0);
                        }
                    }
                    else if (index == "6080")
                    {
                        if (msg[3] == 0x01) //read
                        {
                            uint val = (uint)(msg[7] | msg[8] << 8 | msg[9] << 16 | msg[10] << 24);
                            retMessage = new W6080MaximumSpeed(val);
                        }
                        else
                        {
                            retMessage = new W6080MaximumSpeed(0);
                        }
                    }
                    else if (index == "6064")
                    {
                        if (msg.Length > 9)
                        {
                            int val = (msg[7] | msg[8] << 8 | msg[9] << 16 | msg[10] << 24);
                            retMessage = new W6064PositionActualValue(val);
                        }
                        else
                        {
                            retMessage = new W6064PositionActualValue(-1);
                        }
                    }
                    else if (index == "6062")
                    {
                        int val = msg[7] | msg[8] << 8 | msg[9] << 16 | msg[10] << 24;
                        retMessage = new W6062PositionDemandValue(val);
                    }
                    else if (index == "6077")
                    {
                        short val = (short)(msg[7] | msg[8] << 8);
                        retMessage = new W6077TorqueActualValue(val);
                    }
                    else if (index == "606C")
                    {
                        int val = msg[7] | msg[8] << 8 | msg[9] << 16 | msg[10] << 24;
                        retMessage = new W606CVelocityActualValue(val);
                    }
                    else if (index == "2311")
                    {
                        if (msg[3] == 0x01)
                        {
                            byte val = msg[7];
                            retMessage = new W2311DigitalIOStatus(subindex, val);
                        }
                        else
                        {
                            retMessage = new W2311DigitalIOStatus(0x00, 0);
                        }
                    }
                }

              
            }

            return retMessage;
        }
    }
}

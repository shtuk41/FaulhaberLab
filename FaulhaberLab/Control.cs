using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaulhaberLab
{
    public class Control
    {
        private SerialPort port = new SerialPort("COM5", 1152, Parity.None, 8, StopBits.One);
        private string bufferRx = string.Empty;
        

        public Control()
        {
            try
            {
                if (port.IsOpen)
                {
                    port.Close();
                }

                port = new SerialPort("COM5", 115200, Parity.None, 8, StopBits.One);
                port.DataReceived += new SerialDataReceivedEventHandler(Port_DataReceived);
                port.ErrorReceived += new SerialErrorReceivedEventHandler(Port_ErrorReceived);
                port.RtsEnable = true;
                port.Handshake = Handshake.None;
                port.Open();

                if (port.IsOpen)
                {
                    BufferRx = "Port is open";
                }
            }
            catch (Exception e)
            {
                BufferRx = e.Message;
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
                    bufferRx += value;
                    bufferRx += "\n";
                    Console.Write(value);
                }
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer = new byte[100];

            int count = port.Read(buffer,0,100);

            BufferRx = string.Format("\nNumber of characters read: {0}\n", count);

            string hex = BitConverter.ToString(buffer);

            BufferRx = hex;
        }

        private void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            BufferRx = e.ToString();
        }

        public void SendByteBuffer(byte [] tx)
        {
            if (port.IsOpen)
            {
                Console.WriteLine("\nWriting " + tx);
                port.Write(tx, 0, tx.Count());
            }
        }

        public byte ComputeCRC(byte u8byte, byte u8CRC)
        {
            byte polynomial = 0xD5;
            byte i;
            u8CRC = (byte)(u8CRC ^ u8byte);

            for (i = 0;i < 8;i++)
            {
                if ((u8CRC & 0x01) != 0)
                {
                    u8CRC = (byte)((u8CRC >> 1) ^ polynomial);
                }
                else
                {
                    u8CRC >>= 1;
                }
            }

            return u8CRC;
        }
    }
}

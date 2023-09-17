using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faulhaber.Core.Common;
using Faulhaber.Core.Messages;

namespace Faulhaber.Core
{
    public class SerialPortOperator : ISerialClient, IDisposable
    {
        private bool disposed = false;

        private SerialPort port = new SerialPort("COM1", 1152, Parity.None, 8, StopBits.One);
        private string portName = string.Empty;
        ReceivingBuffer receivingBuffer = new ReceivingBuffer(1000);
        static readonly object eventLock = new object();

        public SerialPortOperator(SetResponse setResponse)
        {
            Response += setResponse;
        }

        #region delegates

        public delegate void SetResponse(string source, byte [] response = null);

        #endregion

        #region events

        public event SetResponse Response;

        #endregion

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            lock (eventLock)
            {
                byte[] buffer = new byte[port.BytesToRead];
                int count = port.Read(buffer, 0, port.BytesToRead);

                receivingBuffer.Add(buffer);

                byte[] recBuffer;

                while ((recBuffer = receivingBuffer.GetNext()) != null)
                {
                    Response("Serial", recBuffer);
                }
            }
        }

        private void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Response(e.ToString());
        }

        public void SendByteBuffer(byte[] tx)
        {
            if (port.IsOpen)
            {
                port.Write(tx, 0, tx.Count());
            }
        }

        public bool TryConnect(string portName)
        {
            try
            {
                if (port.IsOpen)
                {
                    port.Close();
                }

                port = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One);
                port.DataReceived += new SerialDataReceivedEventHandler(Port_DataReceived);
                port.ErrorReceived += new SerialErrorReceivedEventHandler(Port_ErrorReceived);
                port.RtsEnable = true;
                port.Handshake = Handshake.None;
                port.Open();

                if (port.IsOpen)
                {
                    Response("Port is open");
                }
            }
            catch (Exception e)
            {
                Response(e.Message);
            }

            return true;
        }

        public bool IsActive()
        {
            return !disposed && port.IsOpen;
        }


        #region implementing IDisposable

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
                if (port.IsOpen)
                {
                    port.Close();
                }

                port.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}

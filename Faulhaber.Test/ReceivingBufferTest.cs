using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Faulhaber.Core;

namespace Faulhaber.Test
{
    [TestClass]
    public class ReceivingBufferTest
    {
        private void PrintPositions(ReceivingBuffer buf)
        {
            Console.WriteLine(string.Format("S: {0}, E: {1}", buf.PositionS, buf.PositionE));
        }

        private void PrintByteArray(byte [] buf)
        {
            if (buf != null)
            {
                string stringByte = BitConverter.ToString(buf);
                Console.WriteLine(stringByte);
            }
        }

        [TestMethod]
        public void SimpleAddRemove()
        {
            ReceivingBuffer rb = new ReceivingBuffer(100);

            rb.Add(new byte[] { (byte)'S', 0x01, 0x02, 0x03, 0x04, (byte)'E' });

            Assert.AreEqual(rb.PositionS, 0);
            Assert.AreEqual(rb.PositionE, 6);

            PrintPositions(rb);
            PrintByteArray(rb.Buffer);

            rb.Add(new byte[] { (byte)'S', 0x03, 0x04, (byte)'E' });

            Assert.AreEqual(rb.PositionS, 0);
            Assert.AreEqual(rb.PositionE, 10);

            PrintPositions(rb);
            PrintByteArray(rb.Buffer);

            rb.Add(new byte[] { (byte)'S', 0x01, 0x04, (byte)'E' });

            Assert.AreEqual(rb.PositionS, 0);
            Assert.AreEqual(rb.PositionE, 14);
            
            PrintPositions(rb);
            PrintByteArray(rb.Buffer);

            rb.Add(new byte[] { (byte)'S', 0x01, 0x02, (byte)'E' });

            Assert.AreEqual(rb.PositionS, 0);
            Assert.AreEqual(rb.PositionE, 18);
            
            PrintPositions(rb);
            PrintByteArray(rb.Buffer);

            byte[] t1, t2, t3, t4;

            if (!rb.IsEmpty())
            {
                t1 = rb.GetNext();
                PrintPositions(rb);
                PrintByteArray(t1);
                PrintByteArray(rb.Buffer);
            }

            if (!rb.IsEmpty())
            {
                t2 = rb.GetNext();
                PrintPositions(rb);
                PrintByteArray(t2);
                PrintByteArray(rb.Buffer);
            }

            if (!rb.IsEmpty())
            {
                t3 = rb.GetNext();
                PrintPositions(rb);
                PrintByteArray(t3);
                PrintByteArray(rb.Buffer);
            }

            if (!rb.IsEmpty())
            {
                t4 = rb.GetNext();
                PrintPositions(rb);
                PrintByteArray(t4);
                PrintByteArray(rb.Buffer);
            }

            Assert.AreEqual(rb.IsEmpty(), true);
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void EdgeConditions()
        {
            ReceivingBuffer rb = new ReceivingBuffer(11);

            rb.Add(new byte[] { (byte)'S', 0x01, 0x02, 0x03, 0x04, (byte)'E' });

            Assert.AreEqual(rb.PositionS, 0);
            Assert.AreEqual(rb.PositionE, 6);

            PrintPositions(rb);
            PrintByteArray(rb.Buffer);

            rb.Add(new byte[] { (byte)'S', 0x03, 0x04, (byte)'E' });

            Assert.AreEqual(rb.PositionS, 0);
            Assert.AreEqual(rb.PositionE, 10);

            PrintPositions(rb);
            PrintByteArray(rb.Buffer);

            byte[] t1, t2;

            if (!rb.IsEmpty())
            {
                t1 = rb.GetNext();
                PrintPositions(rb);
                PrintByteArray(t1);
                PrintByteArray(rb.Buffer);
            }

            if (!rb.IsEmpty())
            {
                t2 = rb.GetNext();
                PrintPositions(rb);
                PrintByteArray(t2);
                PrintByteArray(rb.Buffer);
            }

            Assert.AreEqual(rb.IsEmpty(), true);
        }

        [TestMethod]
        public void CircularCheckSingleReadWrite1()
        {
            ReceivingBuffer rb = new ReceivingBuffer(15);

            int count = 0;

            while (count < 100000)
            {
                Console.WriteLine("Adding");
                rb.Add(new byte[] { (byte)'S', 0x01, 0x02, 0x03, 0x04, (byte)'E' });
                PrintPositions(rb);
                PrintByteArray(rb.Buffer);
                Console.WriteLine("Removing");
                byte[] output = rb.GetNext();
                PrintByteArray(output);
                PrintPositions(rb);
                PrintByteArray(rb.Buffer);
                count++;
            }

            Assert.AreEqual(true, rb.IsEmpty());
        }

        [TestMethod]
        public void CircularCheckSingleReadWrite2()
        {
            ReceivingBuffer rb = new ReceivingBuffer(15);

            int count = 0;

            while (count < 100000)
            {
                rb.Add(new byte[] { (byte)'S', 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, (byte)'E' });
                PrintPositions(rb);
                byte[] output = rb.GetNext();
                PrintPositions(rb);
                PrintByteArray(output);
                PrintByteArray(rb.Buffer);
                count++;
            }

            Assert.AreEqual(true, rb.IsEmpty());
        }

        [TestMethod]
        public void CircularCheckSingleReadWrite3()
        {
            ReceivingBuffer rb = new ReceivingBuffer(100);

            int count = 0;

            while (count < 100000)
            {
                rb.Add(new byte[] { (byte)'S', 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, (byte)'E' });
                rb.Add(new byte[] { (byte)'S', 0x01, 0x02, 0x03, 0x04, 0x05, (byte)'E' });
                rb.Add(new byte[] { (byte)'S', 0x01, 0x02, 0x03, 0x04, (byte)'E' });
                byte[] output = rb.GetNext();
                PrintByteArray(output);
                output = rb.GetNext();
                PrintByteArray(output);
                rb.Add(new byte[] { (byte)'S', 0x01, 0x02, 0x03, 0x04, 0x05, (byte)'E' });
                rb.Add(new byte[] { (byte)'S', 0x01, 0x02, 0x03, 0x04, (byte)'E' });
                output = rb.GetNext();
                PrintByteArray(output);
                output = rb.GetNext();
                PrintByteArray(output);
                output = rb.GetNext();
                PrintByteArray(output);

                count++;
            }

            Assert.AreEqual(true, rb.IsEmpty());
        }

        [TestMethod]
        public void CircularCheckSingleReadWrite4()
        {
            ReceivingBuffer rb = new ReceivingBuffer(100);

            int count = 0;

            while (count < 100000)
            {
                rb.Add(new byte[] { (byte)'S', 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, (byte)'E' });

                rb.Add(new byte[] { (byte)'S', 0x01, 0x02, 0x03, });
                rb.Add(new byte[] { 0x04, 0x05, 0x06, 0x07, (byte)'E' });

                rb.Add(new byte[] { (byte)'S', 0x01 });
                rb.Add(new byte[] { 0x02, 0x03, });
                rb.Add(new byte[] { 0x04, 0x05, (byte)'E' });

                rb.Add(new byte[] { (byte)'S', 0x01, 0x02, 0x03, 0x04, (byte)'E' });
                byte[] output = rb.GetNext();
                PrintByteArray(output);
                output = rb.GetNext();
                PrintByteArray(output);
                rb.Add(new byte[] { (byte)'S', 0x01, 0x02, });
                rb.Add(new byte[] { 0x03, 0x04, 0x05, (byte)'E' });

                rb.Add(new byte[] { (byte)'S', 0x01, 0x02, 0x03, 0x04, (byte)'E' });
                output = rb.GetNext();
                PrintByteArray(output);
                output = rb.GetNext();
                PrintByteArray(output);
                output = rb.GetNext();
                PrintByteArray(output);
                output = rb.GetNext();
                PrintByteArray(output);

                Console.WriteLine(string.Format("Count {0}", count));

                count++;
            }

            Assert.AreEqual(true, rb.IsEmpty());
        }

        [TestMethod]
        public void CircularGetNext()
        {
            ReceivingBuffer rb = new ReceivingBuffer(100);

            int count = 0;

            while (count < 100000)
            {
                rb.Add(new byte[] { (byte)'S', 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, (byte)'E' });

                rb.Add(new byte[] { (byte)'S', 0x01, 0x02, 0x03, });
                rb.Add(new byte[] { 0x04, 0x05, 0x06, 0x07, (byte)'E' });

                rb.Add(new byte[] { (byte)'S', 0x01 });
                rb.Add(new byte[] { 0x02, 0x03, });
                rb.Add(new byte[] { 0x04, 0x05, (byte)'E' });

                rb.Add(new byte[] { (byte)'S', 0x01, 0x02, 0x03, 0x04, (byte)'E' });

                rb.Add(new byte[] { (byte)'S', 0x01, 0x02, });
                rb.Add(new byte[] { 0x03, 0x04, 0x05, (byte)'E' });

                rb.Add(new byte[] { (byte)'S', 0x01, 0x02, 0x03, 0x04, (byte)'E' });

                byte[] receiveBuffer = null;

                int internalCount = 0;

                while ((receiveBuffer = rb.GetNext()) != null)
                {
                    PrintByteArray(receiveBuffer);
                    internalCount++;
                }

                Assert.AreEqual(6, internalCount);


                count++;
            }


        }
    }
}

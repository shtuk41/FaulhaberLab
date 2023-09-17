using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faulhaber.Core
{
    public class ReceivingBuffer
    {
        private byte[] buffer;
        private int positionS = 0;
        private int positionE = 0;
        private int bufferLength;

        public ReceivingBuffer(int size)
        {
            bufferLength = size;
            buffer = new byte[size];
        }

        public int PositionS
        {
            get
            {
                return positionS;
            }
        }

        public int PositionE
        {
            get
            {
                return positionE;
            }
        }

        public byte [] Buffer
        {
            get
            {
                return buffer;
            }
        }


        public void Add(byte [] data)
        {
            int bytesLeftInBuffer = bufferLength - positionE;

            if (bytesLeftInBuffer >= data.Count())
            {
                data.CopyTo(buffer, positionE);
                positionE += data.Count();

                if (positionE == bufferLength)
                    positionE = 0;
            }
            else
            {
                Array.Copy(data, 0, buffer, positionE, bytesLeftInBuffer);
                int bytelsLeftToWrite = data.Count() - bytesLeftInBuffer;
                int startIndex = bytesLeftInBuffer - data.Count();
                Array.Copy(data, bytesLeftInBuffer, buffer, 0, bytelsLeftToWrite);
                positionE = bytelsLeftToWrite;
            }
        }

        public byte [] GetNext()
        {
            if (positionS == positionE || 
                positionE == 0 && buffer[bufferLength - 1] != (byte)'E' ||
                (positionE > 0) &&  buffer[positionE - 1] != (byte)'E')
            {
                return null;
            }

            int lookahead;

            if ((positionS + 1) == bufferLength)
            {
                lookahead = 0;
            }
            else
            {
                lookahead = positionS + 1;
            }

            while (buffer[lookahead] != (byte)'E')
            {
                lookahead++;

                if (lookahead == bufferLength)
                    lookahead = 0;
            }

            if (lookahead > positionS)
            {
                int lengthL = lookahead - positionS + 1;
                byte[] retArray = new byte[lengthL];
                Array.Copy(buffer, positionS, retArray, 0, lengthL);
                positionS += lengthL;

                if (positionS >= bufferLength)
                {
                    positionS = positionS % bufferLength;
                }

                return retArray;
            }
            else
            {
                int lengthL = (bufferLength - positionS) + lookahead + 1;
                byte[] retArray = new byte[lengthL];
                Array.Copy(buffer, positionS, retArray, 0, bufferLength - positionS);
                Array.Copy(buffer, 0, retArray, bufferLength - positionS, lookahead + 1);
                positionS = lookahead + 1;
                return retArray;
            }
        }

        public bool IsEmpty()
        {
            return (positionS == positionE);
        }
    }
}

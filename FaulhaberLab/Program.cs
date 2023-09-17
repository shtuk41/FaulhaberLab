using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaulhaberLab
{
    class Program
    {
        static void Main(string[] args)
        {
            Control control = new Control();

            //Profile velocity
            byte byteSOF = (byte)'S';
            byte byteUserDataLength = 7;
            byte byteNodeNumber = 0;
            byte byteCommandCode =0x01;
            byte byteIndexLB = 0x81;
            byte byteIndexHB = 0x60;
            byte byteSubindex = 0x00;
            byte byteCrc = 0;
            byte byteEOF = (byte)'E';

           

            byte [] command = { byteSOF,
                                byteUserDataLength ,
                                byteNodeNumber,
                                byteCommandCode,
                                byteIndexLB,
                                byteIndexHB,
                                byteSubindex,
                                byteCrc,
                                byteEOF };

            byte y8CRC = 0xFF;

            y8CRC = control.ComputeCRC(command[1], y8CRC);
            y8CRC = control.ComputeCRC(command[2], y8CRC);
            y8CRC = control.ComputeCRC(command[3], y8CRC);
            y8CRC = control.ComputeCRC(command[4], y8CRC);
            y8CRC = control.ComputeCRC(command[5], y8CRC);
            y8CRC = control.ComputeCRC(command[6], y8CRC);

            command[7] = y8CRC;


            //control.SendByteBuffer(command);

            byteSOF = (byte)'S';
            byteUserDataLength = 7;
            byteNodeNumber = 0;
            byteCommandCode = 0x01;
            byteIndexLB = 0x18;
            byteIndexHB = 0x10;
            byteSubindex = 0x02;
            byteCrc = 0;
            byteEOF = (byte)'E';

            command[0] = byteSOF;
            command[1] = byteUserDataLength;
            command[2] = byteNodeNumber;
            command[3] = byteCommandCode;
            command[4] = byteIndexLB;
            command[5] = byteIndexHB;
            command[6] = byteSubindex;
            command[8] = byteEOF;

            y8CRC = 0xFF;

            y8CRC = control.ComputeCRC(command[1], y8CRC);
            y8CRC = control.ComputeCRC(command[2], y8CRC);
            y8CRC = control.ComputeCRC(command[3], y8CRC);
            y8CRC = control.ComputeCRC(command[4], y8CRC);
            y8CRC = control.ComputeCRC(command[5], y8CRC);
            y8CRC = control.ComputeCRC(command[6], y8CRC);

            command[7] = y8CRC;

            control.SendByteBuffer(command);

            byteSOF = (byte)'S';
            byteUserDataLength = 4;
            byteNodeNumber = 0;
            byteCommandCode = 0x00;
            byteCrc = 0;
            byteEOF = (byte)'E';

            command[0] = byteSOF;
            command[1] = byteUserDataLength;
            command[2] = byteNodeNumber;
            command[3] = byteCommandCode;
            command[4] = byteCrc;
            command[5] = byteEOF;

            y8CRC = 0xFF;

            y8CRC = control.ComputeCRC(command[1], y8CRC);
            y8CRC = control.ComputeCRC(command[2], y8CRC);
            y8CRC = control.ComputeCRC(command[3], y8CRC);

            //control.SendByteBuffer(command);

            Console.ReadKey();
        }
   
    }
}

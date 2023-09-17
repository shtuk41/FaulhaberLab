using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faulhaber.Core.Common
{
    public interface ISerialClient
    {
        void SendByteBuffer(byte[] tx);
        bool TryConnect(string portName);
        bool IsActive();
    }
}

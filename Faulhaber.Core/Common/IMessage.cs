using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faulhaber.Core.Common
{
    public interface IMessage
    {
        void Execute();
        int Index();
        byte SubIndex();
        object Value();
     }
}

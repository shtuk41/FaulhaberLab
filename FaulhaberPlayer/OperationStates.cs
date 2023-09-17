using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaulhaberPlayer
{
    public enum OperationStates
    {
        Neutral = 0,

        TargetReached = 1,

        MotionStarted = 2,

        MotionRequested = 3,

        DigitalIOStatusRequested = 4,

        DigitalIOStatusProvided = 5,

        HomingRequested = 8
    }
}

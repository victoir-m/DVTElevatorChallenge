using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTElevatorChallenge
{
    public enum RequestStatus
    {
        Success,
        NoAvailableElevator,
        ElevatorFull,
        InvalidRequest
    }
      
    public enum ElevatorDirection 
    { 
        Up = 1,
        Down = 2
    }
}
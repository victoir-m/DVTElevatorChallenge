using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTElevatorChallenge
{
    public interface IElevatorBL
    {
        void RequestFloor(int floor);

        void RequestDirection(ElevatorDirection direction);

        void AddElevators(ElevatorModel elevator);

        void RequestElevator(ElevatorRequestModel elevatorRequest);

    }
}

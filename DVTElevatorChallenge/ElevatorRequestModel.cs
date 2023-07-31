using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTElevatorChallenge
{
    public class ElevatorRequestModel
    {
        public int CurrentFloor { get; set; }
        public int DestinationFloor { get; set; }
        public ElevatorDirection Direction { get; set; }
        public int NumberOfPeople { get; set; }
        public bool arrived { get; set; } = false;
    }
}

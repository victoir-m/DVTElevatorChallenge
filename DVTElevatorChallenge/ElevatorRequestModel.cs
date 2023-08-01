using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTElevatorChallenge
{
    public class ElevatorRequestModel
    {
        public int CurrentFloor { get; set; }//curent floor the user is on
        public int DestinationFloor { get; set; }//floor to which the user is going
        public ElevatorDirection Direction { get; set; }//direction the user is going
        public int NumberOfPeople { get; set; }//number of people in th egorup
        public bool arrived { get; set; } = false;//has the user arrvied at the destiantion floor .. defaulted to false
    }
}

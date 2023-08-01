using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTElevatorChallenge
{
    public class ElevatorModel
    {
        //Name to identify different elevators
        //data type string
        public string Alias { get; set; }
        //Current floor of the elevator, initialized to 1
        //data typt int
        public int Floor { get; set; }
        //Direction in which the elevator is moving.
        //data type ElevatorDirection enum
        public ElevatorDirection Direction { get; set; }
        //Maximum number of people that can be in an elevator.
        //data type int
        public int MaxCapacity { get; set; }
        //current number of people in an elevator.
        //data type int
        public int CurrentCapacity { get; set; }
        //List of elevator requests 
        //data type ElevatorRequestModel
        public List<ElevatorRequestModel> ElevatorRequests { get; set; }

        public ElevatorModel(){
            Floor = 1; //initial elevator floor is 1
            Direction = ElevatorDirection.Up; // initial elevator movement direction is up
            MaxCapacity = 10; // maximum number of people 10
            ElevatorRequests = new List<ElevatorRequestModel>();// empty  elevator requests
        }

        /// <summary>
        ///     Check if the number of people waiting for the elevator can enter into the elevator
        ///     <param name="numOfPeople">The number of people that will enter the elevator </param>  expected data type int
        ///     <returns>true or false</returns>
        /// </summary>
        public bool CanEnterElevator(int numOfPeople)
        {
            return CurrentCapacity + numOfPeople <= MaxCapacity;
        }

        /// <summary>
        ///     Encrease the CurrentCapacity of the elevator by the number of people entering
        ///     <param name="numberOfPeople">The number of people entering the elevator </param>  expected data type int
        /// </summary>
        public void EnterElevator(int numberOfPeople)
        {
            if (CanEnterElevator(numberOfPeople)) CurrentCapacity += numberOfPeople;           
        }

        /// <summary>
        ///     Reduce the CurrentCapacity of the elevator by the number of people leaving at that specific floor
        ///     <param name="numOfPeople">The number of people that will be leaving the elevator </param>  expected data type int
        /// </summary>
        public void ExitElevator(int numberOfPeople)
        {
            if (CurrentCapacity - numberOfPeople >= 0) CurrentCapacity -= numberOfPeople;
        }

        /// <summary>
        ///     Remove the request form the list of ElevatorRequests when arrived is true
        /// </summary>
        public void RemoveRequestModel() => ElevatorRequests.RemoveAll(r => r.arrived);

        /// <summary>
        ///     Move elevator to the next floor 
        ///     <param name="numberOfFloors">The number of flloors in the building </param>  expected data type int
        ///     <param name="movingToRequestFloor">Is the elevator moving to the requested floor </param>  expected data type bool
        /// </summary>
        public int Move(int numberOfFloors, bool movingToRequestFloor)
        {

            if (!movingToRequestFloor || ElevatorRequests.Count > 0)
            {
                CheckIfRequestIsAtDestiantionFloor();
                RemoveRequestModel();
            }

            if (CheckIfElvatorHasRequests())
            {
                return ElevatorMovement(numberOfFloors);
            }
            else if (!CheckIfElvatorHasRequests() && movingToRequestFloor)
            {
                return ElevatorMovement(numberOfFloors);
            }
            else {
                WriteElevatorMovementConsole();
                return 0; 
            }
        }

        /// <summary>
        ///     Check if the elevator has arrived at aone the requests destination floor
        /// </summary>
        public void CheckIfRequestIsAtDestiantionFloor()
        {
            if(ElevatorRequests.Count > 0)
            {
                foreach (var item in ElevatorRequests)
                {
                    if (item.DestinationFloor == Floor)
                    {
                        item.arrived = true;
                        WriteElevatorMovementConsole();
                        ExitElevator(item.NumberOfPeople);
                    }
                }
            }
        }

        /// <summary>
        ///     Add request to ElevatorRequests
        ///     <param name="elevatorRequest">New elevator reqiest model</param>  expected data type ElevatorRequestModel
        /// </summary>
        public void AddElevatorRequest(ElevatorRequestModel elevatorRequest) => ElevatorRequests.Add(elevatorRequest);

        /// <summary>
        ///     Update the elevator Floor
        ///     <param name="newFloor">New floor </param>  expected data type int
        /// </summary>
        public void ChangeFloorNumber(int newFloor)
        {
            Floor = newFloor;
        }

        /// <summary>
        ///     Write elevator movements to the console
        /// </summary>
        public void WriteElevatorMovementConsole()
        {
            string currentDirection = "";
            string movingDirection = Direction.Equals(ElevatorDirection.Up) ? "Up" :"Down";
            if (CheckIfElvatorHasRequests())
                currentDirection = $@" carrying {CurrentCapacity} people and it is moving {movingDirection}";
            else
                currentDirection = "Currently stationarty";

            Console.WriteLine($"------------------------------------{Alias} Movement----------------------------------------");

            Console.WriteLine();

            Console.WriteLine($"{Alias} is currently at level: {Floor}, {currentDirection}");

            Console.WriteLine();

        }

        /// <summary>
        ///     Check if the elevator has any requests
        /// </summary>
        public bool CheckIfElvatorHasRequests()
        {

            if (ElevatorRequests.Count == 0)
            {
                return false;
            }
            else return true;
        }

        /// <summary>
        ///     Simulate elevator movement from floor to floor
        /// </summary>
        public int ElevatorMovement(int numberOfFloors)
        {
            int currentFloor = Floor;
            WriteElevatorMovementConsole();
            if (currentFloor == 1) return currentFloor += 1;
            else if (currentFloor == numberOfFloors && Direction.Equals(ElevatorDirection.Up))
            {
                Direction = ElevatorDirection.Down;
                return currentFloor -= 1;
            }
            else return Direction == ElevatorDirection.Up ? currentFloor += 1 : currentFloor -= 1;


        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTElevatorChallenge
{
    public class ElevatorModel
    {
        public string Alias { get; set; }
        public int Floor { get; set; }
        public ElevatorDirection Direction { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentCapacity { get; set; }
        public List<ElevatorRequestModel> ElevatorRequests { get; set; }

        public ElevatorModel(){
            Floor = 1;
            Direction = ElevatorDirection.Up;
            MaxCapacity = 10;
            ElevatorRequests = new List<ElevatorRequestModel>();
        }

        public bool CanEnterElevator(int numOfPeople)
        {
            return CurrentCapacity + numOfPeople <= MaxCapacity;
        }

        public void EnterElevator(int numberOfPeople)
        {
            if (CanEnterElevator(numberOfPeople)) CurrentCapacity += numberOfPeople;           
        }

        public void ExitElevator(int numberOfPeople)
        {
            if (CurrentCapacity - numberOfPeople >= 0) CurrentCapacity -= numberOfPeople;
        }

        public void RemoveRequestModel() => ElevatorRequests.RemoveAll(r => r.arrived);

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

        public void AddElevatorRequest(ElevatorRequestModel elevatorRequest) => ElevatorRequests.Add(elevatorRequest);

        public void ChangeFloorNumber(int newFloor)
        {
            Floor = newFloor;
        }

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
    
        public bool CheckIfElvatorHasRequests()
        {

            if (ElevatorRequests.Count == 0)
            {
                return false;
            }
            else return true;
        }
    
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

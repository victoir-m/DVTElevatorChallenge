using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTElevatorChallenge
{
    public class ElevatorBL
    {
        List<ElevatorModel> elevators = new List<ElevatorModel>();
        int numberOfFloors;

        public void AddElevators(ElevatorModel elevator)
        {
            elevators.Add(elevator);
        }

        public void AddNumberOfFloors(int numberOfFloors)
        {
            this.numberOfFloors = numberOfFloors;
        }

        public async Task<ElevatorRequestResponseModel> RequestElevator(ElevatorRequestModel elevatorRequest)
        {

            if (elevatorRequest.CurrentFloor == numberOfFloors && elevatorRequest.Direction.Equals(ElevatorDirection.Up)) return new ElevatorRequestResponseModel
            {
                RequestStatus = RequestStatus.InvalidRequest,
                Message = "You have made an invalid request, Elevator cannot go above this floor"
            };

            if (elevatorRequest.CurrentFloor.Equals(1) && elevatorRequest.Direction.Equals(ElevatorDirection.Down)) return new ElevatorRequestResponseModel
            {
                RequestStatus = RequestStatus.InvalidRequest,
                Message = "You have made an invalid request, Elevator cannot go below this floor"
            };

            ElevatorModel nearestElevator = FindNearestElevator(elevatorRequest.CurrentFloor);

            if(nearestElevator == null)
            {
                return new ElevatorRequestResponseModel {
                    RequestStatus = RequestStatus.NoAvailableElevator,
                    Message = "There are no available elevators at the moment, please try again."
                };
            }

            Console.WriteLine();

            if (nearestElevator.Floor < elevatorRequest.CurrentFloor) { 
                nearestElevator.Direction = ElevatorDirection.Up;
                Console.WriteLine($"{nearestElevator.Alias} is on its way!");

            }
            if (nearestElevator.Floor > elevatorRequest.CurrentFloor) {
                nearestElevator.Direction = ElevatorDirection.Down;

                Console.WriteLine($"{nearestElevator.Alias} is on its way!");
            }

            Console.WriteLine();

            await MoveElevatorToRequestFloor(nearestElevator, elevatorRequest.CurrentFloor);

            bool canEnterElevator = nearestElevator.CanEnterElevator(elevatorRequest.NumberOfPeople);

            if (nearestElevator.Floor == elevatorRequest.CurrentFloor)
            {
                nearestElevator.Direction = elevatorRequest.Direction;
                Console.WriteLine($"{nearestElevator.Alias} is here");
                if (canEnterElevator)
                {
                    nearestElevator.AddElevatorRequest(elevatorRequest);

                    nearestElevator.EnterElevator(elevatorRequest.NumberOfPeople);
                    Console.WriteLine($"Entered {nearestElevator.Alias}");
                    await Task.Run(() => MoveElavators());
                    return new ElevatorRequestResponseModel
                    {
                        RequestStatus = RequestStatus.Success,
                        Message = $"Successfull request, {nearestElevator.Alias} is  moving"
                    };
                }
                else return new ElevatorRequestResponseModel
                {
                    RequestStatus = RequestStatus.ElevatorFull,
                    Message = $"Elavator { nearestElevator.Alias } is full. Would you like to request again? (yes/no)"
                };
            }
            else return null;
        }

        private ElevatorModel FindNearestElevator(int requestingFloor)
        {
            ElevatorModel nearestElevator = elevators.Where(x => x.Direction.Equals(ElevatorDirection.Up) && x.Floor <= requestingFloor)
                                                     .OrderBy(x => Math.Abs(x.Floor - requestingFloor))
                                                     .FirstOrDefault() ??
                                                     elevators
                                                     .Where(x => x.Direction.Equals(ElevatorDirection.Down) && x.Floor >= requestingFloor)
                                                     .OrderBy(x => Math.Abs(x.Floor - requestingFloor))
                                                     .FirstOrDefault();

            return nearestElevator;
        }


        public async Task MoveElavators()
        {
            for (int i = 0; i < numberOfFloors; i++)
            {
                foreach (var item in elevators)
                {
                    int newFloor = item.Move(numberOfFloors, false);
                    if (newFloor != 0) item.ChangeFloorNumber(newFloor);
                }

                await Task.Delay(1000);
            }
        }
    
        public async Task MoveElevatorToRequestFloor(ElevatorModel nearestElevator, int requestFloor)
        {
            for (int i = 0; i < requestFloor; i++)
            {
                if(nearestElevator.Floor != requestFloor)
                {
                    int newFloor = nearestElevator.Move(requestFloor, true);
                    if (newFloor != 0) nearestElevator.ChangeFloorNumber(newFloor);
                }

                await Task.Delay(1000);
            }

            nearestElevator.WriteElevatorMovementConsole();
        }
    }
}

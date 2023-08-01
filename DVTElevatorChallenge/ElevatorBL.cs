using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTElevatorChallenge
{
    public class ElevatorBL
    {
        List<ElevatorModel> elevators = new List<ElevatorModel>(); //create a new list of elevators
        int numberOfFloors; // number of floors in the building
        const int movementDurationInSeconds = 3; // the duration of the movement of elevators after when requested. this will allow for multiple requestes to be done

        /// <summary>
        ///     Add elevator to the list of elevators
        ///     <param name="elevator">new elevator model</param>  expected data type ElevatorModel
        /// </summary>
        public void AddElevators(ElevatorModel elevator)
        {
            elevators.Add(elevator);
        }

        /// <summary>
        ///     Add or initialize number of floors
        ///     <param name="numberOfFloors">The number of floors in a building </param>  expected data type int
        /// </summary>
        public void AddNumberOfFloors(int numberOfFloors)
        {
            this.numberOfFloors = numberOfFloors;
        }

        /// <summary>
        ///     Make an elevator request and process it asynchronously
        ///     <param name="elevatorRequest"></param>  expected data type ElevatorRequestModel
        /// </summary>
        public async Task<ElevatorRequestResponseModel> RequestElevator(ElevatorRequestModel elevatorRequest)
        {
            //ensure that it a valid elavator request
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

            ElevatorModel nearestElevator = FindNearestElevator(elevatorRequest.CurrentFloor);//find the nearest elevator

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

            await MoveElevatorToRequestFloor(nearestElevator, elevatorRequest.CurrentFloor); //move the elevator you have found to the requested floor

            bool canEnterElevator = nearestElevator.CanEnterElevator(elevatorRequest.NumberOfPeople);// check if people can enter into the elevator

            if (nearestElevator.Floor == elevatorRequest.CurrentFloor)
            {
                nearestElevator.Direction = elevatorRequest.Direction;
                Console.WriteLine($"{nearestElevator.Alias} is here");
                if (canEnterElevator)
                {
                    nearestElevator.AddElevatorRequest(elevatorRequest);// add the elevator request to list of requests 

                    nearestElevator.EnterElevator(elevatorRequest.NumberOfPeople);// allow people to enter people the elevator
                    Console.WriteLine($"Entered {nearestElevator.Alias}");
                    await Task.Run(() => MoveElavators());// start elevator movement
                    return new ElevatorRequestResponseModel
                    {
                        RequestStatus = RequestStatus.Success,
                        Message = $"Successfull request, elevators are moving"
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

        /// <summary>
        ///     Find the nearest elevator to floor on which the request was made
        ///     <param name="requestingFloor">The floor on which the request was made </param>  expected data type int
        /// </summary>
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

        /// <summary>
        ///     Simulate the movement of elevators as asynchronously
        /// </summary>
        public async Task MoveElavators()
        {
            for (int i = 0; i < movementDurationInSeconds; i++)
            {
                foreach (var item in elevators)
                {
                    int newFloor = item.Move(numberOfFloors, false);
                    if (newFloor != 0) item.ChangeFloorNumber(newFloor);//change the floor number as it moves
                }

                await Task.Delay(1000);//delay movement updates by 1 second
            }
        }

        /// <summary>
        ///     Move elevator to the requested floor 
        ///     <param name="nearestElevator">Nearest elevator to the request floor </param>  expected data type ElevatorModel
        ///     <param name="requestFloor">Floor on which th request was made </param>  expected data type int
        /// </summary>
        public async Task MoveElevatorToRequestFloor(ElevatorModel nearestElevator, int requestFloor)
        {
            for (int i = 0; i < requestFloor; i++)
            {
                if(nearestElevator.Floor != requestFloor)
                {
                    int newFloor = nearestElevator.Move(requestFloor, true);
                    if (newFloor != 0) nearestElevator.ChangeFloorNumber(newFloor);//change the floor number
                }

                await Task.Delay(1000);//deleay movement updates by 1 second 
            }

            nearestElevator.WriteElevatorMovementConsole();//write the to the console the elevators current activity
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTElevatorChallenge
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ElevatorBL elevatorBL = new ElevatorBL();
            ApplicationInput input = new ApplicationInput();
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("----------------------System Setup----------------------------");
            Console.WriteLine("Please enter the number of elevators? | 0 to stop the program");
            int numberOfElevators;

            input.InputInger(out numberOfElevators);

            for(int i = 0; i < numberOfElevators; i++)//add elevators to the list of elevators
            {
                ElevatorModel elevator = new ElevatorModel()
                {
                    Alias = $"Elevator {i + 1}",
                    CurrentCapacity = 0
                };

                elevatorBL.AddElevators(elevator);
            }

            Console.WriteLine("Elevators added successfully");


            Console.WriteLine();
            Console.WriteLine("Please enter the number of floors?| 0 to stop the program");
            int numberOfFloors;

            input.InputInger(out numberOfFloors);

            elevatorBL.AddNumberOfFloors(numberOfFloors);//add the number of floors

            Console.WriteLine("Number Of added successfully");

            Console.WriteLine("--------------------------------------------------------------");


            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("----------------------Elevator----------------------------");

            try
            {
                while (true)
                {

                    Console.WriteLine("Are you going up or Down (Enter 1 for Up and 2 for Down | 0 to stop the program)?");
                    int direction;

                    input.InputInger(out direction);

                    Console.WriteLine();
                    Console.WriteLine("Please enter the current floor you are on? | 0 to stop the program");
                    int currentFloor;

                    input.InputInger(out currentFloor);

                    Console.WriteLine();
                    Console.WriteLine("Which floor are you going to? | 0 to stop the program");
                    int destination;

                    input.InputInger(out destination);

                    Console.WriteLine();
                    Console.WriteLine("How many people are with you? | 0 to stop the program");
                    int numberOfPeople;

                    input.InputInger(out numberOfPeople);

                    ElevatorRequestModel requestModel = new ElevatorRequestModel//create new quest model from user inputs
                    {
                        CurrentFloor = currentFloor,
                        DestinationFloor = destination,
                        Direction = direction == 1 ? ElevatorDirection.Up : ElevatorDirection.Down,
                        NumberOfPeople = numberOfPeople,
                        arrived = false
                    };

                    ElevatorRequestResponseModel elevatorRequestResponse = await elevatorBL.RequestElevator(requestModel);//make a new elevator request

                    Console.WriteLine();
                    if (elevatorRequestResponse != null && elevatorRequestResponse.RequestStatus.Equals(RequestStatus.Success))
                    {
                        Console.WriteLine($"{elevatorRequestResponse.Message}");
                        Console.WriteLine();
                    }
                    else if (elevatorRequestResponse != null && elevatorRequestResponse.RequestStatus.Equals(RequestStatus.NoAvailableElevator))
                    {
                        Console.WriteLine($"{elevatorRequestResponse.Message}");
                        Console.WriteLine();
                        break;
                    }
                    else if (elevatorRequestResponse != null && elevatorRequestResponse.RequestStatus.Equals(RequestStatus.InvalidRequest))
                    {
                        Console.WriteLine($"{elevatorRequestResponse.Message}");
                        Console.WriteLine();
                        break;
                    }
                    else if (elevatorRequestResponse != null && elevatorRequestResponse.RequestStatus.Equals(RequestStatus.ElevatorFull))
                    {
                        Console.WriteLine($"{elevatorRequestResponse.Message}");
                        Console.WriteLine();
                        string response = Console.ReadLine().ToLower();

                        if (response.Equals("no"))
                            break;

                    }

                }

            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong in the request please contact support");
                Environment.Exit(0);
            }
        }

        public bool StopProgram(int intput)
        {
            return intput == 0;
        }
    }
}

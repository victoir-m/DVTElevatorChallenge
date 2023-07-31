using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTElevatorChallenge
{
    [TestClass]
    public class ElevatorUnitTest
    {
        [TestMethod]
        public async Task SuccessFullRequestTest()
        {
            ElevatorBL elevatorBL = new ElevatorBL();
            AddElevators(elevatorBL,3, 9, 10, 4);//ElevatorBL elevatorBL, int numberOfElevators, int numberOfFloors, int maxCapacity, int currentCapacity
            RequestStatus requestStatus = await GetElevatorRequestResponse(elevatorBL,3, 6, 1, 4);//ElevatorBL elevatorBL, int currentFloor, int destination, int direction, int numberOfPeople

            Assert.AreEqual(RequestStatus.Success,requestStatus);
        }

        [TestMethod]
        public async Task NoAvailableElevatorRequestTest()
        {
            ElevatorBL elevatorBL = new ElevatorBL();
            //AddElevators(elevatorBL,3, 9, 10, 4);//ElevatorBL elevatorBL, int numberOfElevators, int numberOfFloors, int maxCapacity, int currentCapacity
            RequestStatus requestStatus = await GetElevatorRequestResponse(elevatorBL,3, 6, 1, 4);//ElevatorBL elevatorBL, int currentFloor, int destination, int direction, int numberOfPeople

            Assert.AreEqual(RequestStatus.NoAvailableElevator, requestStatus);
        }

        [TestMethod]
        public async Task InvalidRequestTest()
        {
            ElevatorBL elevatorBL = new ElevatorBL();
            AddElevators(elevatorBL,3, 9, 10, 4);//ElevatorBL elevatorBL, int numberOfElevators, int numberOfFloors, int maxCapacity, int currentCapacity
            RequestStatus requestStatus = await GetElevatorRequestResponse(elevatorBL,9, 6, 1, 4);//ElevatorBL elevatorBL, int currentFloor, int destination, int direction, int numberOfPeople

            Assert.AreEqual(RequestStatus.InvalidRequest, requestStatus);

        }

        [TestMethod]
        public async Task ElevatorFull()
        {
            ElevatorBL elevatorBL = new ElevatorBL();
            AddElevators(elevatorBL,3, 9, 10, 10);//ElevatorBL elevatorBL, int numberOfElevators, int numberOfFloors, int maxCapacity, int currentCapacity
            RequestStatus requestStatus = await GetElevatorRequestResponse(elevatorBL,3, 6, 1, 4);//ElevatorBL elevatorBL, int currentFloor, int destination, int direction, int numberOfPeople

            Assert.AreEqual(RequestStatus.ElevatorFull, requestStatus);

        }

        private async Task<RequestStatus> GetElevatorRequestResponse(ElevatorBL elevatorBL, int currentFloor, int destination, int direction, int numberOfPeople)
        {

            ElevatorRequestModel requestModel = new ElevatorRequestModel
            {
                CurrentFloor = currentFloor,
                DestinationFloor = destination,
                Direction = direction == 1 ? ElevatorDirection.Up : ElevatorDirection.Down,
                NumberOfPeople = numberOfPeople,
                arrived = false
            };

            ElevatorRequestResponseModel elevatorRequestResponse = await elevatorBL.RequestElevator(requestModel);

            return elevatorRequestResponse.RequestStatus;
        }

        public void AddElevators(ElevatorBL elevatorBL, int numberOfElevators, int numberOfFloors, int maxCapacity, int currentCapacity)
        {

            elevatorBL.AddNumberOfFloors(numberOfFloors);


            for (int i = 0; i < numberOfElevators; i++)
            {
                ElevatorModel elevator = new ElevatorModel()
                {
                    Alias = $"Elevator {i + 1}",
                    CurrentCapacity = currentCapacity,
                    MaxCapacity = maxCapacity,
                };

                elevatorBL.AddElevators(elevator);
            }


        }
    }
}

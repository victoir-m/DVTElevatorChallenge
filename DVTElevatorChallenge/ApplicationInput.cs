using System;

namespace DVTElevatorChallenge
{
    public class ApplicationInput // class to accept application inputs  and check if it's the correct input and in the correct format
    {
        /// <summary>
        ///     Get integer inputs from the user via the console using out parameter 
        ///     <param name="validInput"></param>  expected data type int
        /// </summary>
        public void InputInger(out int validInput)
        {
            bool isValidIntergerInput = false;
            validInput = 0;// assign default vaule incase of invalid inputs
            while (!isValidIntergerInput)
            {
                string input = Console.ReadLine();

                if (int.TryParse(input, out validInput))
                {
                    if (validInput == 0)
                    {
                        Console.WriteLine("Application stopped.");
                        Environment.Exit(0);//stops the application if the user enters 0
                    }
                    isValidIntergerInput = true;                    
                }
                else
                {
                    Console.WriteLine("Invalid input, only numeric characters are accpeted, Please enter a valid numeric character.");
                }
            }
        }
    }
}

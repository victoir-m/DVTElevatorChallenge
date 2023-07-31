using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTElevatorChallenge
{
    public class ApplicationInput
    {
        public void InputInger(out int validInput)
        {
            bool isValidIntergerInput = false;
            validInput = 0;
            while (!isValidIntergerInput)
            {
                string input = Console.ReadLine();

                if (int.TryParse(input, out validInput))
                {
                    if (validInput == 0)
                    {
                        Console.WriteLine("Application stopped.");
                        Environment.Exit(0);
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

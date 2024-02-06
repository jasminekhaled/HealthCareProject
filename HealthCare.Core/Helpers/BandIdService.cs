using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Helpers
{
    public static class BandIdService
    {
        private static int globalCounter = 0;

        public static string GenerateUniqueString()
        {
            // Use a timestamp and a global counter to create a unique string
            int uniqueNumber = (int)(DateTimeOffset.Now.ToUnixTimeSeconds() % 1000000) * 1000 + globalCounter;

            // Increment the global counter for the next unique string
            globalCounter++;

            // Format the unique number as a string with leading zeros if needed
            string uniqueString = uniqueNumber.ToString("D6");

            return uniqueString;
        }
    }
}

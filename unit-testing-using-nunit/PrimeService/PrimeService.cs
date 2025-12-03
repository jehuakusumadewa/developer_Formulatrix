using System;

namespace Prime.Services
{
    public class PrimeService
    {
        public bool IsPrime(int candidate)
        {
            if (candidate < 2) // Handles -1, 0, and 1
            {
                return false;
            }


            throw new NotImplementedException("Please create a test first.");
        }
    }
}
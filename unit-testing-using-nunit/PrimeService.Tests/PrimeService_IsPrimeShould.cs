
using NUnit.Framework;
using Prime.Services;

namespace Prime.UnitTests.Services
{
    [TestFixture] // Marks the class as containing unit tests
    public class PrimeService_IsPrimeShould
    {
        private PrimeService _primeService;

        [SetUp] // This method is executed before each test method in the fixture
        public void SetUp()
        {
            _primeService = new PrimeService(); // Initialize the service instance for each test
        }

        // [Test] // Marks this method as a unit test
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void IsPrime_InputIs1_ReturnFalse(int value)
        {
            // Arrange (setup the test conditions)
            var result = _primeService?.IsPrime(value);

            // Assert (verify the outcome)
            Assert.That(result, Is.False, $"{value} should not be prime");
        }
    }
}


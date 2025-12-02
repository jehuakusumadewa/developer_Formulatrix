using Xunit;

namespace CalculatorTests
{
    public class CalculatorTests
    {
        [Fact]
        public void add_WhenGivenTwoNumbers_ReturnsSum()
        {
            var calculator = new CalculatorLib.calculator();
            var result = calculator.Add(2, 3);
            Assert.Equal(5, result);
        }

        [Fact]
        public void add_WhenGivenDifferentNumbers_ReturnsSum()
        {
            var calculator = new CalculatorLib.calculator();
            var result = calculator.Add(10, 5);
            Assert.Equal(15, result);
        }
    }
}
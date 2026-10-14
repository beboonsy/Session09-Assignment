using Session09App;

namespace Session09.Tests
{
    public class FineCalculatorTests
    {
        // The calculator is created once per test in the constructor (xUnit creates a new
        // instance of the test class for every test, so tests stay independent).
        private readonly FineCalculator _calculator;

        public FineCalculatorTests()
        {
            _calculator = new FineCalculator();
        }

        // 1) [Fact] + Assert.Equal
        [Fact]
        public void Add_ShouldReturnCorrectSum()
        {
            // Arrange
            int a = 10;
            int b = 5;

            // Act
            int result = _calculator.Add(a, b);

            // Assert
            Assert.Equal(15, result);
        }

        // 2) [Fact] + Assert.NotEqual
        [Fact]
        public void Add_ShouldReturnNotEqual()
        {
            // Arrange
            int a = 10;
            int b = 5;

            // Act
            int result = _calculator.Add(a, b);

            // Assert
            Assert.NotEqual(20, result);
        }

        // 3) [Fact] + Assert.Equal
        [Fact]
        public void Subtract_ShouldReturnCorrectDifference()
        {
            // Arrange
            int a = 10;
            int b = 4;

            // Act
            int result = _calculator.Subtract(a, b);

            // Assert
            Assert.Equal(6, result);
        }

        // 4) [Theory] + 3 x [InlineData] (including a negative-number case) + Assert.Equal
        [Theory]
        [InlineData(2, 3, 6)]
        [InlineData(-4, 5, -20)]
        [InlineData(0, 9, 0)]
        public void Multiply_ShouldReturnCorrectResult(int a, int b, int expected)
        {
            // Arrange (inputs come from InlineData)

            // Act
            int result = _calculator.Multiply(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        // 5) [Theory] + 3 x [InlineData] + Assert.NotEqual
        [Theory]
        [InlineData(2, 3, 7)]
        [InlineData(-4, 5, 20)]
        [InlineData(0, 9, 1)]
        public void Multiply_ShouldReturnNotEqual(int a, int b, int wrongValue)
        {
            // Arrange (inputs come from InlineData)

            // Act
            int result = _calculator.Multiply(a, b);

            // Assert
            Assert.NotEqual(wrongValue, result);
        }

        // 6) [Fact] + Assert.Throws<DivideByZeroException>
        [Fact]
        public void Divide_ByZero_ShouldThrowException()
        {
            // Arrange
            int a = 10;
            int b = 0;

            // Act + Assert
            Assert.Throws<DivideByZeroException>(() => { _calculator.Divide(a, b); });
        }
    }
}
namespace Caleb_Marks_Assignment_4._1._2
{
    // Do the four calculations promised by the calculator interface
    internal class Math : ICalculator
    {
        public decimal Add(decimal first, decimal second)
        {
            return first + second;
        }

        public decimal Subtract(decimal first, decimal second)
        {
            return first - second;
        }

        public decimal Multiply(decimal first, decimal second)
        {
            return first * second;
        }

        public decimal Divide(decimal first, decimal second)
        {
            return first / second;
        }
    }
}

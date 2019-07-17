namespace SWD.Conditions
{
    public class Present : AssertCondition
    {
        public override bool Invoke<T>(T element)
        {
            return element.Present();
        }

        protected override string DescribeExpected()
        {
            return $"Enabled {true}";
        }

        protected override string DescribeActual()
        {
            return $"Enabled {false}";
        }
    }

    public static partial class Be
    {
        public static AssertCondition Present => new Present();
    }
}
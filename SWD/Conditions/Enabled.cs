namespace SWD.Conditions
{
    public class Enabled : AssertCondition
    {
        public override bool Invoke<T>(T element)
        {
            return element.Enabled;
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
        public static AssertCondition Enabled => new Enabled();
    }
}
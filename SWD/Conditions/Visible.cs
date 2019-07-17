namespace SWD.Conditions
{
    public class Visible : AssertCondition
    {
        public override bool Invoke<T>(T element)
        {
            return element.Displayed;
        }

        protected override string DescribeExpected()
        {
            return $"Displayed {true}";
        }

        protected override string DescribeActual()
        {
            return $"Displayed {false}";
        }
    }

    public static partial class Be
    {
        public static AssertCondition Visible => new Visible();
    }
}
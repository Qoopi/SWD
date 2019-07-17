namespace SWD.Conditions
{
    public class Selected : AssertCondition
    {
        public override bool Invoke<T>(T element)
        {
            return element.Selected;
        }

        protected override string DescribeExpected()
        {
            return $"Selected {true}";
        }

        protected override string DescribeActual()
        {
            return $"Selected {false}";
        }
    }

    public static partial class Be
    {
        public static AssertCondition Selected => new Selected();
    }
}


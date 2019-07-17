namespace SWD.Conditions
{
    public class Text : AssertCondition
    {
        private readonly string _expectedText;
        private string _actualText;

        public Text(string expectedText)
        {
            _expectedText = expectedText;
        }

        public override bool Invoke<T>(T element)
        {
            _actualText = element.Text;
            return _expectedText == _actualText;
        }

        protected override string DescribeExpected()
        {
            return $"\'{_expectedText}\'";
        }

        protected override string DescribeActual()
        {
            return $"\'{_actualText}\'";
        }
    }

    public static partial class Have
    {
        public static AssertCondition Text(string expectedString)
        {
            return new Text(expectedString);
        }
    }
}
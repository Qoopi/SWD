using System.Linq;

namespace SWD.Conditions
{
    public class CSS : AssertCondition
    {
        private readonly string _expected;
        private string _actual;

        public CSS(string expected)
        {
            _expected = expected;
        }

        public override bool Invoke<T>(T element)
        {
            _actual = element.GetAttribute("class");
            return !string.IsNullOrEmpty(_actual) && _actual.Split(' ').Any(cssClass => cssClass.Equals(_expected));
        }

        protected override string DescribeExpected()
        {
            return $"Class = \'{_expected}\'";
        }

        protected override string DescribeActual()
        {
            return $"Class = \'{_actual}\'";
        }
    }

    public static partial class Have
    {
        public static AssertCondition CssClass(string cssClass)
        {
            return new CSS(cssClass);
        }
    }
}
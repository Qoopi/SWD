using SWD.WebElements;

namespace SWD.Conditions
{
    public abstract class AssertCondition
    {
        public abstract bool Invoke<T>(T element) where T : WebElement;

        protected abstract string DescribeExpected();

        protected abstract string DescribeActual();

        public override string ToString()
        {
            return $"\n{GetType().Name} => \nExpected: {DescribeExpected()} \nActual: {DescribeActual()}";
        }        
    }
}
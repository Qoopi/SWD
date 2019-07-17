using OpenQA.Selenium.Interactions;
using SWD.Driver;

namespace SWD.WebElements
{
    public abstract class Clickable : WebElement
    {
        private Actions _action = new Actions(SessionManager.WebDriver);

        protected Clickable Click(int timeout)
        {
            DoSafe(delegate { Element.Value.Click(); }, timeout);
            return this;
        }

        protected Clickable HoverAndClick(int timeout)
        {
            DoSafe(delegate { _action.MoveToElement(Element.Value).Click().Build().Perform(); }, timeout);
            return this;
        }
    }
}
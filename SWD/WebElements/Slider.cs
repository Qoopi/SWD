using OpenQA.Selenium.Interactions;
using SWD.Driver;

namespace SWD.WebElements
{
    public class Slider : WebElement
    {
        private Actions _action = new Actions(SessionManager.WebDriver);

        public new Slider Find(By by, string locator)
        {
            return (Slider) base.Find(by, locator);
        }

        public Slider DragNDrop(WebElement destinationElement, int timeout)
        {
            DoSafe(delegate { _action.ClickAndHold(Element.Value).MoveToElement(destinationElement.Element.Value).Release().Build().Perform(); }, timeout);
            return this;
        }
        
        public Slider DragNDrop(WebElement destinationElement)
        {
            DragNDrop(destinationElement, Settings.DefaultTimeout);
            return this;
        }
    }
}
namespace SWD.WebElements
{
    public class Button : Clickable
    {
        public new Button Find(By by, string locator)
        {
            return (Button) base.Find(by, locator);
        }

        public new Button Click(int timeout)
        {
            return (Button) base.Click(timeout);
        }

        public new Button Click()
        {
            return Click(Settings.DefaultTimeout);
        }

        public Button HoverAndClick(int timeout)
        {
            return (Button) base.HoverAndClick(timeout);
        }

        public Button HoverAndClick()
        {
            return HoverAndClick(Settings.DefaultTimeout);
        }
    }
}
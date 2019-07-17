namespace SWD.WebElements
{
    public class Input : WebElement
    {
        public new Input Find(By by, string locator)
        {
            return (Input) base.Find(by, locator);
        }

        public void SendKeys(string keysToSend, int timeout)
        {
            DoSafe(delegate { Element.Value.SendKeys(keysToSend); }, timeout);
        }

        public void SendKeys(string keysToSend)
        {
            SendKeys(keysToSend, Settings.DefaultTimeout);
        }

        public void ClearInput(int timeout)
        {
            DoSafe(delegate { Element.Value.Clear(); }, timeout);
        }

        public void ClearInput()
        {
            ClearInput(Settings.DefaultTimeout);
        }
    }
}
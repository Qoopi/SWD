using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SWD.WebElements
{
    public class DropDown : WebElement
    {
        private SelectElement _select;
        private IWebElement _element;


        public new DropDown Find(By by, string locator)
        {
            return (DropDown) base.Find(by, locator);
        }

        public void SelectByText(string text, int timeout)
        {
            _select = InitializeSelect();
            DoSafe(delegate { _select.SelectByText(text); }, timeout);
        }

        public void SelectByText(string text)
        {
            SelectByText(text, Settings.DefaultTimeout);
        }

        public void SelectByValue(string value, int timeout)
        {
            _select = InitializeSelect();
            DoSafe(delegate { _select.SelectByValue(value); }, timeout);
        }

        public void SelectByValue(string value)
        {
            SelectByValue(value, Settings.DefaultTimeout);
        }

        public string SelectedValue(int timeout)
        {
            _select = InitializeSelect();
            DoSafe(delegate { _element = _select.SelectedOption; }, timeout);
            return _element.Text;
        }

        public string SelectedValue()
        {
            return SelectedValue(Settings.DefaultTimeout);
        }

        private SelectElement InitializeSelect()
        {
            if (_select == null) return _select = DoSafe(() => new SelectElement(Element.Value));
            return _select;
        }
    }
}
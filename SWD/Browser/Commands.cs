using System.Linq;
using OpenQA.Selenium.Support.Extensions;
using static SWD.Driver.SessionManager;

namespace SWD.Browser
{
    public static class Commands
    {
        public static void OpenPage(string url)
        {
            WebDriver.Navigate().GoToUrl(url);
        }

        public static void RefreshPage()
        {
            WebDriver.Navigate().Refresh();
        }

        public static void SwitchTab(string tabUrl)
        {
            WebDriver.SwitchTo().Window(WebDriver.WindowHandles.Select(w => w.Equals(tabUrl)).ToString());
        }
        
        public static void SwitchTabWichContainsUrl(string tabUrl)
        {
            WebDriver.SwitchTo().Window(WebDriver.WindowHandles.Select(w => w.Contains(tabUrl)).ToString());
        }
        
        public static void CloseCurrentTab()
        {
            WebDriver.Close();
        }
        
        public static void AcceptAlert()
        {
            WebDriver.SwitchTo().Alert().Accept();
        }
        
        public static void DeclineAlert()
        {
            WebDriver.SwitchTo().Alert().Dismiss();
        }
        
        public static string GetAlertText()
        {
            return  WebDriver.SwitchTo().Alert().Text;
        }
        
        public static void SwitchToIFrame(string frameId)
        {
            WebDriver.SwitchTo().Frame(frameId);
        }
        
        public static void SwitchFromIFrameToMainPage()
        {
            WebDriver.SwitchTo().DefaultContent();
        }
        public static string GetJQueryValue(string jQueryLocator)
        {
            return  WebDriver.ExecuteJavaScript<string>($"return $('{jQueryLocator}').val()");
        }
    }
}
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using static SWD.Driver.DriverFactory;

namespace SWD.Driver
 {
     internal static class SessionManager
     {
         public static IWebDriver WebDriver {
             get
             {
                 if (DriverFactory.Driver?.Value == null || !DriverFactory.Driver.IsValueCreated)
                     GetDriver(true, new ChromeOptions());
                 
                 return DriverFactory.Driver.Value;
             }
         }

         public static void CloseSession()
         {
             DriverFactory.Driver?.Value.Quit();
             DriverFactory.Driver?.Value.Dispose();
         }
     }
 }
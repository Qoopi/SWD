using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace SWD.Driver
{
    internal static class DriverFactory
    {
        internal static   ThreadLocal<IWebDriver> Driver = new ThreadLocal<IWebDriver>();

        internal static IWebDriver GetDriver(bool isRemote, ChromeOptions options)
        {
            return Driver.Value = isRemote ? new RemoteWebDriver(new Uri("url"), options) : new ChromeDriver(options);
        }
    }
}
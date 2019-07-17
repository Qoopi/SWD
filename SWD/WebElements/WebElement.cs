using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SWD.Conditions;
using SWD.Driver;
using SWD.Utils;
using static SWD.Settings;
using static SWD.Utils.PollingHelper;
using static SWD.Utils.WaitHelper;

namespace SWD.WebElements
{
    public class WebElement
    {
        private By _by;
        private string _locator;
        internal Lazy<IWebElement> Element;
        public bool Displayed => DoSafe(() => Element.Value.Displayed);
        public bool Enabled => DoSafe(() => Element.Value.Enabled);
        public bool Selected => DoSafe(() => Element.Value.Selected);
        public bool Empty => DoSafe(() => Element.Value.Size.IsEmpty);
        public string GetTagName => DoSafe(() => Element.Value.TagName);
        public string Text => DoSafe(() => Element.Value.Text);

        public WebElement Find(By by, string locator)
        {
            _by = by;
            _locator = locator;

            switch (by)
            {
                case By.ClassName:
                    Element = new Lazy<IWebElement>(() =>
                            SessionManager.WebDriver.FindElement(OpenQA.Selenium.By.ClassName(locator)),
                        LazyThreadSafetyMode.PublicationOnly);
                    break;
                case By.CssSelector:
                    Element = new Lazy<IWebElement>(() =>
                            SessionManager.WebDriver.FindElement(OpenQA.Selenium.By.CssSelector(locator)),
                        LazyThreadSafetyMode.PublicationOnly);
                    break;
                case By.Id:
                    Element = new Lazy<IWebElement>(() =>
                            SessionManager.WebDriver.FindElement(OpenQA.Selenium.By.Id(locator)),
                        LazyThreadSafetyMode.PublicationOnly);
                    break;
                case By.LinkText:
                    Element = new Lazy<IWebElement>(() =>
                            SessionManager.WebDriver.FindElement(OpenQA.Selenium.By.LinkText(locator)),
                        LazyThreadSafetyMode.PublicationOnly);
                    break;
                case By.Name:
                    Element = new Lazy<IWebElement>(() =>
                            SessionManager.WebDriver.FindElement(OpenQA.Selenium.By.Name(locator)),
                        LazyThreadSafetyMode.PublicationOnly);
                    break;
                case By.PartialLinkText:
                    Element = new Lazy<IWebElement>(() =>
                            SessionManager.WebDriver.FindElement(OpenQA.Selenium.By.PartialLinkText(locator)),
                        LazyThreadSafetyMode.PublicationOnly);
                    break;
                case By.TagName:
                    Element = new Lazy<IWebElement>(
                        () => SessionManager.WebDriver.FindElement(OpenQA.Selenium.By.TagName(locator)),
                        LazyThreadSafetyMode.PublicationOnly);
                    break;
                case By.Xpath:
                    Element = new Lazy<IWebElement>(() =>
                            SessionManager.WebDriver.FindElement(OpenQA.Selenium.By.XPath(locator)),
                        LazyThreadSafetyMode.PublicationOnly);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(by), by, null);
            }

            return this;
        }

        public static Lazy<List<T>> FindElements<T>(By by, string locator, int timeout) where T : WebElement, new()
        {
            return new Lazy<List<T>>(() => GetElementsWithPolling<T>(by, locator, timeout),
                LazyThreadSafetyMode.PublicationOnly);
        }

        public static Lazy<List<T>> FindElements<T>(By by, string locator) where T : WebElement, new()
        {
            return FindElements<T>(by, locator, DefaultTimeout);
        }
        
        public bool Present()
        {
            try
            {
                switch (_by)
                {
                    case By.ClassName:
                        SessionManager.WebDriver.FindElement(OpenQA.Selenium.By.ClassName(_locator));
                        break;
                    case By.CssSelector:
                        SessionManager.WebDriver.FindElement(OpenQA.Selenium.By.CssSelector(_locator));
                        break;
                    case By.Id:
                        SessionManager.WebDriver.FindElement(OpenQA.Selenium.By.Id(_locator));
                        break;
                    case By.LinkText:
                        SessionManager.WebDriver.FindElement(OpenQA.Selenium.By.LinkText(_locator));
                        break;
                    case By.Name:
                        SessionManager.WebDriver.FindElement(OpenQA.Selenium.By.Name(_locator));
                        break;
                    case By.PartialLinkText:
                        SessionManager.WebDriver.FindElement(OpenQA.Selenium.By.PartialLinkText(_locator));
                        break;
                    case By.TagName:
                        SessionManager.WebDriver.FindElement(OpenQA.Selenium.By.TagName(_locator));
                        break;
                    case By.Xpath:
                        SessionManager.WebDriver.FindElement(OpenQA.Selenium.By.XPath(_locator));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(_by), _by, null);
                }

                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void MoveToElement()
        {
            var actions = new Actions(SessionManager.WebDriver);
            actions.MoveToElement(Element.Value);
            actions.Perform();
        }

        public string GetAttribute(string attributeName)
        {
            return DoSafe(() => Element.Value.GetAttribute(attributeName));
        }

        public void Should(AssertCondition condition, int timeout)
        {
            Wait(() => AssertCondition.Invoke(this),
                 $"Timed out after {timeout / 1000} seconds while PollingHelper.Waiting for AssertCondition: {condition}",
                timeout, Settings.WaitStrategy);
        }

        public void Should(AssertCondition condition)
        {
            Should(condition, DefaultTimeout);
        }

        public void ShouldNot(AssertCondition condition, int timeout)
        {
            Wait(() => !AssertCondition.Invoke(this),
                 $"Timed out after {timeout / 1000} seconds while PollingHelper.Waiting for AssertCondition Not fulfilled: {condition}",
                timeout, Settings.WaitStrategy);
        }

        public void ShouldNot(AssertCondition condition)
        {
            ShouldNot(condition, DefaultTimeout);
        }

        protected T DoSafe<T>(Func<T> action, int timeout)
        {
            var methodName = new StackFrame(1).GetMethod().Name;
            var message = $"Action {methodName} was failed with retry in {timeout / 1000} seconds. Error:\n";
            var error = string.Empty;
            var result = default(T);
            Wait(() =>
                {
                    try
                    {
                        result = WaitHelper.DoSafe(action, methodName, timeout);
                        error = string.Empty;
                        return true;
                    }
                    catch (StaleElementReferenceException e)
                    {
                        Element = null;
                        Find(_by, _locator);
                        error = e.ToString();
                        return false;
                    }
                },
                () => message + error, timeout, Settings.WaitStrategy);

            return result;
        }

        protected T DoSafe<T>(Func<T> action)
        {
            return DoSafe<T>(action, DefaultTimeout);
        }

        protected void DoSafe(Action action, int timeout)
        {
            var methodName = new StackFrame(1).GetMethod().Name;
            var message = $"Action {methodName} was failed with retry in {timeout / 1000} seconds. Error:\n";
            var error = String.Empty;

            Wait(() =>
                {
                    try
                    {
                        WaitHelper.DoSafe(action, methodName, timeout);
                        error = string.Empty;
                        return true;
                    }
                    catch (StaleElementReferenceException e)
                    {
                        Element = null;
                        Find(_by, _locator);
                        error = e.ToString();
                        return false;
                    }
                },
                () => message + error, timeout,  Settings.WaitStrategy);
        }

        protected void DoSafe(Action action)
        {
            DoSafe(action, DefaultTimeout);
        }

        private static List<T> GetElementsWithPolling<T>(By by, string locator, int timeout) where T : WebElement, new()
        {
            Wait(() => FindNonLazyElements<T>(by, locator).Count > 0,
                $"No one WebElement was found by {by} with locator {locator} is empty after {timeout / 1000} seconds",
                timeout,
                Settings.WaitStrategy);

            return FindNonLazyElements<T>(by, locator);
        }

        private static List<T> FindNonLazyElements<T>(By by, string locator) where T : WebElement, new()
        {
            List<IWebElement> iWebElements;
            switch (by)
            {
                case By.ClassName:
                    iWebElements = new List<IWebElement>(
                        SessionManager.WebDriver.FindElements(OpenQA.Selenium.By.ClassName(locator)));
                    break;
                case By.CssSelector:
                    iWebElements = new List<IWebElement>(
                        SessionManager.WebDriver.FindElements(OpenQA.Selenium.By.CssSelector(locator)));
                    break;
                case By.Id:
                    iWebElements = new List<IWebElement>(
                        SessionManager.WebDriver.FindElements(OpenQA.Selenium.By.Id(locator)));
                    break;
                case By.LinkText:
                    iWebElements = new List<IWebElement>(
                        SessionManager.WebDriver.FindElements(OpenQA.Selenium.By.LinkText(locator)));
                    break;
                case By.Name:
                    iWebElements = new List<IWebElement>(
                        SessionManager.WebDriver.FindElements(OpenQA.Selenium.By.Name(locator)));
                    break;
                case By.PartialLinkText:
                    iWebElements = new List<IWebElement>(
                        SessionManager.WebDriver.FindElements(OpenQA.Selenium.By.PartialLinkText(locator)));
                    break;
                case By.TagName:
                    iWebElements = new List<IWebElement>(
                        SessionManager.WebDriver.FindElements(OpenQA.Selenium.By.TagName(locator)));
                    break;
                case By.Xpath:
                    iWebElements = new List<IWebElement>(
                        SessionManager.WebDriver.FindElements(OpenQA.Selenium.By.XPath(locator)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(by), by, null);
            }

            var result = new List<T>();

            foreach (var iWebElement in iWebElements)
            {
                result.Add(new T {Element = new Lazy<IWebElement>(() => iWebElement)});
            }

            return result;
        }
    }
}
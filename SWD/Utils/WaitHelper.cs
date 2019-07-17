using System;
using OpenQA.Selenium;
using static SWD.Utils.PollingHelper;
namespace SWD.Utils
{
    public static class WaitHelper
    {
        public static void DoSafe(Action action, string actionName, int timeout)
        {
            var message = $"Action {actionName} was failed with retry in {timeout / 1000} seconds. Error:\n";
            var error = string.Empty;
            Wait(() => TryDoAction(action, out error), () => message + error, timeout, WaitStrategy.Progressive);
        }

        public static T DoSafe<T>(Func<T> action, string actionName, int timeout)
        {
            var message = $"Action {actionName} was failed with retry in {timeout / 1000} seconds. Error:\n";
            var error = string.Empty;
            var result = default(T);
            Wait(() => TryDoAction(action, out result, out error), () => message + error, timeout, WaitStrategy.Progressive);
            return result;
        }

        public static bool TryDoAction(Action action, out string exceptionMessage)
        {
            try
            {
                action.Invoke();
                exceptionMessage = string.Empty;
                return true;
            }
            catch (InvalidOperationException e)
            {
                exceptionMessage = e.ToString();
                return false;
            }
            catch (StaleElementReferenceException)
            {
                throw;
            }
            catch (WebDriverException e)
            {
                exceptionMessage = e.ToString();
                return false;
            }
        }

        public static bool TryDoAction<T>(Func<T> action, out T result, out string exceptionMessage)
        {
            try
            {
                result = action.Invoke();
                exceptionMessage = string.Empty;
                return true;
            }
            catch (InvalidOperationException e)
            {
                exceptionMessage = e.ToString();
                result = default(T);
                return false;
            }
            catch (StaleElementReferenceException)
            {
                throw;
            }
            catch (WebDriverException e)
            {
                exceptionMessage = e.ToString();
                result = default(T);
                return false;
            }
        }
    }
}
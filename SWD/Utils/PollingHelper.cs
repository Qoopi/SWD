using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;

namespace SWD.Utils
{
    public enum WaitStrategy
    {
        Progressive,
        Сhangeless
    }
    public static class PollingHelper
    {
           public static void Wait(Func<bool> condition, string errorMessage, int waitTimeout, WaitStrategy strategy)
        {
            if (!TryToWait(condition, waitTimeout, strategy))
            {
                throw new AssertionException(errorMessage);
            }
        }

        public static void Wait(Func<bool> condition, Func<string> errorMessage, int waitTimeout, WaitStrategy strategy)
        {
            if (!TryToWait(condition, waitTimeout, strategy))
            {
                throw new AssertionException(errorMessage.Invoke());
            }
        }
        
        public static void Wait(Func<bool> conditionSuccess, Func<bool> conditionFailure, Func<string> errorMessageTimeout, Func<string> errorMessageFailure, int waitTimeout, WaitStrategy strategy)
        {
            var result = TryToWait(conditionSuccess, conditionFailure, waitTimeout, strategy, out var isFailure);

            if (result) return;
            if (isFailure)
            {
                throw new AssertionException(errorMessageFailure.Invoke());
            }

            throw new AssertionException(errorMessageTimeout.Invoke());
        }

        public static bool TryToWait(Func<bool> condition, int waitTimeout, WaitStrategy strategy)
        {
            var isSuccess = false;
            var alreadyWaited = 0;
            var timeToWait = strategy == WaitStrategy.Progressive ? 0 : 500;
            var stopwatch = new Stopwatch();

            while (true)
            {
                stopwatch.Reset();
                stopwatch.Start();
                var result = condition.Invoke();
                if (result)
                {
                    isSuccess = true;
                    break;
                }

                stopwatch.Stop();

                alreadyWaited += stopwatch.Elapsed.Milliseconds;

                if (alreadyWaited >= waitTimeout)
                    break;

                if (strategy == WaitStrategy.Progressive)
                {
                    if (timeToWait == 0) timeToWait += 100;
                    else timeToWait *= 2;
                }

                Thread.Sleep(timeToWait);

                alreadyWaited += timeToWait;
            }

            return isSuccess;
        }
        
        private static bool TryToWait(Func<bool> conditionSuccess, Func<bool> conditionFailure, int waitTimeout, WaitStrategy strategy, out bool isFaliure)
        {
            isFaliure = false;
            var isSuccess = false;
            var alreadyWaited = 0;
            var timeToWait = strategy == WaitStrategy.Progressive ? 0 : 500;
            var stopwatch = new Stopwatch();

            while (true)
            {
                stopwatch.Reset();
                stopwatch.Start();
                
                if (conditionSuccess.Invoke())
                {
                    isSuccess = true;
                    break;
                }

                if (conditionFailure.Invoke())
                {
                    isFaliure = true;
                    break;
                }

                stopwatch.Stop();

                alreadyWaited += stopwatch.Elapsed.Milliseconds;

                if (alreadyWaited >= waitTimeout)
                    break;

                if (strategy == WaitStrategy.Progressive)
                {
                    if (timeToWait == 0) timeToWait += 100;
                    else timeToWait *= 2;
                }

                Thread.Sleep(timeToWait);

                alreadyWaited += timeToWait;
            }

            return isSuccess;
        }
    }
}
namespace Pulse.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public static class AsyncHelper
    {
        private static readonly TaskFactory _myTaskFactory = new
          TaskFactory(CancellationToken.None,
                      TaskCreationOptions.None,
                      TaskContinuationOptions.None,
                      TaskScheduler.Default);

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncHelper._myTaskFactory
              .StartNew<Task<TResult>>(func)
              .Unwrap<TResult>()
              .GetAwaiter()
              .GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
            AsyncHelper._myTaskFactory
              .StartNew<Task>(func)
              .Unwrap()
              .GetAwaiter()
              .GetResult();
        }

        public static Task<TResult> RunAsync<TResult>(Func<TResult> callBack)
        {
            return Task<TResult>.Factory.StartNew(callBack);
        }

        public static Task<IEnumerable<TResult>> RunAsync<TResult>(Func<IEnumerable<TResult>> callBack)
        {
            return Task<IEnumerable<TResult>>.Factory.StartNew(callBack);
        }
    }
}

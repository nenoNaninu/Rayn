using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Rayn.Services.Threading
{
    public static class TaskExtensions
    {
        public static async void Forget(this Task task, ILogger logger)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                logger?.LogError(e, e.Message);
            }
        }
        public static async void Forget(this ValueTask task, ILogger logger)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                logger?.LogError(e, e.Message);
            }
        }
    }
}
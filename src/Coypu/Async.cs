using System;
using System.Threading.Tasks;

namespace Coypu
{
  /// <summary>
  /// Provides asynchronous utility methods.
  /// </summary>
  public static class Async
  {
    /// <summary>
    /// Waits for the result of the specified asynchronous function.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="task">The asynchronous task.</param>
    /// <returns>The result of the asynchronous task.</returns>
    public static T WaitForResult<T>(Task<T> task)
    {
        task.Wait();
        return task.Result;
    }

    /// <summary>
    /// Waits for the specified asynchronous function to complete.
    /// </summary>
    /// <param name="task">The asynchronous task.</param>
    /// <returns>The result of the asynchronous task.</returns>
    public static void WaitForResult(Task task)
    {
      task.Wait();
    }
  }
}

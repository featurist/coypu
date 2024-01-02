using System.Threading.Tasks;

namespace Coypu
{
    /// <summary>
    /// Provides extension methods for working with asynchronous tasks.
    /// </summary>
    public static class AsyncExtensions
    {
      /// <summary>
      /// Waits for the task to complete and returns the result.
      /// </summary>
      /// <typeparam name="T">The type of the task result.</typeparam>
      /// <param name="task">The task to wait for.</param>
      /// <returns>The result of the completed task.</returns>
      public static T Sync<T>(this Task<T> task)
      {
        task.Wait();
        return task.Result;
      }

      /// <summary>
      /// Waits for the task to complete.
      /// </summary>
      /// <param name="task">The task to wait for.</param>
      public static void Sync(this Task task)
      {
        task.Wait();
      }
    }
}

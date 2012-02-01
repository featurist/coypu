namespace Coypu.Queries
{
    internal interface Query<out TReturn>
    {
        void Run();
        TReturn ExpectingResult { get; }
        TReturn Result { get; }
    }
}
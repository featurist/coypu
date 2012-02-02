namespace Coypu.Queries
{
    internal interface Query<out TReturn>
    {
        void Run();
        TReturn ExpectedResult { get; }
        TReturn Result { get; }
    }
}
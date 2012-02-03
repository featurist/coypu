namespace Coypu.Queries
{
    public interface Query<out TReturn>
    {
        void Run();
        TReturn ExpectedResult { get; }
        TReturn Result { get; }
    }
}
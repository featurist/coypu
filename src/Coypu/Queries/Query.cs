namespace Coypu.Queries
{
    public interface Query<out TReturn>
    {
        TReturn Run();
        TReturn ExpectedResult { get; }
        Options Options { get; }
        DriverScope Scope { get; }    
    }
}
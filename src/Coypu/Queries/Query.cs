namespace Coypu.Queries
{
    public interface Query<out TReturn>
    {
        TReturn Run();
        object ExpectedResult { get; }
        Options Options { get; }
        DriverScope Scope { get; }    
    }
}
namespace DynamicCompiler
{
    public interface ICompiler
    {
        Task CompileAsync(StreamReader reader, StreamWriter writer);
    }
}

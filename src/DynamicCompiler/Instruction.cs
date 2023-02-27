namespace DynamicCompiler
{
    public class Instruction
    {
        public string Syntax { get; set; } = null!;

        public string Template { get; set; } = null!;

        public Dictionary<string, Symbol> Symbols { get; set; } = new();
    }
}

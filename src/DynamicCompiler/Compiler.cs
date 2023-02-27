namespace DynamicCompiler
{
    public class Compiler : ICompiler
    {
        private readonly IReadOnlyDictionary<string, Instruction> _instructions;

        public Compiler(IEnumerable<Instruction> instructions)
        {
            _instructions = instructions.ToDictionary(i => GetSections(i.Syntax).First());
        }

        public async Task CompileAsync(StreamReader reader, StreamWriter writer)
        {
            while (!reader.EndOfStream)
            {
                string? line = await reader.ReadLineAsync();

                Compile(line!);


            }
        }

        public string Compile(string line)
        {
            string[] lineSymbols = GetSections(line);

            if (!_instructions.TryGetValue(lineSymbols[0], out Instruction instruction))
            {
                throw new KeyNotFoundException(lineSymbols[0]);
            }

            string[] syntaxSymbols = GetSections(instruction.Syntax);

            if (syntaxSymbols.Length != lineSymbols.Length)
            {
                throw new ArgumentException();
            }

            Dictionary<string, string> compiledSymbols = new Dictionary<string, string>();

            for (int i = 0; i < lineSymbols.Length; i++)
            {
                string syntaxSymbol = syntaxSymbols[i];
                string lineSymbol = lineSymbols[i];

                if (!syntaxSymbol.StartsWith('{') && !syntaxSymbol.EndsWith('}'))
                {
                    continue;
                }
                
                string compiledSymbol = CompileSymbol(lineSymbol, syntaxSymbol, instruction);

                compiledSymbols.Add(syntaxSymbol, compiledSymbol);
            }

            return CompileInstruction(instruction, compiledSymbols);
        }

        private string CompileSymbol(string lineSymbol, string syntaxSymbol, Instruction instruction)
        {
            string symbolKey = GetSymbolKey(syntaxSymbol);

            if (!instruction.Symbols.TryGetValue(symbolKey, out Symbol symbol))
            {
                throw new KeyNotFoundException();
            }

            if (symbol.Rules.Count == 0)
            {
                if (!int.TryParse(lineSymbol, out int lineValue))
                {
                    throw new ArgumentException();
                }

                string symbolBinary = ToBinary(lineValue, symbol.BitWidth);

                return symbolBinary;
            }
            else
            {
                if (!symbol.Rules.TryGetValue(lineSymbol, out string lineValue))
                {
                    throw new KeyNotFoundException();
                }

                return lineValue;
            }
        }

        private static string CompileInstruction(Instruction instruction, Dictionary<string, string> compiledSymbols)
        {
            string compiledInstruction = instruction.Template;

            foreach ((string symbol, string value) in compiledSymbols)
            {
                compiledInstruction = compiledInstruction.Replace(symbol, value);
            }

            return compiledInstruction;
        }

        private static string GetSymbolKey(string value)
            => value.Substring(1, value.Length - 2);

        private static string[] GetSections(string value)
            => value.Split(' ');

        private static string ToBinary(int value, int bitWidth)
        {
            string binary = Convert.ToString(value, 2);

            if (binary.Length > bitWidth)
            {
                throw new OverflowException();
            }

            return binary.PadLeft(bitWidth, '0');
        }
    }
}

using Shouldly;
using Xunit;

namespace DynamicCompiler.Tests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("ADDI 57 A", "0111001000000011")]
        [InlineData("ADDI 57 B", "0111001000010011")]
        [InlineData("ADDI 57 C", "0111001000100011")]
        [InlineData("ADDI 57 I", "0111001001110011")]
        [InlineData("ADDI 0 I", "0000000001110011")]
        [InlineData("ADDI 0 A", "0000000000000011")]
        [InlineData("ADDI 5 A", "0000101000000011")]
        public void Test1(string lineValue, string expectedValue)
        {
            Instruction instruction = new Instruction
            {
                Syntax = "ADDI {A} {B}",
                Template = "{A}00{B}0011",
                Symbols = new Dictionary<string, Symbol>
                {
                    { "A", new Symbol
                    {
                        BitWidth = 7

                    }},
                    { "B", new Symbol
                    {
                        BitWidth = 7,
                        Rules = new Dictionary<string, string>
                        {
                            { "A", "000" },
                            { "B", "001" },
                            { "C", "010" },
                            { "D", "011" },
                            { "E", "100" },
                            { "F", "101" },
                            { "H", "110" },
                            { "I", "111" },

                        }
                    }}
                }
            };

            Compiler compiler = new Compiler(new[] { instruction });

            compiler.Compile(lineValue).ShouldBe(expectedValue);
        }
    }
}
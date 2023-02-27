using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicCompiler
{
    public class Symbol
    {
        public int BitWidth { get; set; }

        public Dictionary<string, string> Rules { get; set; } = new();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MathEngine
{
    public record Token(TokenType Type, string Value)
    {
        public override string ToString() => $"[{Type}: '{Value}']";
    }
}

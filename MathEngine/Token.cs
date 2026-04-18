namespace MathEngine
{
    public record Token(TokenType Type, string Value)
    {
        public override string ToString() => $"[{Type}: '{Value}']";
    }
}

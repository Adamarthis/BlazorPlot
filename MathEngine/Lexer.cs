using System.Text;

namespace MathEngine.Lexing
{
    public class Lexer
    {
        private readonly string _input;
        private int _position;

        public Lexer(string input)
        {
            _input = input.Replace(" ", "").ToLowerInvariant();
            _position = 0;
        }

        private char Current => _position < _input.Length ? _input[_position] : '\0';

        private void Advance() => _position++;

        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();

            while (_position < _input.Length)
            {
                char c = Current;
                if (char.IsDigit(c) || c == '.')
                {
                    tokens.Add(new Token(TokenType.Number, ReadNumber()));
                }
                else if (char.IsLetter(c))
                {
                    tokens.Add(ReadIdentifier());
                }
                else if ("+-*/^".Contains(c))
                {
                    tokens.Add(new Token(TokenType.Operator, c.ToString()));
                    Advance();
                }
                else if (c == '(')
                {
                    tokens.Add(new Token(TokenType.LParenthesis, c.ToString()));
                    Advance();

                }
                else if (c == ')')
                {
                    tokens.Add(new Token(TokenType.RParenthesis, c.ToString()));
                    Advance();
                }
                else
                {
                    throw new Exception($"Unexpected character: {c}");
                }
            }
            tokens.Add(new Token(TokenType.EOF, string.Empty));
            return tokens;
        }

        private string ReadNumber()
        {
            var sb = new StringBuilder();
            while (char.IsDigit(Current) || Current == '.')
            {
                sb.Append(Current);
                Advance();
            }
            return sb.ToString();
        }

        private Token ReadIdentifier()
        {
            var sb = new StringBuilder();
            while (char.IsLetter(Current))
            {
                sb.Append(Current);
                Advance();
            }

            string identifier = sb.ToString();

            if (identifier == "x" || identifier == "y")
            {
                return new Token(TokenType.Variable, identifier);
            }
            
            return new Token(TokenType.Function, identifier);
        }

    }
}

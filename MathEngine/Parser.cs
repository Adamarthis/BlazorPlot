using System.Globalization;
using MathEngine.Configuration;
using MathEngine.Expressions;
using MathEngine.Lexing;

namespace MathEngine
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _position;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _position = 0;
        }

        private Token Current => _position < _tokens.Count ? _tokens[_position] : new Token(TokenType.EOF, "");
        private void Advance() => _position++;

        public INode Parse()
        {
            var nodes = new Stack<INode>();
            var operators = new Stack<Token>();

            while (Current.Type != TokenType.EOF)
            {
                if (Current.Type == TokenType.Number)
                {
                    double value = double.Parse(Current.Value, CultureInfo.InvariantCulture);
                    nodes.Push(new NumberNode(value));
                    Advance();
                }
                else if (Current.Type == TokenType.Function)
                {
                    if (!FunctionRegistry.isFunction(Current.Value))
                    {
                        throw new Exception($"Function '{Current.Value}' is not supported");
                    }

                    operators.Push(Current);
                    Advance();
                }
                else if (Current.Type == TokenType.Variable)
                {
                    nodes.Push(new VariableNode());
                    Advance();
                }
                else if (Current.Type == TokenType.Operator)
                {
                    while (operators.Count > 0 && operators.Peek().Type == TokenType.Operator && OperatorRegistry.GetPrecedence(operators.Peek().Value) >= OperatorRegistry.GetPrecedence(Current.Value))
                    {
                        ProcessOperator(nodes, operators.Pop());
                    }

                    operators.Push(Current);
                    Advance();
                }
                else if (Current.Type == TokenType.LParenthesis)
                {
                    operators.Push(Current);
                    Advance();
                }
                else if (Current.Type == TokenType.RParenthesis)
                {
                    while (operators.Count > 0 && operators.Peek().Type != TokenType.LParenthesis)
                    {
                        ProcessOperator(nodes, operators.Pop());
                    }

                    if (operators.Count > 0 && operators.Peek().Type == TokenType.LParenthesis)
                    {
                        operators.Pop();
                    }

                    if (operators.Count > 0 && operators.Peek().Type == TokenType.Function)
                    {
                        var funcToken = operators.Pop();
                        var arg = nodes.Pop();
                        nodes.Push(new FunctionNode(funcToken.Value, arg));
                    }
                    Advance();
                }
            }

            while (operators.Count > 0)
            {
                ProcessOperator(nodes, operators.Pop());
            }

            return nodes.Pop();
        }

        private void ProcessOperator(Stack<INode> nodes, Token opToken)
        {
            var right = nodes.Pop();
            var left = nodes.Pop();

            nodes.Push(new BinaryNode(left, right, opToken.Value));
        }
    }
}

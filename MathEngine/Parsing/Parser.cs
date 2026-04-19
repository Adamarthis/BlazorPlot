using System.Globalization;
using System.Xml;
using MathEngine.Configuration;
using MathEngine.Expressions;
using MathEngine.Lexing;

namespace MathEngine.Parsing
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
                    if (!FunctionRegistry.IsFunction(Current.Value))
                    {
                        throw new Exception($"Function '{Current.Value}' is not supported");
                    }

                    operators.Push(Current);
                    Advance();
                }
                else if (Current.Type == TokenType.Variable)
                {
                    nodes.Push(new VariableNode(Current.Value));
                    Advance();
                }
                else if (Current.Type == TokenType.Operator)
                {
                    var currentToken = Current;
                    
                    bool isUnary = Current.Value == "-" && (_position == 0 || _tokens[_position - 1].Type == TokenType.LParenthesis || _tokens[_position - 1].Type == TokenType.Operator);
                    
                    if (isUnary)
                    {
                        nodes.Push(new NumberNode(0));
                        currentToken = new Token(TokenType.Operator, "~");
                    }

                    while (operators.Count > 0 && operators.Peek().Type == TokenType.Operator && OperatorRegistry.GetPrecedence(operators.Peek().Value) >= OperatorRegistry.GetPrecedence(currentToken.Value))
                    {
                        ProcessOperator(nodes, operators.Pop());
                    }

                    operators.Push(currentToken);
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
                        if (nodes.Count == 0)
                        {
                            throw new Exception($"Function {funcToken.Value} is expecting an argument");
                        }
                        
                        var arg = nodes.Pop();
                        nodes.Push(new FunctionNode(funcToken.Value, arg));
                    }
                    Advance();
                }
            }

            while (operators.Count > 0)
            {
                var op = operators.Pop();

                if (op.Type == TokenType.LParenthesis)
                {
                    throw new Exception("Mismatched parentheses: Missing ')'");
                }
                
                ProcessOperator(nodes, op);
            }

            if (nodes.Count == 0)
            {
                throw new Exception("Empty expression");
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

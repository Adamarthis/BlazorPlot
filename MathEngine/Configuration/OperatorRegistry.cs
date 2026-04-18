using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace MathEngine.Configuration
{
    public class OperatorDef
    {
        public string Symbol { get;  init; }
        public int Precedence { get; init; }

        public Func<Expression, Expression, Expression> Compiler { get; init; }
    }

    public class OperatorRegistry
    {
        private readonly static Dictionary<string, OperatorDef> _operators = new();

        static OperatorRegistry()
        {
            Register("+", 1, Expression.Add);
            Register("-", 1, Expression.Subtract);
            Register("*", 2, Expression.Multiply);
            Register("/", 2, Expression.Divide);
            Register("^", 1, Expression.Power);
        }

        public static void Register(string symbol, int precendence, Func<Expression, Expression, Expression> compiler)
        {
            _operators[symbol] = new OperatorDef
            {
                Symbol = symbol,
                Precedence = precendence,
                Compiler = compiler
            };
        }

        public static bool IsOperator(string symbol) => _operators.ContainsKey(symbol);

        public static bool IsOperatorChar(char c) => _operators.Keys.Any(k => k.StartsWith(c));

        public static int GetPrecedence(string  symbol)
        {
            return _operators.TryGetValue(symbol, out var def) ? def.Precedence : 0;
        }
        public static Expression Compile(string symbol, Expression left, Expression right)
        {
            if (_operators.TryGetValue(symbol, out var def))
            {
                return def.Compiler(left, right);
            }

            throw new InvalidOperationException($"Unknown operator: {symbol}");
        }
    }
}

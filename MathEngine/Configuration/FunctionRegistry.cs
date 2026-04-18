using System.Linq.Expressions;
using System.Reflection;

namespace MathEngine.Configuration
{
    public static class FunctionRegistry
    {
        private static readonly Dictionary<string, MethodInfo> _functions = new();

        static FunctionRegistry()
        {
            Register("sin");
            Register("cos");
            Register("tan");
            Register("sqrt");
            Register("abs");
            Register("log");
            Register("exp");
        }

        public static void Register(string functionName, Type? sourceType = null)
        {
            sourceType ??= typeof(Math);

            var method = sourceType.GetMethod(functionName,BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static, null, new[] {typeof(double)}, null);

            if (method == null)
            {
                throw new ArgumentException($"Method {functionName}(double) does not exist.");
            }

            _functions[functionName.ToLowerInvariant()] = method;
        }

        public static bool IsFunction(string name) => _functions.ContainsKey(name.ToLowerInvariant());

        public static Expression Compile(string name, Expression argument) 
        {
            var lowerName = name.ToLowerInvariant();

            if (_functions.TryGetValue(lowerName, out var method))
            {
                return Expression.Call(method, argument);
            }

            throw new InvalidOperationException($"Unknown function: {name}");
        }
    }
}

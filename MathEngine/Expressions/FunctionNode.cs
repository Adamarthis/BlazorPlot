using System.Linq.Expressions;
using MathEngine.Configuration;

namespace MathEngine.Expressions
{
    public class FunctionNode(string functionName, INode argument) : INode
    {
        public Expression ToLinqExpression(ParameterExpression xParam)
        {
            var argExpr = argument.ToLinqExpression(xParam);

            return FunctionRegistry.Compile(functionName, argExpr);
        }
    }
}
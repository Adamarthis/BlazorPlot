using System.Linq.Expressions;
using MathEngine.Configuration;

namespace MathEngine.Expressions
{
    public class FunctionNode(string functionName, INode argument) : INode
    {
        public Expression ToLinqExpression(ParameterExpression xParam, ParameterExpression yParam, ParameterExpression dictParam)
        {
            var argExpr = argument.ToLinqExpression(xParam, yParam, dictParam);

            return FunctionRegistry.Compile(functionName, argument.ToLinqExpression(xParam, yParam, dictParam));
        }
    }
}
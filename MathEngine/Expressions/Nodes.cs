using MathEngine.Configuration;
using System.Linq.Expressions;

namespace MathEngine.Expressions
{
    public class NumberNode(double value) : INode
    {
        public Expression ToLinqExpression(ParameterExpression xParam, ParameterExpression yParam) => Expression.Constant(value);
    }

    public class VariableNode(string name) : INode
    {
        public Expression ToLinqExpression(ParameterExpression xParam, ParameterExpression yParam)
        {
            return name.ToLowerInvariant() == "y" ? yParam : xParam;
        }
    }

    public class  BinaryNode(INode left, INode right, string op) : INode
    {
        public Expression ToLinqExpression(ParameterExpression xParam, ParameterExpression yParam)
        {
            return OperatorRegistry.Compile(op, left.ToLinqExpression(xParam, yParam), right.ToLinqExpression(xParam, yParam));
        }
    }
}

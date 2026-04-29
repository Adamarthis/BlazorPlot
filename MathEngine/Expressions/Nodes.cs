using MathEngine.Configuration;
using System.Linq.Expressions;
using System.Net.Quic;

namespace MathEngine.Expressions
{
    public class NumberNode(double value) : INode
    {
        public Expression ToLinqExpression(ParameterExpression xParam, ParameterExpression yParam, ParameterExpression dictParam) => Expression.Constant(value);
    }

    public class VariableNode(string name) : INode
    {
        public Expression ToLinqExpression(ParameterExpression xParam, ParameterExpression yParam, ParameterExpression dictParam)
        {
            if (name.ToLowerInvariant()=="x")
            {
                return xParam;
            }
            if (name.ToLowerInvariant()=="y")
            {
                return yParam;
            }

            var getItemMethod = typeof(Dictionary<string, double>).GetMethod("get_Item")!;
            return Expression.Call(dictParam, getItemMethod, Expression.Constant(name));
        }
    }

    public class  BinaryNode(INode left, INode right, string op) : INode
    {
        public Expression ToLinqExpression(ParameterExpression xParam, ParameterExpression yParam, ParameterExpression dictParam)
        {
            return OperatorRegistry.Compile(op, left.ToLinqExpression(xParam, yParam, dictParam), right.ToLinqExpression(xParam, yParam, dictParam));
        }
    }
}

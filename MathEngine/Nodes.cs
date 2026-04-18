using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MathEngine.Expressions
{
    public class NumberNode(double value) : INode
    {
        public Expression ToLinqExpression(ParameterExpression xParam) => Expression.Constant(value);
    }

    public class VariableNode : INode
    {
        public Expression ToLinqExpression(ParameterExpression xParam) => xParam;
    }

    public class  BinaryNode(INode left, INode right, string op) : INode
    {
        public Expression ToLinqExpression(ParameterExpression xParam)
        {
            var leftExpr = left.ToLinqExpression(xParam);
            var rightExpr = right.ToLinqExpression(xParam);

            return op switch
            {
                "+" => Expression.Add(leftExpr, rightExpr),
                "-" => Expression.Subtract(leftExpr, rightExpr),
                "*" => Expression.Multiply(leftExpr, rightExpr),
                "/" => Expression.Divide(leftExpr, rightExpr),
                "^" => Expression.Power(leftExpr, rightExpr),
                _ => throw new Exception($"Unsupported operator: {op}")
            };
        }
        
    }
}

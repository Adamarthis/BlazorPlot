using System.Linq.Expressions;

namespace MathEngine.Expressions
{
    public interface INode
    {
        Expression ToLinqExpression(ParameterExpression xParam, ParameterExpression yParam);
    }
}

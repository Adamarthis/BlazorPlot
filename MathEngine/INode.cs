using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MathEngine
{
    public interface INode
    {
        Expression ToLinqExpression(ParameterExpression xParam);
    }
}

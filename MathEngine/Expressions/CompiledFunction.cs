using System.Linq.Expressions;

namespace MathEngine.Expressions
{
    public class CompiledFunction
    {
        public Func<double, double, double> Function { get; }

        public CompiledFunction(INode rootNode)
        {
            var xParam = Expression.Parameter(typeof(double), "x");
            var yParam = Expression.Parameter(typeof(double), "y");

            var body = rootNode.ToLinqExpression(xParam, yParam);
            var lambda = Expression.Lambda<Func<double, double, double>>(body, xParam, yParam);
            Function = lambda.Compile();
        }
        
        public double Evaluate(double x, double y = 0) => Function(x, y);
    }
}

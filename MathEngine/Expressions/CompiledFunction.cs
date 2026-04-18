using System.Linq.Expressions;

namespace MathEngine.Expressions
{
    public class CompiledFunction
    {
        public Func<double, double> Function { get; }

        public CompiledFunction(INode rootNode)
        {
            var XParam = Expression.Parameter(typeof(double), "x");
            var body = rootNode.ToLinqExpression(XParam);
            var lambda = Expression.Lambda<Func<double, double>>(body, XParam);
            Function = lambda.Compile();
        }
        
        public double Evaluate(double x) => Function(x);
    }
}

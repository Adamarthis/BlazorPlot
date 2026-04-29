using System.Linq.Expressions;

namespace MathEngine.Expressions
{
    public class CompiledFunction
    {
        public Func<double, double, Dictionary<string, double>, double> Function { get; }

        public CompiledFunction(INode rootNode)
        {
            var xParam = Expression.Parameter(typeof(double), "x");
            var yParam = Expression.Parameter(typeof(double), "y");
            var dictParam = Expression.Parameter(typeof(Dictionary<string, double>), "p");

            var body = rootNode.ToLinqExpression(xParam, yParam, dictParam);
            var lambda = Expression.Lambda<Func<double, double, Dictionary<string, double>, double>>(body, xParam, yParam, dictParam);
            Function = lambda.Compile();
        }

        public double Evaluate(double x, double y = 0, Dictionary<string, double>? parameters = null)
        {
            return Function(x, y, parameters ?? new Dictionary<string, double>());
        }
    }
}

using MathEngine.Expressions;
using System.Runtime.InteropServices;

namespace BlazorPlot.Web.Models
{
    public class Equation
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string ExpressionText { get; set; } = string.Empty;
        public CompiledFunction? Function { get; set; }
        public string Color { get; set; } = "#0078D7";
        public bool IsVisible { get; set; } = true;
        public bool HasError { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
        public bool IsImplicit { get; set; } = false;
        public bool IsPoint { get; set; } = false;
        public (double X, double Y)? PointCoordinates { get; set; }
        public Dictionary<string, double> Parameters { get; set; } = new();

        public double Evaluate(double x, double y = 0)
        {
            if (Function == null) return double.NaN;
            return Function.Evaluate(x, y, Parameters);
        }
    }
}

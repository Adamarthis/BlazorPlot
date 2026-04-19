using MathEngine;
using MathEngine.Lexing;
using MathEngine.Expressions;
using BlazorPlot.Web.Models;
using MathEngine.Parsing;

namespace BlazorPlot.Web.Services
{
    public class CalculatorState
    {
        public List<Equation> Equations { get; private set; } = new();
        public event Action? OnChange;

        private readonly string[] _colors = { "#FF3B30", "#007AFF", "#34C759", "#FF9500", "#AF52DE" };

        public void AddEquation()
        {
            Equations.Add(new Equation { ExpressionText = "", Color = _colors[Equations.Count % _colors.Length] });
            NotifyStateChanged();
        }

        public void UpdateEquationText(Guid id, string newText)
        {
            var eq = Equations.FirstOrDefault(e => e.Id == id);
            if (eq == null)
            {
                return;
            }
            eq.ExpressionText = newText;
            if (string.IsNullOrWhiteSpace(newText))
            {
                eq.Function = null;
                eq.HasError  = false;
                eq.ErrorMessage = "";
            }
            else
            {
                try
                {
                    var lexer = new Lexer(newText);
                    var tokens = lexer.Tokenize();
                    var parser = new Parser(tokens);
                    var rootNode = parser.Parse();

                    eq.Function = new CompiledFunction(rootNode);
                    eq.HasError = false;
                    eq.ErrorMessage = "";
                }
                catch (Exception ex)
                {
                    eq.Function = null;
                    eq.HasError = true;
                    eq.ErrorMessage = ex.Message;
                }
            }
            NotifyStateChanged();
        }

        public void RemoveEquation(Guid id)
        {
            Equations.RemoveAll(e => e.Id == id);
            NotifyStateChanged();
        }
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}

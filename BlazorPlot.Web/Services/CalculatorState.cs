using MathEngine;
using MathEngine.Lexing;
using MathEngine.Expressions;
using BlazorPlot.Web.Models;
using MathEngine.Parsing;
using System.Xml;

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
            if (eq == null) return;

            eq.ExpressionText = newText;

            eq.IsImplicit = false;
            eq.IsPoint = false;
            eq.PointCoordinates = null;
            eq.Function = null;

            if (string.IsNullOrWhiteSpace(newText))
            {
                eq.HasError = false;
                eq.ErrorMessage = "";
                eq.Parameters.Clear();
                NotifyStateChanged();
                return;
            }
            try
            {
                string mathText = newText.Trim().ToLowerInvariant();
                
                if (mathText.StartsWith("(") && mathText.EndsWith(")") && mathText.Contains(","))
                {
                    var inner = mathText.Substring(1, mathText.Length - 2);
                    var parts = inner.Split(',');

                    if (parts.Length == 2)
                    {
                        // make it more unreadable later
                        var funcX = new CompiledFunction(new Parser(new Lexer(parts[0]).Tokenize()).Parse());
                        var funcY = new CompiledFunction(new Parser(new Lexer(parts[1]).Tokenize()).Parse());

                        eq.PointCoordinates = (funcX.Evaluate(0), funcY.Evaluate(0));
                        eq.IsPoint = true;
                    }
                    else throw new Exception("Expected (X, Y) point format.");
                }
                else
                {
                    if (mathText.StartsWith("y=") || mathText.StartsWith("y ="))
                    {
                        mathText = mathText.Substring(mathText.IndexOf('=') + 1).Trim();
                    }
                    else if (mathText.Contains("="))
                    {
                        var parts = mathText.Split('=');
                        if (parts.Length == 2)
                        {
                            mathText = $"{parts[0]} - ({parts[1]})";
                            eq.IsImplicit = true;
                        }
                        else throw new Exception("Too many '='");
                    }

                    var tokens = new Lexer(mathText).Tokenize();
                    var extractedParams = tokens
                        .Where(t => t.Type == TokenType.Variable && t.Value != "x" && t.Value != "y")
                        .Select(t => t.Value)
                        .Distinct()
                        .ToList();

                    var newParams = new Dictionary<string, double>();
                    foreach (var param in extractedParams)
                    {
                        newParams[param] = eq.Parameters.ContainsKey(param) ? eq.Parameters[param] : 1.0;
                    }
                    eq.Parameters = newParams;

                    var rootNode = new Parser(tokens).Parse();
                    eq.Function = new CompiledFunction(rootNode);
                }
                eq.HasError = false; 
                eq.ErrorMessage = "";
            }
            catch (Exception e)
            {
                eq.HasError = true;
                eq.ErrorMessage = e.Message;
            }
            NotifyStateChanged();
        }

        public void RemoveEquation(Guid id)
        {
            Equations.RemoveAll(e => e.Id == id);
            NotifyStateChanged();
        }
        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}

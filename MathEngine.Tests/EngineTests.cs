using NUnit.Framework;
using MathEngine.Lexing;
using MathEngine.Parsing;
using MathEngine.Expressions;

namespace MathEngine.Tests;

[TestFixture]
public class EngineTests
{
    // helper function to make evaluation of functions in tests faster - full life cycle of expression
    private double Evaluate(string expression, double x = 0)
    {
        var lexer = new Lexer(expression);
        var tokens = lexer.Tokenize();

        var parser = new Parser(tokens);
        var rootNode = parser.Parse();

        var compiledFunc = new CompiledFunction(rootNode);
        return compiledFunc.Evaluate(x);
    }

    [Test]
    public void BasicArithmeticsTest()
    {
        Assert.That(Evaluate("2 + 2"), Is.EqualTo(4).Within(1e-9));
        Assert.That(Evaluate("10 - 3.5 "), Is.EqualTo(6.5).Within(1e-9));
        Assert.That(Evaluate("3 - 5"), Is.EqualTo(-2).Within(1e-9));
        Assert.That(Evaluate("4 * 2.5"), Is.EqualTo(10).Within(1e-9));
        Assert.That(Evaluate("10 / 4"), Is.EqualTo(2.5).Within(1e-9));
        Assert.That(Evaluate("2 ^ 3"), Is.EqualTo(8).Within(1e-9));
    }

    [Test]
    public void OperatorPrecedenceTest()
    {
        Assert.That(Evaluate("2 + 2 * 2"), Is.EqualTo(6).Within(1e-9));
        Assert.That(Evaluate("3 * 2 ^ 2"), Is.EqualTo(24).Within(1e-9));
    }

    [Test]
    public void ParenthesesTest()
    {
        Assert.That(Evaluate("(2 + 2) * 2"), Is.EqualTo(8).Within(1e-9));
        Assert.That(Evaluate("2 * (3 + (1 + 1))"), Is.EqualTo(10).Within(1e-9));
    }

    [Test]
    public void VariablesTest()
    {
        Assert.That(Evaluate("x + 5", x: 10), Is.EqualTo(15).Within(1e-9));
        Assert.That(Evaluate("x * x", x: 4), Is.EqualTo(16).Within(1e-9));
        Assert.That(Evaluate("2 * x ^ 2", x: 10), Is.EqualTo(18).Within(1e-9));
    }

    [Test]
    public void MathFunctionsTest()
    {
        Assert.That(Evaluate("sin(x)", x: Math.PI / 2), Is.EqualTo(1).Within(1e-9));
        Assert.That(Evaluate("cos(sin(0))"), Is.EqualTo(1).Within(1e-9));
    }

    public void UnaryMinusTest()
    {
        Assert.That(Evaluate("-5"), Is.EqualTo(-5).Within(1e-9));
        Assert.That(Evaluate("-x", x: 10), Is.EqualTo(-10).Within(1e-9));
        Assert.That(Evaluate("5 * -2"), Is.EqualTo(-10).Within(1e-9));
        Assert.That(Evaluate("-(2 + 3)"), Is.EqualTo(-5).Within(1e-9));
    }

    //TODO: ZERO DIVISION TEST 
}

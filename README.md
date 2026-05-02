# 📈 SharpGraph WebAssembly

![C#](https://img.shields.io/badge/C%23-12.0-blue.svg?style=flat&logo=csharp)
![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg?style=flat&logo=dotnet)
![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-512BD4.svg?style=flat&logo=blazor)
![Canvas](https://img.shields.io/badge/Rendering-HTML5_Canvas-E34F26.svg?style=flat&logo=html5)
![License](https://img.shields.io/badge/License-MIT-green.svg)

**SharpGraph** is a high-performance, client-side mathematical visualization engine and graphical calculator built entirely in C# using Blazor WebAssembly.

Designed to overcome the performance limitations of JavaScript-based math parsers, SharpGraph compiles abstract mathematical expressions directly into native .NET delegates on the fly. It combines the computational raw power of WebAssembly with hardware-accelerated HTML5 Canvas rendering to visualize complex explicit and implicit functions at 60 FPS.

---

## ✨ Key Features

* 🚀 **JIT Math Engine:** Uses `System.Linq.Expressions` to dynamically compile Abstract Syntax Trees (AST) into native C# lambdas for zero-overhead execution during rendering.
* 🧮 **Advanced Parsing:** Custom Lexer and Parser (Shunting-yard algorithm) with built-in support for implicit multiplication (e.g., $2x\sin(x)$) and context-aware shadow unary operators.
* 🍩 **Implicit Equation Rendering:** Implements a dynamic-resolution **Marching Squares** algorithm to visualize complex contours like $x^2 + y^2 - r^2 = 0$ in real-time.
* 🎯 **Smart Tracing & Micro-stepping:** Numerical local-minimum search algorithms act as a "local radar" to instantly find and magnetically snap to roots and function intersections.
* 🎛️ **Reactive State Management:** Automatically detects unknown variables in expressions and generates dynamic UI sliders to animate parameters without recompiling the AST.
* ⚡ **Optimized JS Interop:** Avoids Interop bottlenecks by batching coordinate computations in Wasm and passing raw `double[]` arrays to a dedicated JavaScript renderer.

---

## 📊 Architecture & Performance

SharpGraph runs **100% on the client-side**. No server round-trips for rendering or math evaluation. 

* **Explicit Functions ($y=f(x)$):** Linear scanning with 1:1 Math-to-Pixel ratio.
* **Implicit Functions ($f(x,y)=0$):** Grid-based evaluation with linear interpolation. Grid resolution dynamically shifts (16px during Pan/Zoom for max FPS, 4px on idle for crisp edges).
* **Memory Footprint:** Centralized `CalculatorState` ensures singleton data flow without component-level state duplication.

### The Interop Bridge
```javascript
// renderer.js - Batch rendering avoids Wasm->JS bottleneck
window.canvasRenderer = {
    drawGraph: function (canvas, coordsArray, color) {
        const ctx = canvas.getContext('2d');
        ctx.beginPath();
        ctx.strokeStyle = color;
        // One JS call processes thousands of pre-computed Wasm points
        for (let i = 0; i < coordsArray.length; i += 2) {
            ctx.lineTo(coordsArray[i], coordsArray[i+1]);
        }
        ctx.stroke();
    }
};
```
## 🛠️ Quick Start
### 1. Prerequisites
* .NET 8.0 SDK or later.
* Any modern web browser supporting WebAssembly
### 2. Build and Run
* Clone the repository and launch the Blazor development server:
```Bash
git clone [https://github.com/yourusername/SharpGraph.git](https://github.com/yourusername/SharpGraph.git)
cd SharpGraph
dotnet watch run
```
The application will be available at ```https://localhost:5001.3```. 
### 3. Core Engine Example
Under the hood, the engine parses and compiles expressions instantly:
```C#
// 1. Lexical Analysis (Tokenization)
Lexer lexer = new Lexer("a * sin(x) + 2y");
List<Token> tokens = lexer.Tokenize();

// 2. Syntax Analysis (AST Construction)
Parser parser = new Parser(tokens);
INode rootNode = parser.Parse();

// 3. Dynamic Compilation to Native Delegate
// Signature: Func<double, double, Dictionary<string, double>, double>
var compiledEq = rootNode.Compile();

// 4. Ultra-fast evaluation in rendering loop
Dictionary<string, double> parameters = new() { { "a", 5.0 } };
double result = compiledEq(Math.PI, 10.0, parameters);
```
## 🧪 Unit Testing
The mathematical core is thoroughly tested using NUnit. *Tests cover:
* Operator precedence and parenthetical grouping.
* Contextual unary minus evaluation (e.g., $-x$ vs $0-x$).
* Exception handling for mathematical edge cases (e.g., returning NaN or Infinity for division by zero without crashing the WebAssembly runtime).
To run the test suite:
```Bash
dotnet test
```
## 🛣️ Roadmap & Future Work
* **Symbolic Differentiation:** Add capabilities to analytically find and plot derivatives $f'(x)$.
* **Polar Coordinates:** Support for $r = f(\theta)$ visualization.
* **3D Surface Rendering:** Extend the engine to support $z = f(x,y)$ using WebGL integration.
* **Export Options:** Save graphs as high-resolution SVG or PNG.
### 👨‍💻 Author
**Adam Kovalchuk**

Developed as a comprehensive research project exploring compiler theory, computer graphics, and .NET WebAssembly capabilities.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace RMModern.Generator;

[Generator]
public class MainGenerator : ISourceGenerator
{
    static List<SourceGenerator> generators = new List<SourceGenerator>()
    {
        Create<TranslationsGenerator>()
    };
    public static T Create<T>() where T : SourceGenerator, new() => new T();
    public void Execute(GeneratorExecutionContext context)
    {
        foreach (var generator in generators)
            generator.Execute(context);
    }


    public void Initialize(GeneratorInitializationContext context)
    {
        /*#if DEBUG
                if (!Debugger.IsAttached)
                    Debugger.Launch();
        #endif*/
    }
}

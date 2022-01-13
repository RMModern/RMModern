using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace RMModern.Generator;

[Generator, DiagnosticAnalyzer("C#", "CSharp")]
public class Main : DiagnosticAnalyzer, ISourceGenerator
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => new ImmutableArray<DiagnosticDescriptor>
    {
        //new ("RM5555", "Title", "Message", "Category", DiagnosticSeverity.Hidden, true, description: "")
    };

    public static T Create<T>() where T : SourceGenerator, new() => new T();
    public void Execute(GeneratorExecutionContext context)
    {
        var tasks = new List<Task>();
        var generators = new List<SourceGenerator>() 
        { 
            Create<TranslationsGenerator>() 
        };
        foreach(var generator in generators)
            tasks.Add(generator.Execute(context));
        Task.WaitAll(tasks.ToArray());
    }

    public void Initialize(GeneratorInitializationContext context)
    {
/*
#if DEBUG
        if (!Debugger.IsAttached)
            Debugger.Launch();
#endif
*/
    }

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
    }
}

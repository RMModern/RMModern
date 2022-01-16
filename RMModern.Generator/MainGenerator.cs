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

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class Analyzer : DiagnosticAnalyzer
{
    public static Analyzer Instance = new();
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = new ImmutableArray<DiagnosticDescriptor>()
    {
        new ("RM1500", "Generator Error", "Generator throwed an error.", "Generation", DiagnosticSeverity.Error, false)
    };

    DiagnosticDescriptor GetReportDescriptor(string id) => SupportedDiagnostics.FirstOrDefault(x => x.Id.ToLower() == id.ToLower());
    public void Report(string id, GeneratorExecutionContext context, Location at, params object[] args)
    {
        var descriptor = GetReportDescriptor(id);
        if (descriptor is null)
            return;
        context.ReportDiagnostic(Diagnostic.Create(descriptor, at, args));
    }

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
    }
}
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

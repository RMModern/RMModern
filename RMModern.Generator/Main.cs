using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RMModern.Generator;

[Generator]
public class Main : ISourceGenerator
{
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
}

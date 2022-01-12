using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace RMModern.Generator;

public abstract class SourceGenerator
{
    public SourceGenerator() { }
    public readonly StringBuilder Content = new StringBuilder();
    public abstract string ContentFile { get; }
    public async Task Execute(GeneratorExecutionContext context)
    {
        await OnExecute(context);
        context.AddSource(ContentFile, Content.ToString());
    }
    static string FormatModifiers(string[] modifiers, string joiner = " ") => 
        modifiers.Length > 0 ? string.Join(joiner, modifiers)+" " : "";
    string CurrentTabulation = "";
    const string Tab = "    ";
    public void AddTab() => CurrentTabulation += Tab;
    public void RemoveTab() => CurrentTabulation = CurrentTabulation.Remove(0, Tab.Length);
    public SourceGenerator Using(string @namespace) => AppendLine($"using {@namespace};");
    public IDisposable Block(string start = "{", string end = "}", bool tab = true)
    {
        AppendLine(start);
        if(tab)
            AddTab();
        return new Disposable(() => 
        { 
            if(tab) 
                RemoveTab(); 
            AppendLine(end); 
            AppendLine(); 
        });
    }
    public IDisposable Namespace(string @namespace, bool fileScoped = false)
    {
        AppendLine();
        if (fileScoped)
        {
            Content.AppendLine($"namespace {@namespace};");
            return null;
        }
        AppendLine($"namespace {@namespace}");
        return Block();
    }
    public IDisposable Class(string name, params string[] modifiers)
    {
        AppendLine();
        AppendLine($"{FormatModifiers(modifiers)}class {name}");
        return Block();
    }
    string IfNotEmpty(string value, string nonEmptyText) => string.IsNullOrWhiteSpace(value) ? "" : nonEmptyText;
    public SourceGenerator Field(string type, string name, string initializer = "", params string[] modifiers)
    {
        AppendLine();
        AppendLine($"{FormatModifiers(modifiers)}{type} {name}{IfNotEmpty(initializer, $" = {initializer}")};");
        return AppendLine();
    }
    public IDisposable Method(string type, string name, string[] modifiers = null, params string[] args)
    {
        AppendLine();
        AppendLine($"{FormatModifiers(modifiers)}{type} {name}({FormatModifiers(args, ", ")})");
        return Block();
    }
    public SourceGenerator InlineMethod(string type, string name, string[] modifiers, string expression, params string[] args)
    {
        AppendLine();
        return AppendLine($"{FormatModifiers(modifiers)}{type} {name}({FormatModifiers(args, ", ")}) => {expression};");
    }
    public SourceGenerator Append(string text)
    {
        Content.Append(CurrentTabulation+text);
        return this;
    }
    public SourceGenerator AppendLine()
    {
        Content.AppendLine();
        return this;
    }
    public SourceGenerator AppendLine(string text)
    {
        Content.AppendLine(CurrentTabulation + text);
        return this;
    }
    public abstract Task OnExecute(GeneratorExecutionContext context);
}
public class Disposable : IDisposable
{
    Action OnDispose;
    public Disposable(Action onDispose)
    {
        OnDispose = onDispose;
    }
    public void Dispose() => OnDispose?.Invoke();
}
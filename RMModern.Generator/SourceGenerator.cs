using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace RMModern.Generator;

public abstract class SourceGenerator
{
    public SourceGenerator() { }
    public readonly StringBuilder Content = new StringBuilder();
    public abstract string ContentFile { get; }
    public void Execute(GeneratorExecutionContext context)
    {
        Content.Clear();
        OnExecute(context);
        if(!string.IsNullOrWhiteSpace(ContentFile))
            context.AddSource(ContentFile, SourceText.From(Content.ToString(), Encoding.UTF8));
    }
    static string FormatModifiers(string[] modifiers, string joiner = " ", string postfix = " ") =>
        modifiers.Length > 0 ? string.Join(joiner, modifiers) + postfix : "";
    static string FormatArgs(string[] args) => FormatModifiers(args, ", ", "");
    string CurrentTabulation = "";
    const string Tab = "    ";
    public void AddTab() => CurrentTabulation += Tab;
    public void RemoveTab() => CurrentTabulation = CurrentTabulation.Remove(0, Tab.Length);
    public SourceGenerator Using(string @namespace) => AppendLine($"using {@namespace};");
    public IDisposable Block(string start = "{", string end = "}", bool tab = true)
    {
        AppendLine(start);
        if (tab)
            AddTab();
        return new Disposable(() =>
        {
            if (tab)
                RemoveTab();
            AppendLine(end);
            AppendLine();
        });
    }
    string IfNotEmpty(string value, string nonEmptyText) => string.IsNullOrWhiteSpace(value) ? "" : nonEmptyText;

    public IDisposable Namespace(string @namespace, bool fileScoped = false)
    {
        if (string.IsNullOrWhiteSpace(@namespace))
            return new Disposable();
        if (fileScoped)
        {
            Content.AppendLine($"namespace {@namespace};");
            return new Disposable();
        }

        return AppendLine($"namespace {@namespace}").Block();
    }

    public IDisposable Class(string name, params string[] modifiers) =>
        AppendLine($"{FormatModifiers(modifiers)}class {name}").Block();

    public SourceGenerator Field(string type, string name, string initializer = "", params string[] modifiers) =>
        AppendLine($"{FormatModifiers(modifiers)}{type} {name}{IfNotEmpty(initializer, $" = {initializer}")};").AppendLine();

    public IDisposable Method(string type, string name, string[] modifiers = null, params string[] args) =>
        AppendLine($"{FormatModifiers(modifiers)}{type} {name}({FormatArgs(args)})").Block();

    public SourceGenerator InlineMethod(string type, string name, string[] modifiers, string expression, params string[] args) =>
        AppendLine($"{FormatModifiers(modifiers)}{type} {name}({FormatArgs(args)}) => {expression};");

    public SourceGenerator Append(string text)
    {
        Content.Append(CurrentTabulation + text);
        return this;
    }
    public SourceGenerator AppendLine() => AppendLine("");
    public SourceGenerator AppendLine(string text)
    {
        Content.AppendLine(CurrentTabulation + text);
        return this;
    }
    public abstract void OnExecute(GeneratorExecutionContext context);
}
public class Disposable : IDisposable
{
    Action OnDispose;
    public Disposable(Action onDispose = null)
    {
        OnDispose = onDispose;
    }
    public void Dispose() => OnDispose?.Invoke();
}
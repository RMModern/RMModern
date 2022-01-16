using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace RMModern.Generator;

public class SyntaxRewriter : CSharpSyntaxRewriter
{
    public SyntaxRewriter() { }
    public SyntaxRewriter(SemanticModel semantic, GeneratorExecutionContext context, SyntaxRewriter parent = null)
    {
        Semantic = semantic;
        Context = context;
        Parent = parent;
    }
    public SyntaxRewriter WithSemantic(SemanticModel model)
    {
        Semantic = model;
        return this;
    }
    public SyntaxRewriter WithContext(GeneratorExecutionContext context)
    {
        Context = context;
        return this;
    }

    /// <summary>
    /// Calls OnVisit and Child.Visit
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public override sealed SyntaxNode Visit(SyntaxNode node)
    {
        node = OnVisit(base.Visit(node));
        return Child is null ? node : Child.WithSemantic(Semantic).WithContext(Context).Visit(node);
    }
    public virtual SyntaxNode OnVisit(SyntaxNode node) => node;
    public T With<T>() where T : SyntaxRewriter, new() => (T)(Child = new T() { Parent = this });
    public SyntaxRewriter Child, Parent;
    protected GeneratorExecutionContext Context;
    protected SemanticModel Semantic;
}

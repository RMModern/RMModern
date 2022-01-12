using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace RMModern.Generator;

public class SyntaxRewriter : CSharpSyntaxRewriter
{
    public SyntaxRewriter() { }
    public SyntaxRewriter(SemanticModel semantic, SyntaxRewriter parent = null)
    {
        Semantic = semantic;
        Parent = parent;
    }
    public SyntaxRewriter WithSemantic(SemanticModel model)
    {
        Semantic = model;
        return this;
    }
    /// <summary>
    /// Calls OnVisit and Child.Visit after that.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public override sealed SyntaxNode Visit(SyntaxNode node)
    {
        node = base.Visit(node);
        node = OnVisit(node);
        return Child is null ? node : Child.WithSemantic(Semantic).Visit(node);
    }
    public virtual SyntaxNode OnVisit(SyntaxNode node)
    {
        return node;
    }
    public T With<T>() where T : SyntaxRewriter, new() => (T)(Child = new T() { Parent = this });
    public SyntaxRewriter Child, Parent;
    protected SemanticModel Semantic;
}

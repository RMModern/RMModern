using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RMModern.Generator;

internal static class Extensions
{
    public static bool Has(this SyntaxTokenList modifiers, params SyntaxKind[] kinds)
    {
        foreach (var kind in kinds)
        {
            if (!modifiers.Any(x => x.IsKind(kind)))
                return false;
        }
        return true;
    }
    public static void RewriteRoot(this SyntaxTree old, SyntaxNode newRoot)
    {
        if (newRoot != old.GetRoot())
            File.WriteAllText(old.FilePath, newRoot.ToFullString());
    }
    public static bool IsMissing(this SyntaxNode node) => node is null || node.IsMissing;
    public static bool IsMissing(this SyntaxToken token) => token.IsMissing || string.IsNullOrWhiteSpace(token.ValueText);

    public static IEnumerable<TypeDeclarationSyntax> FindTypeDeclarations(this SyntaxNode root, string name, params SyntaxKind[] modifiers)
    {
        foreach (var type in root.DescendantNodes().OfType<TypeDeclarationSyntax>())
        {
            if (!type.IsMissing() && !type.Identifier.IsMissing() && type.Identifier.ValueText == name && type.Modifiers.Has(modifiers))
                yield return type;
        }
        yield break;
    }
    public static string GetNamespace(this TypeDeclarationSyntax type, SemanticModel semantic)
    {
        try
        {
            var symbol = semantic.GetDeclaredSymbol(type.Parent);
            if (symbol is not INamespaceSymbol namespaceSymbol)
                return null;
            return namespaceSymbol.Name.ToString();
        }
        catch { }
        return null;
    }
}

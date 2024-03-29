﻿using System;
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

    public static IEnumerable<TypeDeclarationSyntax> FindTypeDeclarations(this SyntaxNode root, string name = null, params SyntaxKind[] modifiers)
    {
        foreach (var type in root.DescendantNodes().OfType<TypeDeclarationSyntax>())
        {
            if (!type.IsMissing() && !type.Identifier.IsMissing() && type.Modifiers.Has(modifiers) && (name is null || (type.Identifier.ValueText == name)))
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
            return namespaceSymbol.GetFullNamespace();
        }
        catch { }
        return null;
    }
    public static string GetFullNamespace(this INamespaceSymbol namespaceSymbol)
    {
        var fullNamespace = namespaceSymbol.Name;

        if (!string.IsNullOrEmpty(fullNamespace))
        {
            var parentNamespace = namespaceSymbol.ContainingNamespace.GetFullNamespace();
            if (!string.IsNullOrEmpty(parentNamespace))
                fullNamespace = parentNamespace + Type.Delimiter + fullNamespace;
        }

        return fullNamespace;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RMModern.Generator;

public class TranslationsSearcher : SyntaxRewriter
{
    public TranslationsSearcher(SemanticModel semantic, GeneratorExecutionContext context, ClassDeclarationSyntax @class) : base(semantic, context) { Class = @class; }
    public ClassDeclarationSyntax Class;
    public Dictionary<string, string> Translations = new();
    public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax field)
    {
        var fieldDecl = field.Declaration;
        if (fieldDecl.IsMissing())
            return field;
        if (fieldDecl.Type.IsMissing())
            return field;
        var typeSymbol = Semantic.GetTypeInfo(field.Declaration.Type);
        if (typeSymbol.Type.SpecialType != SpecialType.System_String)
            return field;
        try
        {
            foreach (var variable in fieldDecl.Variables)
            {
                var id = variable.Identifier;
                var initializer = variable.Initializer;
                if (variable.IsMissing() || id.IsMissing || initializer.IsMissing() || initializer.Value.IsMissing())
                    continue;
                var name = id.ValueText;
                if (Translations.ContainsKey(name))
                    continue;
                Translations.Add(name, initializer.Value.ToString());
            }
        }
        catch { }

        return field;
    }
}

public class TranslationsGenerator : SourceGenerator
{
    public override string ContentFile => "Translations.g.cs";

    public override void OnExecute(GeneratorExecutionContext context)
    {
        var compilation = context.Compilation;
        var syntaxTrees = compilation.SyntaxTrees;
        foreach (var tree in syntaxTrees)
        {
            if (!tree.TryGetRoot(out var root))
                continue;
            var semantic = compilation.GetSemanticModel(tree);
            Dictionary<string, string> allTranslations = new();
            foreach (var type in root.FindTypeDeclarations("Translations", SyntaxKind.StaticKeyword, SyntaxKind.PartialKeyword))
            {
                if (type is not ClassDeclarationSyntax @class)
                    continue;

                var @namespace = @class.GetNamespace(semantic);

                var searcher = new TranslationsSearcher(semantic, context, @class);
                searcher.Visit(root);

                foreach (var translation in allTranslations)
                    searcher.Translations.Remove(translation.Key);

                if (searcher.Translations.Count > 0)
                {
                    using (Namespace(@namespace))
                    {
                        using (Class(@class.Identifier.ValueText, "public", "static", "partial"))
                        {
                            foreach (var translation in searcher.Translations)
                            {
                                allTranslations.Add(translation.Key, translation.Value);
                                AppendLine($"/// <summary><c>{translation.Value}</c></summary>");
                                InlineMethod("string", $"Translate{translation.Key}", new[] { "public", "static" }, $"Translate(nameof({translation.Key}), args)", "params string[] args");
                            }
                        }
                    }
                }
            }
        }
    }
}

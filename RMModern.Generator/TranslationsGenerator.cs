using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RMModern.Generator;

public class TranslationsRewriter : SyntaxRewriter
{
    public Dictionary<string, string> Translations = new Dictionary<string, string>();
    public string Namespace;
    public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax field)
    {
        field = base.VisitFieldDeclaration(field) as FieldDeclarationSyntax;
        if (field.Parent is ClassDeclarationSyntax
            {
                Identifier:
                {
                    ValueText: "Translations"
                }
            } @class && @class.Modifiers.Has(SyntaxKind.StaticKeyword, SyntaxKind.PartialKeyword))
        {
            var symbol = Semantic.GetDeclaredSymbol(@class.Parent);
            Namespace = (symbol as INamespaceSymbol).Name.ToString();
            foreach (var variable in field.Declaration.Variables)
            {
                var name = variable.Identifier.ValueText;
                if (Translations.ContainsKey(name))
                    continue;
                Translations.Add(name, variable.Initializer.Value.ToString());
            }
        }
        return field;
    }
}

public class TranslationsGenerator : SourceGenerator
{
    public override string ContentFile => "Translations.g.cs";

    public override async Task OnExecute(GeneratorExecutionContext context)
    {
        var compilation = context.Compilation;
        var syntaxTrees = compilation.SyntaxTrees;
        foreach (var tree in syntaxTrees)
        {
            if (!tree.TryGetRoot(out var root))
                continue;
            var semantic = compilation.GetSemanticModel(tree);
            var rewriter = new SyntaxRewriter(semantic);
            var translations = rewriter.With<TranslationsRewriter>();
            rewriter.Visit(root);
            if (translations.Translations.Count > 0)
            {
                using (Namespace(translations.Namespace))
                {
                    using (Class("Translations", "public", "static", "partial"))
                    {
                        foreach (var translation in translations.Translations)
                        {
                            AppendLine($"/// <summary><c>{translation.Value}</c></summary>");
                            InlineMethod("string", $"Translate{translation.Key}", new[] { "public", "static" }, $"Translate(nameof({translation.Key}), args)", "params string[] args");
                        }
                    }
                }
            }
        }
    }
}

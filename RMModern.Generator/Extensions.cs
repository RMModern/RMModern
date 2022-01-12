using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

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
}

using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.Statements;

namespace CCompiler.CSyntaxTree.TacExpressions;

public abstract class TacExpressionNode : ExpressionNode
{
    public override TacExpressionNode ConvertToTac(List<BlockItem> blockItems)
    {
        return this;
    }

    public override Token GetRepresentativeToken()
    {
        return null!;
    }

    public override void VariableResolution(SymbolTable symbolTable)
    {
    }
}
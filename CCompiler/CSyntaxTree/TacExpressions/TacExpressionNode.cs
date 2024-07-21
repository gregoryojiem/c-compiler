using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.TacExpressions;

public abstract class TacExpressionNode : ExpressionNode
{
    public override TacExpressionNode ConvertToTac(List<TacStatementNode> tacStatements)
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
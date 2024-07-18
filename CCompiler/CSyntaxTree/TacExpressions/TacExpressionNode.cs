using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.Statements;

namespace CCompiler.CSyntaxTree.TacExpressions;

public abstract class TacExpressionNode : ExpressionNode
{
    public override TacExpressionNode ConvertToTac(List<StatementNode> statementList)
    {
        return this;
    }
}
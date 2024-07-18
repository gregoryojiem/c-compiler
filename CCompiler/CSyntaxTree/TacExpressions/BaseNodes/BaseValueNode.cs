using CCompiler.CSyntaxTree.Statements;

namespace CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

public abstract class BaseValueNode : TacExpressionNode
{
    public override TacExpressionNode ConvertToTac(List<StatementNode> statementList)
    {
        return this;
    }
}
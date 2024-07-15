using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.Statements;

public class DeclarationNode : StatementNode
{
    public readonly VariableNode Variable;
    public readonly TacExpressionNode ExpressionNode;

    public DeclarationNode(VariableNode variable, TacExpressionNode expressionNode)
    {
        Variable = variable;
        ExpressionNode = expressionNode;
    }

    public override void ConvertToTac(List<StatementNode> statementList)
    {
        throw new NotImplementedException();
    }
}
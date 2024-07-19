using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.TacStatements;

public class AssignmentNode : TacStatementNode
{
    public readonly TacVariableNode TacVariable;
    public readonly TacExpressionNode ExpressionNode;

    public AssignmentNode(TacVariableNode tacVariable, TacExpressionNode expressionNode)
    {
        TacVariable = tacVariable;
        ExpressionNode = expressionNode;
    }
    
    public override string ToString()
    {
        return TacVariable + " = " + ExpressionNode;
    }
}
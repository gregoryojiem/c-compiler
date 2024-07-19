using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.TacStatements;

public class AssignmentNode : TacStatementNode
{
    public readonly VariableNode Variable;
    public readonly TacExpressionNode ExpressionNode;

    public AssignmentNode(VariableNode variable, TacExpressionNode expressionNode)
    {
        Variable = variable;
        ExpressionNode = expressionNode;
    }
    
    public override string ToString()
    {
        return Variable + " = " + ExpressionNode;
    }
}
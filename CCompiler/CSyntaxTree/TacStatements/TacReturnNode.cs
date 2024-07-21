using CCompiler.CSyntaxTree.TacExpressions;

namespace CCompiler.CSyntaxTree.TacStatements;

public class TacReturnNode : TacStatementNode
{
    public TacExpressionNode ReturnValue;

    public TacReturnNode(TacExpressionNode returnValue)
    {
        ReturnValue = returnValue;
    }
}
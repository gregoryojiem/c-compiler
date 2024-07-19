using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.TacExpressions;

public class TacUnaryOpNode : TacExpressionNode
{
    public readonly TokenType UnaryOperator;
    public readonly BaseValueNode Operand;

    public TacUnaryOpNode(TokenType unaryOperator, BaseValueNode operand)
    {
        UnaryOperator = unaryOperator;
        Operand = operand;
    }

    public override string ToString()
    {
        return Token.GetTypeString(UnaryOperator) + "(" + Operand + ")";
    }
}
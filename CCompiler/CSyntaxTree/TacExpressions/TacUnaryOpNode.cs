using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.TacExpressions;

public class TacUnaryOpNode : TacExpressionNode
{
    public readonly TokenType UnaryOperator;
    public readonly ValueNode Operand;

    public TacUnaryOpNode(TokenType unaryOperator, ValueNode operand)
    {
        UnaryOperator = unaryOperator;
        Operand = operand;
    }

    public override string ToString()
    {
        return Token.GetTypeString(UnaryOperator) + "(" + Operand + ")";
    }
}
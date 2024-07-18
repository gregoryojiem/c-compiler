using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.TacExpressions;

public class TacUnaryOpNode : TacExpressionNode
{
    public TokenType UnaryOperator;
    public BaseValueNode Operand;

    public TacUnaryOpNode(TokenType unaryOperator, BaseValueNode operand)
    {
        UnaryOperator = unaryOperator;
        Operand = operand;
    }
}
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.TacExpressions;

public class TacUnaryOpNode : TacExpressionNode
{
    public Token UnaryOperator;
    public BaseValueNode Operand;

    public TacUnaryOpNode(Token unaryOperator, BaseValueNode operand)
    {
        UnaryOperator = unaryOperator;
        Operand = operand;
    }
}
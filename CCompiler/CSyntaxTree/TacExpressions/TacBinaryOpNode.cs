using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.TacExpressions;

public class TacBinaryOpNode : TacExpressionNode
{
    public Token BinaryOperator;
    public BaseValueNode LeftOperand;
    public BaseValueNode RightOperand;

    public TacBinaryOpNode(Token binaryOperator, BaseValueNode leftOperand, BaseValueNode rightOperand)
    {
        BinaryOperator = binaryOperator;
        LeftOperand = leftOperand;
        RightOperand = rightOperand;
    }

    public override string ToString()
    {
        return "(" + LeftOperand + " " + BinaryOperator + " " + RightOperand + ")";
    }
}
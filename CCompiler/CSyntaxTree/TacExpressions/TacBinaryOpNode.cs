using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.TacExpressions;

public class TacBinaryOpNode : TacExpressionNode
{
    public TokenType BinaryOperator;
    public BaseValueNode LeftOperand;
    public BaseValueNode RightOperand;

    public TacBinaryOpNode(TokenType binaryOperator, BaseValueNode leftOperand, BaseValueNode rightOperand)
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
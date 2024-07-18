using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.TacExpressions;

public class TacBinaryOpNode : TacExpressionNode
{
    public readonly TokenType BinaryOperator;
    public readonly BaseValueNode LeftOperand;
    public readonly BaseValueNode RightOperand;

    public TacBinaryOpNode(TokenType binaryOperator, BaseValueNode leftOperand, BaseValueNode rightOperand)
    {
        BinaryOperator = binaryOperator;
        LeftOperand = leftOperand;
        RightOperand = rightOperand;
    }

    public override string ToString()
    {
        return "(" + LeftOperand + " " + Token.GetTypeString(BinaryOperator) + " " + RightOperand + ")";
    }
}
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.TacExpressions;

public class TacBinaryOpNode : TacExpressionNode
{
    public readonly TokenType BinaryOperator;
    public readonly ValueNode LeftOperand;
    public readonly ValueNode RightOperand;

    public TacBinaryOpNode(TokenType binaryOperator, ValueNode leftOperand, ValueNode rightOperand)
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
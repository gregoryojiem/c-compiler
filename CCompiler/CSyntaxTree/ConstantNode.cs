namespace CCompiler.CSyntaxTree;

public class ConstantNode : ExpressionNode
{
    public readonly Token _constantValue;

    public ConstantNode(TokenList tokens)
    {
        _constantValue = tokens.PopExpected(TokenType.IntegerLiteral);
    }

    public override string ToString()
    {
        if (_constantValue == null)
        {
            throw new Exception("Unexpected null constant");
        }
        return _constantValue.ToString();
    }
}
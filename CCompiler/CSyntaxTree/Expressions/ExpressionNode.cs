using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.Expressions;

public abstract class ExpressionNode
{
    protected static int TempVariableCounter;
    protected static int TempLabelCounter;

    public static ExpressionNode ParseExpressionNode(TokenList tokens, int minPrecedence)
    {
        var leftExpr = ParseExpressionFactor(tokens);
        var nextTokenType = tokens.Peek().Type;
        while (Token.BinaryOps.Contains(nextTokenType) && Token.GetPrecedence(nextTokenType) >= minPrecedence)
        {
            var binaryOp = tokens.Pop();
            var rightExpr = ParseExpressionNode(tokens, Token.GetPrecedence(nextTokenType) + 1);
            leftExpr = new BinaryOpNode(binaryOp, leftExpr, rightExpr);
            nextTokenType = tokens.Peek().Type;
        }

        return leftExpr;
    }

    protected static ExpressionNode ParseExpressionFactor(TokenList tokens)
    {
        ExpressionNode expressionNode;

        var rightParenRequired = tokens.Peek().Type == TokenType.LeftParen;
        if (rightParenRequired)
        {
            tokens.PopExpected(TokenType.LeftParen);
            expressionNode = ParseExpressionNode(tokens, 0);
            tokens.PopExpected(TokenType.RightParen);
            return expressionNode;
        }

        var expressionStart = tokens.Peek().Type;
        if (Token.UnaryOps.Contains(expressionStart))
        {
            expressionNode = new UnaryOpNode(tokens);
        }
        else if (expressionStart == TokenType.IntegerLiteral)
        {
            var tokenString = tokens.PopExpected(TokenType.IntegerLiteral).Value;
            expressionNode = new ConstantNode(int.Parse(tokenString));
        }
        else
        {
            var invalidToken = tokens.Pop();
            throw new ParseException(invalidToken, $"Expected valid expression, instead found: {invalidToken}");
        }

        return expressionNode;
    }

    public abstract TacExpressionNode ConvertToTac(List<StatementNode> statementList);
}
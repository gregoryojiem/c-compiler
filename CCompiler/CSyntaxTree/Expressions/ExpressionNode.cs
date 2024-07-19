using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;

namespace CCompiler.CSyntaxTree.Expressions;

public abstract class ExpressionNode
{
    public static int UniqueVariableCounter;
    protected static int UniqueLabelCounter;

    public static ExpressionNode ParseExpressionNode(TokenList tokens, int minPrecedence)
    {
        var leftExpr = ParseExpressionFactor(tokens);
        var nextToken = tokens.Peek().Type;
        while (tokens.NextTokenGroup(TokenList.Group.BinaryOp) && Token.GetPrecedence(nextToken) >= minPrecedence)
        {
            // assignment is right-associative in C 
            var assignment = nextToken == TokenType.Assignment;
            var nextPrecedence = Token.GetPrecedence(nextToken) + (assignment ? 0 : 1);
            var binaryOp = tokens.Pop();
            var rightExpr = ParseExpressionNode(tokens, nextPrecedence);

            if (assignment)
            {
                leftExpr = new AssignmentOpNode(leftExpr, rightExpr);
            }
            else
            {
                leftExpr = new BinaryOpNode(binaryOp, leftExpr, rightExpr);
            }

            nextToken = tokens.Peek().Type;
        }

        return leftExpr;
    }

    protected static ExpressionNode ParseExpressionFactor(TokenList tokens)
    {
        var rightParenRequired = tokens.Peek().Type == TokenType.LeftParen;
        if (rightParenRequired)
        {
            tokens.PopExpected(TokenType.LeftParen);
            var expressionNode = ParseExpressionNode(tokens, 0);
            tokens.PopExpected(TokenType.RightParen);
            return expressionNode;
        }

        if (tokens.NextTokenGroup(TokenList.Group.UnaryOp))
        {
            return new UnaryOpNode(tokens);
        }

        if (tokens.Peek().Type == TokenType.IntLiteral)
        {
            return new ConstantNode(tokens.Pop());
        }

        if (tokens.Peek().Type == TokenType.Identifier)
        {
            return new VariableNode(tokens.Pop());
        }

        var invalidToken = tokens.Pop();
        throw new ParseException(invalidToken, $"Expected valid expression, instead found: {invalidToken}");
    }

    public abstract void VariableResolution(Dictionary<string, string> variableMap);

    public abstract Token GetRepresentativeToken();

    public abstract TacExpressionNode ConvertToTac(List<StatementNode> statementList);
}
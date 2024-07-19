using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.Expressions;

public abstract class ExpressionNode
{
    protected static int TempVariableCounter;
    protected static int TempLabelCounter;

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
            return new ConstantNode(int.Parse(tokens.Pop().Value));
        }

        if (tokens.Peek().Type == TokenType.Identifier)
        {
            return new VariableNode(tokens.Pop().Value);
        }

        var invalidToken = tokens.Pop();
        throw new ParseException(invalidToken, $"Expected valid expression, instead found: {invalidToken}");
    }

    public abstract TacExpressionNode ConvertToTac(List<StatementNode> statementList);
}
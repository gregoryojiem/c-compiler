using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;

namespace CCompiler.CSyntaxTree.Expressions;

public abstract class ExpressionNode
{
    public static int UniqueVariableCounter;
    public static int UniqueLabelCounter;

    public static ExpressionNode ParseExpressionNode(TokenList tokens, int minPrecedence)
    {
        var leftExpr = ParseExpressionFactor(tokens);
        var nextToken = tokens.Peek().Type;
        while (tokens.NextTokenGroup(TokenList.Group.BinaryOp) && Token.GetPrecedence(nextToken) >= minPrecedence)
        {
            // assignment is right-associative in C 
            var increasePrecedence = nextToken != TokenType.Assignment && nextToken != TokenType.Ternary;
            var nextPrecedence = Token.GetPrecedence(nextToken) + (increasePrecedence ? 1 : 0);
            var operatorType = tokens.Pop().Type;

            ExpressionNode? middleExpr = null;
            if (operatorType == TokenType.Ternary)
            {
                middleExpr = ParseExpressionNode(tokens, 0);
                tokens.PopExpected(TokenType.TernaryDelim);
            }

            var rightExpr = ParseExpressionNode(tokens, nextPrecedence);

            if (middleExpr != null)
                leftExpr = new TernaryOpNode(leftExpr, middleExpr, rightExpr);
            else if (operatorType == TokenType.Assignment)
                leftExpr = new AssignmentOpNode(leftExpr, rightExpr);
            else
                leftExpr = new BinaryOpNode(operatorType, leftExpr, rightExpr);

            nextToken = tokens.Peek().Type;
        }

        return leftExpr;
    }

    protected static ExpressionNode ParseExpressionFactor(TokenList tokens)
    {
        if (tokens.PopIfFound(TokenType.LeftParen))
        {
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

    public abstract void VariableResolution(SymbolTable symbolTable);

    public abstract Token GetRepresentativeToken();

    public abstract TacExpressionNode ConvertToTac(List<StatementNode> statementList);
}
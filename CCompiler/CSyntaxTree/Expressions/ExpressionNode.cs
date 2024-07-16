using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.Expressions;

public abstract class ExpressionNode
{
    private static int _tempVariableCounter;

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
            expressionNode = new ConstantNode(tokens);
        }
        else
        {
            var invalidToken = tokens.Pop();
            throw new ParseException(invalidToken, $"Expected valid expression, instead found: {invalidToken}");
        }

        return expressionNode;
    }

    public static TacExpressionNode ConvertToTac(List<StatementNode> statementList, ExpressionNode expressionNode)
    {
        if (expressionNode is BaseValueNode baseNode)
        {
            return baseNode;
        }

        if (expressionNode is UnaryOpNode unaryOpNode)
        {
            var exprValue = (BaseValueNode)ConvertToTac(statementList, unaryOpNode.Expression);
            var unaryOp = unaryOpNode.UnaryOperator;
            var tacNode = new TacUnaryOpNode(unaryOp, exprValue);
            var tempVar = new VariableNode("tmp" + _tempVariableCounter++);
            statementList.Add(new DeclarationNode(tempVar, tacNode));
            return tempVar;
        }

        if (expressionNode is BinaryOpNode binaryOpNode)
        {
            var leftExprValue = (BaseValueNode)ConvertToTac(statementList, binaryOpNode.LeftExpression);
            var rightExprValue = (BaseValueNode)ConvertToTac(statementList, binaryOpNode.RightExpression);
            var binaryOp = binaryOpNode.BinaryOperator;
            var tacNode = new TacBinaryOpNode(binaryOp, leftExprValue, rightExprValue);
            var tempVar = new VariableNode("tmp" + _tempVariableCounter++);
            statementList.Add(new DeclarationNode(tempVar, tacNode));
            return tempVar;
        }

        throw new NotImplementedException();
    }
}
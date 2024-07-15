using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.Expressions;

public abstract class ExpressionNode
{
    private static int _tempVariableCounter;
    
    public static ExpressionNode CreateExpressionNode(TokenList tokens)
    {
        ExpressionNode expressionNode;

        var rightParenRequired = tokens.Peek().Type == TokenType.LeftParen;
        if (rightParenRequired)
        {
            tokens.PopExpected(TokenType.LeftParen);
            expressionNode = CreateExpressionNode(tokens);
            tokens.PopExpected(TokenType.RightParen);
            return expressionNode;
        }

        var expressionStart = tokens.Peek().Type;
        if (Token.UnaryOperators.Contains(expressionStart))
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
            var baseValue = (BaseValueNode)ConvertToTac(statementList, unaryOpNode.Expression);
            var unaryOp = unaryOpNode.UnaryOperator;
            var tacNode = new TacUnaryOpNode(unaryOp, baseValue);
            var tempVar = new VariableNode("tmp" + _tempVariableCounter++);
            statementList.Add(new DeclarationNode(tempVar, tacNode));
            return tempVar;
        }
        throw new NotImplementedException();
    }
}
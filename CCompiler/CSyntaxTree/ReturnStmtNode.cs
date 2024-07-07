namespace CCompiler.CSyntaxTree;

public class ReturnStmtNode : StatementNode
{
    public readonly ExpressionNode ReturnValue;

    public ReturnStmtNode(TokenList tokens)
    {
        tokens.PopExpected(TokenType.Return);
        ReturnValue = ExpressionNode.CreateExpressionNode(tokens);
        tokens.PopExpected(TokenType.Semicolon);
    } 
}
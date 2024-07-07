namespace CCompiler.CSyntaxTree;

public class FunctionNode
{
    public readonly Token ReturnType;
    public readonly Token Name;
    public readonly List<StatementNode> Body;

    public FunctionNode(TokenList tokens)
    {
        Body = new List<StatementNode>();
        ReturnType = tokens.Pop();
        SyntaxError.ValidReturnType(ReturnType);

        Name = tokens.Pop();
        SyntaxError.ValidFunctionName(Name);

        tokens.PopExpected(TokenType.LeftParen);
        //todo argument handling
        tokens.PopExpected(TokenType.RightParen);

        tokens.PopExpected(TokenType.LeftBrace);
        while (!tokens.PeekExpected(TokenType.RightBrace)) //todo see if adding a special body class with a return would be worthwhile
        {
            var statementNode = StatementNode.CreateStatementNode(tokens);
            Body.Add(statementNode);
        }
        tokens.PopExpected(TokenType.RightBrace);
    }
}
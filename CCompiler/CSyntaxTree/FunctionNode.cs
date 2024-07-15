using CCompiler.CSyntaxTree.Statements;

namespace CCompiler.CSyntaxTree;

public class FunctionNode
{
    public readonly Token ReturnType;
    public readonly Token Name;
    public List<StatementNode> Body;

    public FunctionNode(TokenList tokens)
    {
        Body = new List<StatementNode>();
        ReturnType = tokens.Pop();
        ParseException.ValidReturnType(ReturnType);

        Name = tokens.Pop();
        ParseException.ValidFunctionName(Name);

        ParseFunctionArguments(tokens);
        ParseFunctionBody(tokens);
    }

    private void ParseFunctionArguments(TokenList tokens)
    {
        tokens.PopExpected(TokenType.LeftParen);
        //TODO argument handling
        while (tokens.Pop().Type != TokenType.RightParen)
        {
        }
        //tokens.PopExpected(TokenType.RightParen);
    }

    private void ParseFunctionBody(TokenList tokens)
    {
        //TODO in the future, look into whether adding a FunctionBodyNode class would be worthwhile
        tokens.PopExpected(TokenType.LeftBrace);
        while (tokens.Peek().Type != TokenType.RightBrace)
        {
            var statementNode = StatementNode.CreateStatementNode(tokens);
            Body.Add(statementNode);
        }

        tokens.PopExpected(TokenType.RightBrace);
    }

    public void ConvertToTac()
    {
        var tacStatementBody = new List<StatementNode>();
        foreach (var statementNode in Body)
        {
            statementNode.ConvertToTac(tacStatementBody);
        }

        Body = tacStatementBody;
    }
}
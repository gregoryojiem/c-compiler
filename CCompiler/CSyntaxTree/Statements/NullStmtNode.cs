namespace CCompiler.CSyntaxTree.Statements;

public class NullStmtNode : StatementNode
{
    public NullStmtNode(TokenList tokens)
    {
        tokens.PopExpected(TokenType.Semicolon);
    }

    public override void SemanticPass(Dictionary<string, string> variableMap)
    {
    }

    public override void ConvertToTac(List<StatementNode> statementList)
    {
        throw new NotImplementedException();
    }
}
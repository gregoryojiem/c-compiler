using CCompiler.CSyntaxTree.Statements;

namespace CCompiler.CSyntaxTree;

public class BlockNode
{
    public List<StatementNode> _statements;

    public BlockNode(TokenList tokens)
    {
        _statements = new List<StatementNode>();

        tokens.PopExpected(TokenType.LeftBrace);
        while (!tokens.PopIfFound(TokenType.RightBrace))
        {
            var statementNode = StatementNode.CreateStatementNode(tokens);
            _statements.Add(statementNode);
        }
    }

    public void AddStmt(StatementNode stmt)
    {
        _statements.Add(stmt);
    }

    public void Validate(SymbolTable symbolTable)
    {
        foreach (var statementNode in _statements)
        {
            statementNode.SemanticPass(symbolTable);
        }
    }

    public void ConvertToTac()
    {
        var tacStatements = new List<StatementNode>();
        foreach (var statementNode in _statements)
        {
            statementNode.ConvertToTac(tacStatements);
        }

        _statements = tacStatements;
    }

    public void ConvertToTac(List<StatementNode> tacStatements)
    {
        foreach (var statementNode in _statements)
        {
            statementNode.ConvertToTac(tacStatements);
        }
    }
    
    public override string ToString()
    {
        return _statements.Aggregate("", (current, statementNode) => current + (statementNode + "\n"));
    }
}
using CCompiler.CSyntaxTree.Statements;

namespace CCompiler.CSyntaxTree;

public class BlockNode
{
    public static int IndentationLevel = 1;
    public List<StatementNode> _statements;

    public BlockNode(TokenList tokens)
    {
        _statements = new List<StatementNode>();

        tokens.PopExpected(TokenType.LeftBrace);
        while (!tokens.PopIfFound(TokenType.RightBrace))
        {
            var statementNode = StatementNode.ParseStatementNode(tokens);
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

    public static string GetIndent(StatementNode? stmt, bool addNewline = true, int modifier = 0)
    {
        var indent =  new string('\t', IndentationLevel + modifier);
        if (addNewline)
            indent = "\n" + indent;
        if (stmt is CompoundStmt)
            indent = " ";
        return indent;
    }
    
    public static void IncreaseIndent(StatementNode stmt)
    {
        if (stmt is not CompoundStmt)
        {
            IndentationLevel++;
        }
    }
    
    public static void DecreaseIndent(StatementNode stmt)
    {
        if (stmt is not CompoundStmt)
        {
            IndentationLevel--;
        }
    }
    
    public override string ToString()
    {
        var indent = new string('\t', IndentationLevel);
        IndentationLevel++;
        var bodyString = _statements.Aggregate("", (current, statement) => current + indent + statement + "\n");
        IndentationLevel--;
        return "{\n" + bodyString + new string('\t', IndentationLevel - 1) + "}";
    }
}
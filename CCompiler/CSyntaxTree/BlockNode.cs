using CCompiler.CSyntaxTree.Statements;
using CCompiler.CSyntaxTree.TacStatements;

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
        var indent = new string('\t', IndentationLevel + modifier);
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
        var outputBlock = "";
        var declarations = new List<string>();
        foreach (var statement in _statements)
        {
            var declaration = "";
            if (statement is AssignmentNode assignment && !declarations.Contains(assignment.TacVariable.Identifier))
            {
                declarations.Add(assignment.TacVariable.Identifier);
                declaration = "int ";
            }

            var indentToUse = statement is LabelNode ? indent[..^1] : indent;
            outputBlock += indentToUse + declaration + statement + "\n";
        }

        IndentationLevel--;
        return "{\n" + outputBlock + new string('\t', IndentationLevel - 1) + "}";
    }
}
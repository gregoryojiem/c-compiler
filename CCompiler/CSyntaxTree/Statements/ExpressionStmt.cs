using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.Statements.Loops;

namespace CCompiler.CSyntaxTree.Statements;

public class ExpressionStmt : StatementNode, IForInitialClause
{
    private readonly ExpressionNode _expression;

    public ExpressionStmt(TokenList tokens)
    {
        _expression = ExpressionNode.ParseExpression(tokens);
        tokens.PopExpected(TokenType.Semicolon);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
        _expression.VariableResolution(symbolTable);
    }

    public override void ConvertToTac(List<StatementNode> statementList)
    {
        _expression.ConvertToTac(statementList);
    }

    public override string ToString()
    {
        return _expression.ToString() ?? string.Empty;
    }
}
using CCompiler.CSyntaxTree.Expressions;

namespace CCompiler.CSyntaxTree.Statements;

public class ExpressionStmt : StatementNode
{
    private readonly ExpressionNode _expression;

    public ExpressionStmt(TokenList tokens)
    {
        _expression = ExpressionNode.ParseExpressionNode(tokens, 0);
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
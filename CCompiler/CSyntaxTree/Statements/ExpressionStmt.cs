using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.Statements.Loops;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.Statements;

public class ExpressionStmt : StatementNode
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

    public override void ConvertToTac(List<TacStatementNode> tacStatements)
    {
        _expression.ConvertToTac(tacStatements);
    }

    public override string ToString()
    {
        return _expression + ";";
    }
}
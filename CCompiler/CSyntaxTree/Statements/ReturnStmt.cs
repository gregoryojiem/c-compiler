using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.TacExpressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.Statements;

public class ReturnStmt : StatementNode
{
    private readonly ExpressionNode _returnValue;

    public ReturnStmt(TokenList tokens)
    {
        tokens.PopExpected(TokenType.Return);
        _returnValue = ExpressionNode.ParseExpression(tokens);
        tokens.PopExpected(TokenType.Semicolon);
    }

    public ReturnStmt(int returnValue)
    {
        _returnValue = new TacConstantNode(returnValue);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
        _returnValue.VariableResolution(symbolTable);
    }

    public override void ConvertToTac(List<TacStatementNode> tacStatements)
    {
        tacStatements.Add(new TacReturnNode(_returnValue.ConvertToTac(tacStatements)));
    }

    public override string ToString()
    {
        return "return " + _returnValue + ";";
    }
}
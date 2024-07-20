using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.Statements.Loops;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.Statements;

public class DeclarationStmt : ForInitialClause
{
    public readonly Token Identifier;
    private readonly TokenType _type;
    private readonly ExpressionNode? _expression;

    public DeclarationStmt(TokenList tokens)
    {
        _type = tokens.PopExpected(TokenType.IntType).Type;
        Identifier = tokens.PopExpected(TokenType.Identifier);
        if (tokens.PopIfFound(TokenType.Assignment))
            _expression = ExpressionNode.ParseExpression(tokens);

        tokens.PopExpected(TokenType.Semicolon);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
        SemanticException.CheckDuplicateDeclaration(symbolTable, Identifier);
        var variableName = Identifier.Value;
        var uniqueName = variableName + "." + ExpressionNode.UniqueVariableCounter++;
        symbolTable.AddVariable(variableName, uniqueName);
        Identifier.Value = uniqueName;
        _expression?.VariableResolution(symbolTable);
    }

    public override void ConvertToTac(List<StatementNode> statementList)
    {
        if (_expression == null)
            return;
        var variableName = Identifier.Value;
        var assignedExpr = _expression.ConvertToTac(statementList);
        statementList.Add(new AssignmentNode(new TacVariableNode(variableName), assignedExpr));
    }

    public override string ToString()
    {
        return Token.GetTypeString(_type) + " " + Identifier + " = " + _expression + ";";
    }
}
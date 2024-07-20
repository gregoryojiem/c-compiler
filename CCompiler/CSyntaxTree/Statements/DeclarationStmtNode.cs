using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.Statements;

public class DeclarationStmtNode : StatementNode
{
    private TokenType _type;
    public readonly Token _identifier;
    private ExpressionNode? _expression;

    public DeclarationStmtNode(TokenList tokens)
    {
        _type = tokens.PopExpected(TokenType.IntType).Type;
        _identifier = tokens.PopExpected(TokenType.Identifier);
        if (tokens.PopIfFound(TokenType.Assignment))
            _expression = ExpressionNode.ParseExpressionNode(tokens, 0);

        tokens.PopExpected(TokenType.Semicolon);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
        SemanticException.CheckDuplicateDeclaration(symbolTable, _identifier);
        var variableName = _identifier.Value;
        var uniqueName = variableName + "." + ExpressionNode.UniqueVariableCounter++;
        symbolTable.AddVariable(variableName, uniqueName);
        _identifier.Value = uniqueName;
        _expression?.VariableResolution(symbolTable);
    }

    public override void ConvertToTac(List<StatementNode> statementList)
    {
        if (_expression == null)
            return;
        var variableName = _identifier.Value;
        var assignedExpr = _expression.ConvertToTac(statementList);
        statementList.Add(new AssignmentNode(new TacVariableNode(variableName), assignedExpr));
    }

    public override string ToString()
    {
        return Token.GetTypeString(_type) + " " + _identifier + " = " + _expression;
    }
}
using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.Statements.Loops;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree;

public class DeclarationNode : BlockItem
{
    private readonly Token _identifier;
    private readonly TokenType _type;
    private readonly ExpressionNode? _expression;

    public DeclarationNode(TokenList tokens)
    {
        _type = tokens.PopExpected(TokenType.IntType).Type;
        _identifier = tokens.PopExpected(TokenType.Identifier);
        if (tokens.PopIfFound(TokenType.Assignment))
            _expression = ExpressionNode.ParseExpression(tokens);

        tokens.PopExpected(TokenType.Semicolon);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
        SemanticException.CheckDuplicateDeclaration(symbolTable, _identifier);
        var variableName = _identifier.Value;
        var uniqueName = variableName + "." + SymbolTable.VariableId++;
        symbolTable.AddVariable(variableName, uniqueName);
        _identifier.Value = uniqueName;
        _expression?.VariableResolution(symbolTable);
    }

    public override void ConvertToTac(List<TacStatementNode> tacStatements)
    {
        if (_expression == null)
            return;
        var variableName = _identifier.Value;
        var assignedExpr = _expression.ConvertToTac(tacStatements);
        tacStatements.Add(new AssignmentNode(new TacVariableNode(variableName), assignedExpr));
    }

    public override string ToString()
    {
        return Token.GetTypeString(_type) + " " + _identifier + " = " + _expression + ";";
    }
}
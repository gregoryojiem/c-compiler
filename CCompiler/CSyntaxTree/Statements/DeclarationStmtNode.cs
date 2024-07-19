using CCompiler.CSyntaxTree.Expressions;

namespace CCompiler.CSyntaxTree.Statements;

public class DeclarationStmtNode : StatementNode
{
    private TokenType _type;
    private Token _identifier;
    private ExpressionNode? _expression;

    public DeclarationStmtNode(TokenList tokens)
    {
        _type = tokens.PopExpected(TokenType.IntType).Type;
        _identifier = tokens.PopExpected(TokenType.Identifier);
        if (tokens.Peek().Type == TokenType.Assignment)
        {
            tokens.Pop();
            _expression = ExpressionNode.ParseExpressionNode(tokens, 0);
        }
        tokens.PopExpected(TokenType.Semicolon);
    }

    public override void SemanticPass(Dictionary<string, string> variableMap)
    {
        SemanticException.CheckDuplicateDeclaration(variableMap, _identifier);
        var variableName = _identifier.Value;
        var uniqueName = variableName + "." + ExpressionNode.UniqueVariableCounter++;
        variableMap.Add(variableName, uniqueName);
        _identifier.Value = uniqueName;
        _expression?.VariableResolution(variableMap);
    }
    
    public override void ConvertToTac(List<StatementNode> statementList)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return Token.GetTypeString(_type) + " " + _identifier + " = " + _expression;
    }
}
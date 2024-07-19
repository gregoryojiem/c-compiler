using System.Reflection;

namespace CCompiler;

public class TokenList
{
    public enum Group
    {
        DataType,
        LiteralValue,
        UnaryOp,
        BinaryOp,
        ExpressionStart
    }

    private static readonly List<TokenType> DataTypes = new()
    {
        TokenType.IntType
    };

    private static readonly List<TokenType> LiteralValues = new()
    {
        TokenType.IntLiteral,
        TokenType.Identifier
    };

    private static readonly List<TokenType> UnaryOps = new()
    {
        TokenType.Complement,
        TokenType.Negate,
        TokenType.Not
    };

    private static readonly List<TokenType> BinaryOps = new()
    {
        TokenType.Negate,
        TokenType.Add,
        TokenType.Multiply,
        TokenType.Divide,
        TokenType.Modulo,
        TokenType.And,
        TokenType.Or,
        TokenType.Eq,
        TokenType.Neq,
        TokenType.Lt,
        TokenType.Gt,
        TokenType.LtOrEq,
        TokenType.GtOrEq,
        TokenType.Assignment,
        TokenType.Ternary
    };

    private readonly List<Token> _tokens;
    private readonly int _lastLine;
    private readonly int _lastColumn;

    public TokenList(List<Token> tokens, int lastLine, int lastColumn)
    {
        _tokens = tokens;
        _lastLine = lastLine;
        _lastColumn = lastColumn;
    }

    public Token Peek()
    {
        if (_tokens.Count == 0)
        {
            throw new ParseException(_lastLine, _lastColumn, "Unexpected end of file.");
        }

        return _tokens[0];
    }

    public Token Pop()
    {
        if (_tokens.Count == 0)
        {
            throw new ParseException(_lastLine, _lastColumn, "Unexpected end of file.");
        }

        var token = _tokens[0];
        _tokens.RemoveAt(0);
        return token;
    }

    public bool PopIfFound(TokenType type)
    {
        if (Peek().Type != type)
            return false;
        Pop();
        return true;
    }

    public Token PopExpected(TokenType type)
    {
        var token = Pop();
        if (token.Type == type) return token;
        var expectedValue = Token.GetExpectedTypeString(type, token);
        throw new ParseException(token, $"Unexpected token '{token}', was expecting: '{expectedValue}'");
    }

    public static bool IsExpressionStart(TokenType type)
    {
        return type is TokenType.IntLiteral or TokenType.Identifier or TokenType.LeftParen || UnaryOps.Contains(type);
    }

    public static bool TokenTypeInGroup(TokenType type, Group group)
    {
        return group switch
        {
            Group.DataType when DataTypes.Contains(type) => true,
            Group.LiteralValue when LiteralValues.Contains(type) => true,
            Group.UnaryOp when UnaryOps.Contains(type) => true,
            Group.BinaryOp when BinaryOps.Contains(type) => true,
            Group.ExpressionStart when IsExpressionStart(type) => true,
            _ => false
        };
    }

    public bool NextTokenGroup(Group group)
    {
        return TokenTypeInGroup(Peek().Type, group);
    }

    public bool Empty()
    {
        return _tokens.Count == 0;
    }
}
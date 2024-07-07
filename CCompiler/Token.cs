namespace CCompiler;

public enum TokenType
{
    // Keywords
    IntType,
    Return,

    // TODO - Operators 

    // Punctuation
    LeftParen,
    RightParen,
    LeftBrace,
    RightBrace,
    Semicolon,

    // Literals
    Identifier,
    IntegerLiteral
}

public class Token
{
    private static readonly Dictionary<string, TokenType> TokenMappings = new()
    {
        // Keywords
        { "int", TokenType.IntType },
        { "return", TokenType.Return },

        // Operators

        // Punctuation
        { "(", TokenType.LeftParen },
        { ")", TokenType.RightParen },
        { "{", TokenType.LeftBrace },
        { "}", TokenType.RightBrace },
        { ";", TokenType.Semicolon },
    };

    private static readonly Dictionary<TokenType, string> StringMappings =
        TokenMappings.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

    public static readonly List<TokenType> VariableTypeTokens = new()
    {
        TokenType.IntType,
    };

    public readonly TokenType Type;
    public readonly string Value;
    public readonly int Line;
    public readonly int Column;

    private Token(TokenType type, string value, int line, int column)
    {
        Type = type;
        Value = value;
        Line = line;
        Column = column;
    }

    public static Token CreateToken(string tokenString, int line, int column)
    {
        if (TokenMappings.TryGetValue(tokenString, out var type))
        {
            return new Token(type, tokenString, line, column);
        }

        if (int.TryParse(tokenString, out _))
        {
            return new Token(TokenType.IntegerLiteral, tokenString, line, column);
        }

        return new Token(TokenType.Identifier, tokenString, line, column);
    }

    public static string GetTypeString(TokenType type)
    {
        return type switch
        {
            TokenType.IntegerLiteral => "integer literal",
            TokenType.Identifier => "string literal",
            _ when StringMappings.TryGetValue(type, out var value) => value,
            _ => throw new Exception("Unhandled token type!")
        };
    }

    public override string ToString()
    {
        return Value;
    }
}
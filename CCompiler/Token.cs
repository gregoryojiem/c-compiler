namespace CCompiler;

public enum TokenType
{
    // Keywords
    IntType,
    Return,

    // Operators
    Decrement,
    Increment,
    Complement,
    Negate,
    Plus,
    Multiply,
    Divide,
    Modulo,

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
        { "--", TokenType.Decrement },
        { "++", TokenType.Increment },
        { "~", TokenType.Complement },
        { "-", TokenType.Negate },
        { "+", TokenType.Plus },
        { "*", TokenType.Multiply },
        { "/", TokenType.Divide },
        { "%", TokenType.Modulo },

        // Punctuation
        { "(", TokenType.LeftParen },
        { ")", TokenType.RightParen },
        { "{", TokenType.LeftBrace },
        { "}", TokenType.RightBrace },
        { ";", TokenType.Semicolon },
    };

    private static readonly Dictionary<TokenType, string> StringMappings =
        TokenMappings.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

    public static readonly List<TokenType> DataTypes = new()
    {
        TokenType.IntType
    };

    public static readonly List<TokenType> UnaryOperators = new()
    {
        TokenType.Complement,
        TokenType.Negate
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

        if (ValidIdentifierCheck(tokenString))
        {
            return new Token(TokenType.Identifier, tokenString, line, column);
        }

        throw new SyntaxException(line, column, "Invalid token found: " + tokenString);
    }

    private static bool ValidIdentifierCheck(string tokenString)
    {
        if (!char.IsLetter(tokenString[0]) && tokenString[0] != '_')
        {
            return false;
        }

        return tokenString.All(c => char.IsLetterOrDigit(c) || c.Equals('_'));
    }

    public static string GetTypeString(TokenType expectedType, Token token)
    {
        return expectedType switch
        {
            TokenType.IntegerLiteral => "integer literal",
            TokenType.Identifier => "string literal",
            _ when StringMappings.TryGetValue(expectedType, out var value) => value,
            _ => throw new SyntaxException(token.Line, token.Column, "Unexpected token found!")
        };
    }

    public override string ToString()
    {
        return Value;
    }
}
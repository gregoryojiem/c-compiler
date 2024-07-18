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
    Add,
    Multiply,
    Divide,
    Modulo,

    //Logical operators
    Not,
    And,
    Or,
    Eq,
    Neq,
    Lt,
    Gt,
    LtOrEq,
    GtOrEq,

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
        { "+", TokenType.Add },
        { "*", TokenType.Multiply },
        { "/", TokenType.Divide },
        { "%", TokenType.Modulo },

        // Logical Operators 
        { "!", TokenType.Not },
        { "&&", TokenType.And },
        { "||", TokenType.Or },
        { "==", TokenType.Eq },
        { "!=", TokenType.Neq },
        { "<", TokenType.Lt },
        { ">", TokenType.Gt },
        { "<=", TokenType.LtOrEq },
        { ">=", TokenType.GtOrEq },

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

    public static readonly List<TokenType> UnaryOps = new()
    {
        TokenType.Complement,
        TokenType.Negate,
        TokenType.Not
    };

    public static readonly List<TokenType> BinaryOps = new()
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

        if (ValidIntegerCheck(tokenString))
        {
            return new Token(TokenType.IntegerLiteral, tokenString, line, column);
        }

        if (ValidIdentifierCheck(tokenString))
        {
            return new Token(TokenType.Identifier, tokenString, line, column);
        }

        throw new SyntaxException(line, column, "Invalid token found: " + tokenString);
    }

    public static bool ValidToken(string tokenString)
    {
        return TokenMappings.TryGetValue(tokenString, out _) ||
               ValidIntegerCheck(tokenString) ||
               ValidIdentifierCheck(tokenString);
    }

    private static bool ValidIntegerCheck(string tokenString)
    {
        return int.TryParse(tokenString, out _);
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

    public static int GetPrecedence(TokenType type)
    {
        switch (type)
        {
            case TokenType.Multiply:
            case TokenType.Divide:
            case TokenType.Modulo:
                return 10;
            case TokenType.Add:
            case TokenType.Negate:
                return 9;
            case TokenType.Lt:
            case TokenType.Gt:
            case TokenType.LtOrEq:
            case TokenType.GtOrEq:
                return 8;
            case TokenType.Eq:
            case TokenType.Neq:
                return 7;
            case TokenType.And:
                return 6;
            case TokenType.Or:
                return 5;
            default:
                throw new NotImplementedException();
        }
    }

    public override string ToString()
    {
        return Value;
    }
}
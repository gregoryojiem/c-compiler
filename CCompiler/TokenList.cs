namespace CCompiler;

public class TokenList
{
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

    public Token PopExpected(TokenType type)
    {
        var token = Pop();
        if (token.Type == type) return token;
        var expectedValue = Token.GetTypeString(type, token);
        throw new ParseException(token, $"Unexpected token '{token}', was expecting: '{expectedValue}'");
    }

    public bool Empty()
    {
        return _tokens.Count == 0;
    }
}
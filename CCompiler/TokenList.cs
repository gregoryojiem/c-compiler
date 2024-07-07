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

    public bool PeekExpected(TokenType type)
    {
        if (_tokens.Count == 0)
        {
            throw new SyntaxError(_lastLine, _lastColumn, "Unexpected end of file.");
        }

        return _tokens[0].Type == type;
    }

    public Token Pop()
    {
        if (_tokens.Count == 0)
        {
            throw new SyntaxError(_lastLine, _lastColumn, "Unexpected end of file.");
        }

        var token = _tokens[0];
        _tokens.RemoveAt(0);
        return token;
    }

    //todo replace with enum comparison
    public Token PopExpected(TokenType type)
    {
        var token = Pop();
        if (token.Type != type)
        {
            throw new SyntaxError(token, $"Unexpected token '{token}', was expecting: '{Token.GetTypeString(type)}'");
        }

        return token;
    }

    public bool Empty()
    {
        return _tokens.Count == 0;
    }
}
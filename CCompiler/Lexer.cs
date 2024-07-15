namespace CCompiler;

public class Lexer
{
    private static readonly char[] Delimiters =
        { ' ', '\t', '\n', '/', '(', ')', '{', '}', '[', ']', ',', ';', ':', '~', '-' };

    private readonly string _inputCode;
    private int _currentLine;
    private int _currentColumn;
    private int _positionInCode;

    public Lexer(string inputCode)
    {
        _inputCode = inputCode;
    }

    public TokenList TokenizeCode()
    {
        var tokens = new List<Token>();

        SkipWhitespaceAndComments();
        while (_positionInCode < _inputCode.Length)
        {
            var endOfToken = GetEndOfCurrentToken();
            var tokenLength = endOfToken - _positionInCode;
            var tokenString = _inputCode.Substring(_positionInCode, tokenLength);
            tokens.Add(Token.CreateToken(tokenString, _currentLine + 1, _currentColumn + 1));

            _positionInCode += tokenLength;
            _currentColumn += tokenLength;
            SkipWhitespaceAndComments();
        }

        return new TokenList(tokens, _currentLine + 1, _currentColumn + 1);
    }

    private int GetEndOfCurrentToken()
    {
        var endOfToken = _inputCode.IndexOfAny(Delimiters, _positionInCode);
        if (endOfToken == -1)
            return _inputCode.Length;
        if (endOfToken == _positionInCode)
            return endOfToken + 1;
        return endOfToken;
    }

    private void SkipWhitespaceAndComments()
    {
        var commentStartFound = false;

        while (_positionInCode < _inputCode.Length)
        {
            var currentChar = _inputCode[_positionInCode];
            GoToNextPos(currentChar);

            switch (commentStartFound)
            {
                case true when currentChar is '/':
                    HandleLineComment();
                    commentStartFound = false;
                    continue;
                case true when currentChar is '*':
                    HandleBlockComment();
                    commentStartFound = false;
                    continue;
                case true: //comment wasn't found, go back
                    _positionInCode -= 2;
                    _currentColumn -= 2;
                    return;
            }

            switch (currentChar)
            {
                case ' ':
                case '\t':
                case '\r':
                case '\n':
                case '/':
                    break;
                default:
                    _positionInCode--;
                    _currentColumn--;
                    return;
            }

            commentStartFound = currentChar == '/';
        }
    }

    private void HandleLineComment()
    {
        while (_positionInCode < _inputCode.Length && _inputCode[_positionInCode] != '\n')
        {
            _positionInCode++;
            _currentColumn++;
        }
    }

    private void HandleBlockComment()
    {
        var commentEndFound = false;
        while (_positionInCode < _inputCode.Length)
        {
            var currentChar = _inputCode[_positionInCode];
            GoToNextPos(currentChar);

            if (commentEndFound && currentChar == '/')
            {
                return;
            }

            commentEndFound = currentChar == '*';
        }
    }

    private void GoToNextPos(char currentChar)
    {
        _currentColumn++;
        _positionInCode++;
        if (currentChar != '\n') return;
        _currentColumn = 0;
        _currentLine++;
    }
}
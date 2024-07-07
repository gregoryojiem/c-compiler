namespace CCompiler;

public class Lexer
{
    private static readonly char[] Delimiters = { ' ', '\t', '\n', '/', '(', ')', '{', '}', '[', ']', ',', ';', ':' };
    private int _line;
    private int _column;
    private int _position;
    
    public void Reset()
    {
        _line = 0;
        _column = 0;
        _position = 0;
    }

    public TokenList TokenizeCode(string inputCode)
    {
        var tokens = new List<Token>();

        SkipToNextToken(inputCode);
        while (_position < inputCode.Length)
        {
            var endOfToken = inputCode.IndexOfAny(Delimiters, _position);
            endOfToken = endOfToken == -1 ? inputCode.Length : endOfToken;
            endOfToken = endOfToken == _position ? endOfToken + 1 : endOfToken;
            var tokenLength = endOfToken - _position;
            var tokenString = inputCode.Substring(_position, tokenLength);
            tokens.Add(Token.CreateToken(tokenString, _line, _column));
            
            _position += tokenLength;
            _column += tokenLength;
            SkipToNextToken(inputCode);
        }

        return new TokenList(tokens, _line, _column);
    }

    private void SkipToNextToken(string inputCode)
    {
        while (_position < inputCode.Length)
        {
            var currentChar = inputCode[_position];
            switch (currentChar)
            {
                case ' ':
                case '\t':
                    _column++;
                    _position++;
                    break;
                case '\n':
                    _column = 0;
                    _line++;
                    _position++;
                    break;
                case '/':
                    break;
                default:
                    return;
            }
        }
    }
}
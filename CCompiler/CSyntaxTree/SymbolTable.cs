namespace CCompiler.CSyntaxTree;

public class SymbolTable
{
    private readonly Stack<Dictionary<string, string>> _outerScopes;
    private Dictionary<string, string> _currentScope;
    private bool mergeNextScope;

    public SymbolTable()
    {
        _outerScopes = new Stack<Dictionary<string, string>>();
        _currentScope = new Dictionary<string, string>();
        mergeNextScope = false;
    }

    public void MergeNextScope()
    {
        mergeNextScope = true;
    }

    public void NewScope()
    {
        if (mergeNextScope)
        {
            mergeNextScope = false;
            return;
        }

        _outerScopes.Push(_currentScope);
        _currentScope = new Dictionary<string, string>();
    }

    public void ExitScope()
    {
        _currentScope = _outerScopes.Pop();
    }

    public void AddVariable(string variableName, string uniqueName)
    {
        _currentScope.Add(variableName, uniqueName);
    }

    public string GetUniqueId(Token variable)
    {
        _currentScope.TryGetValue(variable.Value, out var innerId);
        if (innerId != null)
            return innerId;
        var outerId = GetUniqueIdOuterScope(variable.Value);
        if (outerId != null)
            return outerId;
        throw new SemanticException(variable, "The variable '" + variable + "' has not been declared");
    }

    private string? GetUniqueIdOuterScope(string variableName)
    {
        foreach (var scope in _outerScopes)
        {
            scope.TryGetValue(variableName, out var uniqueName);
            if (uniqueName != null)
                return uniqueName;
        }

        return null;
    }

    public bool ExistsInCurrentScope(string identifier)
    {
        return _currentScope.ContainsKey(identifier);
    }
}
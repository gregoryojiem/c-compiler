namespace CCompiler.CSyntaxTree;

public class SymbolTable
{
    public static int VariableId;
    public static int LabelId;

    private readonly Stack<Dictionary<string, string>> _outerScopeVariables;
    private Dictionary<string, string> _currentVariables;
    private bool _mergeNextScope;

    public SymbolTable()
    {
        _outerScopeVariables = new Stack<Dictionary<string, string>>();
        _currentVariables = new Dictionary<string, string>();
        _mergeNextScope = false;
    }

    public void MergeNextScope()
    {
        _mergeNextScope = true;
    }

    public void NewScope()
    {
        if (_mergeNextScope)
        {
            _mergeNextScope = false;
            return;
        }

        _outerScopeVariables.Push(_currentVariables);
        _currentVariables = new Dictionary<string, string>();
    }

    public void ExitScope()
    {
        _currentVariables = _outerScopeVariables.Pop();
    }

    public void AddVariable(string variableName, string uniqueName)
    {
        _currentVariables.Add(variableName, uniqueName);
    }

    public string GetUniqueId(Token variable)
    {
        _currentVariables.TryGetValue(variable.Value, out var innerId);
        if (innerId != null)
            return innerId;
        var outerId = GetUniqueIdOuterScope(variable.Value);
        if (outerId != null)
            return outerId;
        throw new SemanticException(variable, "The variable '" + variable + "' has not been declared");
    }

    private string? GetUniqueIdOuterScope(string variableName)
    {
        foreach (var scope in _outerScopeVariables)
        {
            scope.TryGetValue(variableName, out var uniqueName);
            if (uniqueName != null)
                return uniqueName;
        }

        return null;
    }

    public bool ExistsInCurrentScope(string identifier)
    {
        return _currentVariables.ContainsKey(identifier);
    }
}
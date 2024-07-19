using CCompiler.CSyntaxTree.Statements;

namespace CCompiler.CSyntaxTree.TacStatements;

public abstract class TacStatementNode : StatementNode
{
    public override void SemanticPass(Dictionary<string, string> variableMap)
    {
    }

    public override void ConvertToTac(List<StatementNode> statementList)
    {
    }
}
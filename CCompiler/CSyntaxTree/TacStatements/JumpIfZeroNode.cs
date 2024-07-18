using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.TacStatements;

public class JumpIfZeroNode : TacStatementNode
{
    private BaseValueNode _condition;
    private string _target;
    private bool _inverted;

    public JumpIfZeroNode(BaseValueNode condition, string target, bool inverted)
    {
        _condition = condition;
        _target = target;
        _inverted = inverted;
    }

    public override string ToString()
    {
        var condition = _inverted ? "" : "!";
        return "if (" + condition + _condition + ")" + " goto " + _target;
    }
}
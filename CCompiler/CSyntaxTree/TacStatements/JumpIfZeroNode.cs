using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.TacStatements;

public class JumpIfZeroNode : TacStatementNode
{
    public readonly BaseValueNode Condition;
    public readonly string Target;
    public readonly bool Inverted;

    public JumpIfZeroNode(BaseValueNode condition, string target, bool inverted)
    {
        Condition = condition;
        Target = target;
        Inverted = inverted;
    }

    public override string ToString()
    {
        var condition = Inverted ? "" : "!";
        return "if (" + condition + Condition + ")" + " goto " + Target;
    }
}
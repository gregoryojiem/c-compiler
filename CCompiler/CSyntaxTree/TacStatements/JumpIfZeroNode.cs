using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;

namespace CCompiler.CSyntaxTree.TacStatements;

public class JumpIfZeroNode : TacStatementNode
{
    public readonly ValueNode Condition;
    public readonly string Target;
    public readonly bool Inverted;

    public JumpIfZeroNode(ValueNode condition, string target, bool inverted)
    {
        Condition = condition;
        Target = target;
        Inverted = inverted;
    }

    public override string ToString()
    {
        var condition = Inverted ? "" : "!";
        return "if (" + condition + Condition + ")" + "\n\t\tgoto " + Target.Replace(".", "_") + ";";
    }
}
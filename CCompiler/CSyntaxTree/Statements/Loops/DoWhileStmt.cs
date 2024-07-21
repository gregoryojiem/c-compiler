using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.Statements.Loops;

public class DoWhileStmt : LoopStmt
{
    private readonly StatementNode _body;
    private readonly ExpressionNode _condition;

    public DoWhileStmt(TokenList tokens)
    {
        tokens.PopExpected(TokenType.Do);
        _body = ParseStatementNode(tokens);
        tokens.PopExpected(TokenType.While);
        tokens.PopExpected(TokenType.LeftParen);
        _condition = ExpressionNode.ParseExpression(tokens);
        tokens.PopExpected(TokenType.RightParen);
        tokens.PopExpected(TokenType.Semicolon);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
        symbolTable.EnterLoop("do_while_loop");
        SetLabel(symbolTable);
        _body.SemanticPass(symbolTable);
        _condition.VariableResolution(symbolTable);
        symbolTable.ExitLoop();
    }

    public override void ConvertToTac(List<BlockItem> blockItems)
    {
        var startLabel = new LabelNode("start_" + GetLabel());
        var continueLabel = new LabelNode("continue_" + GetLabel());
        var breakLabel = new LabelNode("break_" + GetLabel());

        blockItems.Add(startLabel);
        _body.ConvertToTac(blockItems);
        blockItems.Add(continueLabel);
        var tacCondition = (BaseValueNode)_condition.ConvertToTac(blockItems);
        blockItems.Add(new JumpIfZeroNode(tacCondition, startLabel.Identifier, true));
        blockItems.Add(breakLabel);
    }

    public override string ToString()
    {
        var indent = BlockNode.GetIndent(_body);
        BlockNode.IncreaseIndent(_body);
        var output = "do" + indent + _body + " while (" + _condition + ");";
        BlockNode.DecreaseIndent(_body);
        return output;
    }
}
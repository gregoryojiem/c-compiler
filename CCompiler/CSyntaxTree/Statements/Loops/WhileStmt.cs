using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.Statements.Loops;

public class WhileStmt : LoopStmt
{
    private readonly ExpressionNode _condition;
    private readonly StatementNode _body;

    public WhileStmt(TokenList tokens)
    {
        tokens.PopExpected(TokenType.While);
        tokens.PopExpected(TokenType.LeftParen);
        _condition = ExpressionNode.ParseExpression(tokens);
        tokens.PopExpected(TokenType.RightParen);
        _body = ParseStatementNode(tokens);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
        symbolTable.EnterLoop("while_loop");
        SetLabel(symbolTable);
        _condition.VariableResolution(symbolTable);
        _body.SemanticPass(symbolTable);
        symbolTable.ExitLoop();
    }

    public override void ConvertToTac(List<StatementNode> statementList)
    {
        var continueLabel = new LabelNode("continue_" + GetLabel());
        var breakLabel = new LabelNode("break_" + GetLabel());

        statementList.Add(continueLabel);
        var tacCondition = (BaseValueNode)_condition.ConvertToTac(statementList);
        statementList.Add(new JumpIfZeroNode(tacCondition, breakLabel.Identifier, false));
        _body.ConvertToTac(statementList);
        statementList.Add(new JumpNode(continueLabel.Identifier));
        statementList.Add(breakLabel);
    }

    public override string ToString()
    {
        var indent = BlockNode.GetIndent(_body);
        BlockNode.IncreaseIndent(_body);
        var output = "while (" + _condition + ")" + indent + _body;
        BlockNode.DecreaseIndent(_body);
        return output;
    }
}
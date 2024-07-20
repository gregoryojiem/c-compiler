using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.Statements;

public class IfStmt : StatementNode
{
    private readonly ExpressionNode _condition;
    private readonly StatementNode _thenStmt;
    private readonly StatementNode? _elseStmt;

    public IfStmt(TokenList tokens)
    {
        tokens.PopExpected(TokenType.If);
        tokens.PopExpected(TokenType.LeftParen);
        _condition = ExpressionNode.ParseExpression(tokens);
        tokens.PopExpected(TokenType.RightParen);
        _thenStmt = ParseStatementNode(tokens);

        if (tokens.PopIfFound(TokenType.Else))
            _elseStmt = ParseStatementNode(tokens);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
        _condition.VariableResolution(symbolTable);
        if (_thenStmt is DeclarationStmt declarationStmt)
        {
            throw new SemanticException(declarationStmt.Identifier,
                $"Cannot make declaration {declarationStmt} in non-scoped if statement");
        }

        _thenStmt.SemanticPass(symbolTable);
        _elseStmt?.SemanticPass(symbolTable);
    }

    public override void ConvertToTac(List<StatementNode> statementList)
    {
        var handleElse = _elseStmt != null;
        var elseId = handleElse ? ExpressionNode.UniqueLabelCounter++ : 0;
        var ifId = ExpressionNode.UniqueLabelCounter++;
        var elseStartLabel = "else_start_" + elseId;
        var ifEndLabel = "if_end_" + ifId;
        var ifLabelToUse = handleElse ? elseStartLabel : ifEndLabel;

        var tacCondition = (BaseValueNode)_condition.ConvertToTac(statementList);
        statementList.Add(new JumpIfZeroNode(tacCondition, ifLabelToUse, false));
        _thenStmt.ConvertToTac(statementList);

        if (_elseStmt != null)
        {
            statementList.Add(new JumpNode(ifEndLabel));
            statementList.Add(new LabelNode(elseStartLabel));
            _elseStmt.ConvertToTac(statementList);
        }

        statementList.Add(new LabelNode(ifEndLabel));
    }

    public override string ToString()
    {
        return "if " + _condition + " " + _thenStmt + " else " + _elseStmt;
    }
}
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
        var elseId = handleElse ? SymbolTable.LabelId++ : 0;
        var ifId = SymbolTable.LabelId++;
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
        var thenIndent = BlockNode.GetIndent(_thenStmt);
        var elseIndent = BlockNode.GetIndent(_elseStmt);

        BlockNode.IncreaseIndent(_thenStmt);
        var output = "if " + _condition + thenIndent + _thenStmt + "\n";
        BlockNode.DecreaseIndent(_thenStmt);

        if (_elseStmt == null)
            return output;

        var elseStartIndent = BlockNode.GetIndent(this, false, -1);
        BlockNode.IncreaseIndent(_elseStmt);
        output += elseStartIndent + "else " + elseIndent + _elseStmt;
        BlockNode.DecreaseIndent(_elseStmt);
        return output;
    }
}
using CCompiler.CSyntaxTree.Expressions;
using CCompiler.CSyntaxTree.TacExpressions.BaseNodes;
using CCompiler.CSyntaxTree.TacStatements;

namespace CCompiler.CSyntaxTree.Statements.Loops;

public class ForStmt : LoopStmt
{
    private readonly BlockItem _initialClause;
    private readonly ExpressionNode? _condition;
    private readonly ExpressionNode? _post;
    private readonly StatementNode _body;

    public ForStmt(TokenList tokens)
    {
        tokens.PopExpected(TokenType.For);
        tokens.PopExpected(TokenType.LeftParen);
        _initialClause = ParseBlockItem(tokens);
        if (_initialClause is not (DeclarationNode or ExpressionStmt or NullStmt))
        {
            throw new ParseException(tokens.Peek(), $"Invalid for loop initializer {tokens.Peek()}," +
                                                    " expected expression, declaration, or semicolon");
        }

        if (tokens.Peek().Type != TokenType.Semicolon)
        {
            _condition = ExpressionNode.ParseExpression(tokens);
        }

        tokens.PopExpected(TokenType.Semicolon);
        if (tokens.Peek().Type != TokenType.RightParen)
        {
            _post = ExpressionNode.ParseExpression(tokens);
        }

        tokens.PopExpected(TokenType.RightParen);
        _body = ParseStatementNode(tokens);
    }

    public override void SemanticPass(SymbolTable symbolTable)
    {
        symbolTable.EnterLoop("for_loop");
        SetLabel(symbolTable);
        symbolTable.NewScope();

        _initialClause.SemanticPass(symbolTable);
        _condition?.VariableResolution(symbolTable);
        _post?.VariableResolution(symbolTable);
        _body.SemanticPass(symbolTable);
        symbolTable.ExitScope();
        symbolTable.ExitLoop();
    }

    public override void ConvertToTac(List<BlockItem> blockItems)
    {
        var startLabel = new LabelNode("start_" + GetLabel());
        var continueLabel = new LabelNode("continue_" + GetLabel());
        var breakLabel = new LabelNode("break_" + GetLabel());

        _initialClause.ConvertToTac(blockItems);
        blockItems.Add(startLabel);
        if (_condition != null)
        {
            var tacCondition = (ValueNode)_condition.ConvertToTac(blockItems);
            blockItems.Add(new JumpIfZeroNode(tacCondition, breakLabel.Identifier, false));
        }

        _body.ConvertToTac(blockItems);
        blockItems.Add(continueLabel);
        _post?.ConvertToTac(blockItems);
        blockItems.Add(new JumpNode(startLabel.Identifier));
        blockItems.Add(breakLabel);
    }

    public override string ToString()
    {
        var indent = BlockNode.GetIndent(_body);
        BlockNode.IncreaseIndent(_body);
        var output = "for (" + _initialClause + " " + _condition + "; " + _post + ")" + indent + _body;
        BlockNode.DecreaseIndent(_body);
        return output;
    }
}
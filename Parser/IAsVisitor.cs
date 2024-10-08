
namespace Gwent__.Parser
{
	public interface IAstVisitor<T>
	{
		//KEYWORDS
		T VisitEffectDefinition(EffectDefinitionNode node);
		T VisitNameDefinition(NameDefinitionNode node);
		T VisitParamsDefinition(ParamsDefinitionNode node);
		T VisitParamList(ParamListNode node);
		T VisitParamDefinition(ParamDefinitionNode node);
		T VisitActionNode(ActionDefinitionNode node);
		T VisitFunctionDefinition(FunctionDefinitionNode node);
		
		//STATEMENTS
		T VisitBlockStatement(BlockStatementNode node);
		T VisitWhileStatement(WhileStatementNode node);
		T VisitForStatement(ForStatementNode node);
		T VisitAssignment(AssignmentStatementNode node);
		T VisitIfStatement(IfStatementNode ifStatementNode);
		//EXPRESSIONS
		T VisitNumberLiteral(NumberLiteralNode node);
		T VisitString(StringLiteralNode node);
		T VisitVariableDeclaration(VariableDeclarationNode node);
		//FUNCTION CALL	
		T VisitFunctionCall(FunctionCallNode node);
        T VisitArgument(ArgumentNode node);
        T VisitBinaryExpression(BinaryExpressionNode node);
        T VisitUnaryExpression(UnaryExpressionNode unaryExpressionNode);
    }
}
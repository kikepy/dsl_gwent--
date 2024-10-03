using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
		
		//EXPRESSIONS
		T VisitNumberLiteral(NumberLiteralNode node);
		
	}
}
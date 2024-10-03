using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Gwent__.Parser
{
	public enum NodeType
	{
		Program,
		EffectDefinition,
		NameDefinition,
		ParamsDefinition,
		ParamList,
		ParamDefinition,
		ActionDefinition,
		FunctionDefinition,
		
		//STATEMENTS
		StatementList,
		AssignmentStatement,
		WhileStatement,
		ForStatement,
		IfStatement,
		
		//EXPRESSIONS
		BinaryExpression,
		NumberLiteral,
		StringLiteral,
		Variable,
		UnaryExpression,
		
		//FUNCTION CALLS
		FunctionCall,
		
	}
	public abstract class AstNode
	{
		public NodeType Type { get; protected set; }
		public int Line { get; set; }
		public int Column { get; set; }
		
		protected AstNode(int lineNumber, int columnNumber, NodeType nodeType)
		{
			Line = lineNumber;
			Column = columnNumber;
			Type = nodeType;
		}
		
		public abstract T Accept<T>(IAstVisitor<T> visitor);
	}
	
	public class EffectDefinitionNode : AstNode
	{
		public NameDefinitionNode Name { get; set; }
		public ParamsDefinitionNode Params { get; set; }
		public ActionDefinitionNode Action { get; set; }
		
		public EffectDefinitionNode(int line, int column, NameDefinitionNode name, ParamsDefinitionNode _params, ActionDefinitionNode action) : base(line, column, NodeType.EffectDefinition)
		{
			Name = name;
			Params = _params;
			Action = action;
		}

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitEffectDefinition(this);
		}
	}
	
	public class NameDefinitionNode : AstNode
	{
		public string Value { get; set; }
		
		public NameDefinitionNode(int line, int column, string value) : base (line, column, NodeType.NameDefinition)
		{
			Value = value;
		}

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitNameDefinition(this);
		}
	}
	
	public class ParamsDefinitionNode : AstNode
	{
		public ParamListNode ParamList { get; set;}
		
		public ParamsDefinitionNode(int line, int column, ParamListNode paramList) : base(line, column, NodeType.ParamsDefinition)
		{
			ParamList = paramList;
		}

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitParamsDefinition(this);
		}
	}
	
	public class ParamListNode : AstNode
	{
		public List<ParamDefinitionNode> Params { get; set;}
		
		public ParamListNode(int line, int column) : base(line, column, NodeType.ParamList){ }

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitParamList(this);
		}
	}
	
	public class ParamDefinitionNode : AstNode
	{
		public string Identifier { get; set; }
		public string Type { get; set; }
		
		public ParamDefinitionNode(int line, int column, string identifier, string type) : base(line, column, NodeType.ParamDefinition)
		{
			Identifier = identifier;
			Type = type;
		}

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitParamDefinition(this);
		}
	}
	
	public class ActionDefinitionNode : AstNode
	{
		public FunctionDefinitionNode FunctionDefinition { get; set;}

		public ActionDefinitionNode(int line, int column, FunctionDefinitionNode functionDefinition) : base(line, column, NodeType.ActionDefinition)
		{
			FunctionDefinition = functionDefinition;
		}

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitActionNode(this);
		}
	}
	
	public class FunctionDefinitionNode : AstNode
	{
		public ParamListNode ParamList { get; set; }
		public StatementListNode StatementList { get; set; }
		
		public FunctionDefinitionNode(int line, int column, ParamListNode paramList, StatementListNode statementList) : base(line, column, NodeType.FunctionDefinition)
		{
			ParamList = paramList;
			StatementList = statementList;
		}
        public override T Accept<T>(IAstVisitor<T> visitor)
        {
            return visitor.VisitFunctionDefinition(this);
        }
    }
	/*--------STATEMENTS----------------*/
	
	
	
	/*--------EXPRESSIONS---------------*/
	public class NumberLiteralNode : AstNode
	{
		public int Value { get; set;}
		public NumberLiteralNode(int line, int column, int value) : base(line, column, NodeType.NumberLiteral)
		{
			Value = value;
		}

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitNumberLiteral(this);
		}
	}
}
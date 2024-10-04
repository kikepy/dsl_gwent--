using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Gwent__.Lexer;

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
		Statement,
		//EXPRESSIONS
		BinaryExpression,
		NumberLiteral,
		StringLiteral,
		Variable,
		UnaryExpression,
		Argument,
		//FUNCTION CALLS
		FunctionCall,
		
	}
	
	public enum StatementType
	{
		AssignmentStatement,
		WhileStatement,
		ForStatement,
		IfStatement,
	}
	public abstract class AstNode
	{
		public NodeType Type { get; set; }
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
		public NameDefinitionNode Name { get; protected set; }
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
		public ParamListNode ParamList { get; set; }
		
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
		public List<ParamDefinitionNode>? Params { get; set; }
		
		public ParamListNode(int line, int column) : base(line, column, NodeType.ParamList){ }

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitParamList(this);
		}
	}
	
	public class ParamDefinitionNode : AstNode
	{
		public string Identifier { get; set; }
		public new string Type { get; set; }
		
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
		public FunctionDefinitionNode FunctionDefinition { get; set; }

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
	public class StatementListNode : AstNode
	{
		public StatementNode StatementNode { get; set; } 
		
		public StatementListNode(int line, int column, StatementNode statementNode) : base(line, column, NodeType.StatementList)
		{
			StatementNode = statementNode;
		}
		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitStatement(this);
		}
	}
	
	public abstract class StatementNode : AstNode
	{
		public new StatementType Type { get; set; }
		public new int Line { get; set; }
		public new int Column { get; set; }
		public StatementNode(int line, int column, StatementType type) : base(line, column, NodeType.Statement)
		{
			Line = line;
			Column = column;
			Type = type;
		}
		public abstract override T Accept<T>(IAstVisitor<T> visitor);
	}
	
	public class WhileStatementNode : StatementNode
	{
		public WhileStatementNode(int line, int column) : base(line, column, StatementType.WhileStatement){ }

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitWhileStatement(this);
		}
	}
	
	public class ForStatementNode : StatementNode
	{
		public ForStatementNode(int line, int column) : base(line, column, StatementType.ForStatement){ }

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitForStatement(this);
		}
	}
	
	public class IfStatementNode : StatementNode
	{
		public IfStatementNode(int line, int column) : base(line, column, StatementType.IfStatement){ }

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitIfStatement(this);
		}
	}
	
	public class AssignmentStatementNode : StatementNode
	{
		public AssignmentStatementNode(int line, int column) : base(line, column, StatementType.AssignmentStatement){ }

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitAssignment(this);
		}
	}
	
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
	
	public class StringLiteralNode : AstNode
	{
		public string? Value { get; set;}
		
		public StringLiteralNode(int line, int column, string value) : base(line, column, NodeType.StringLiteral)
		{
			Value = value;
		}
		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitString(this);
		}
	}
	
	public class VariableNode : AstNode
	{
		public VariableValue Value { get; set; }
		public VariableNode(int line, int column, VariableValue value) : base(line, column, NodeType.Variable) 
		{
			Value = value;
		}
		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitVariable(this);
		}
	}
	
	public class VariableValue
	{
		public string? StringValue { get; }
		public int? IntValue { get; }
		public float? FLoatValue { get; }
		public VariableValue(int numberValue, string stringValue, float floatValue)
		{
			IntValue = numberValue;
			StringValue = stringValue;
			FLoatValue = floatValue;
		}
	}
	
	//FUNCTION CALL
	public class FunctionCallNode : AstNode
	{
		public string Identifier { get; set; }
		public List<ArgumentNode> Arguments { get; set;}
		
		public FunctionCallNode(int line, int column, string identifier, List<ArgumentNode> arguments) 
		: base(line, column, NodeType.FunctionCall)
		{
			Identifier = identifier;
			Arguments = arguments;
		}

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitFunctionCall(this);
		}
	}
	
	public class ArgumentNode : AstNode 
	{
		public ExpressionNode Expression { get; set;}
		
		public ArgumentNode(int line, int column, ExpressionNode expression)
		: base(line, column, NodeType.Argument)
		{
			Expression = expression;
		}

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitArgument(this);
		}
	}
	
	public abstract class ExpressionNode : AstNode
	{
		public ExpressionNode(int line, int column, NodeType nodeType)
		: base(line, column, nodeType)
		{	
		}

		public abstract override T Accept<T>(IAstVisitor<T> visitor);
	}
	
	public class BinaryExpressionNode : AstNode
	{
		public ExpressionNode Left { get; set;}
		public string Operator { get; set; }
		public ExpressionNode Right { get; set;}
		
		public BinaryExpressionNode(int line, int column, ExpressionNode left, string _operator, ExpressionNode right)
		: base(line, column, NodeType.BinaryExpression)
		{
			Left = left;
			Operator = _operator;
			Right = right;
		}

        public override T Accept<T>(IAstVisitor<T> visitor)
        {
            return visitor.VisitBinaryExpression(this);	
        }
    }
}
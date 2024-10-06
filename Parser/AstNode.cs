
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
	public abstract class AstNode(int lineNumber, int columnNumber, NodeType nodeType)
	{
		public NodeType Type { get; set; } = nodeType;
		public int Line { get; set; } = lineNumber;
		public int Column { get; set; } = columnNumber;

		public abstract T Accept<T>(IAstVisitor<T> visitor);
	}
	
	public class EffectDefinitionNode(
		int line,
		int column,
		NameDefinitionNode name,
		ParamsDefinitionNode @params,
		ActionDefinitionNode action)
		: AstNode(line, column, NodeType.EffectDefinition)
	{
		public NameDefinitionNode Name { get; protected set; } = name;
		public ParamsDefinitionNode Params { get; set; } = @params;
		public ActionDefinitionNode Action { get; set; } = action;

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitEffectDefinition(this);
		}
	}
	
	public class NameDefinitionNode(int line, int column, string value) : AstNode(line, column, NodeType.NameDefinition)
	{
		public string Value { get; set; } = value;

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitNameDefinition(this);
		}
	}
	
	public class ParamsDefinitionNode(int line, int column, ParamListNode paramList)
		: AstNode(line, column, NodeType.ParamsDefinition)
	{
		public ParamListNode ParamList { get; set; } = paramList;

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitParamsDefinition(this);
		}
	}
	
	public class ParamListNode(int line, int column) : AstNode(line, column, NodeType.ParamList)
	{
		public List<ParamDefinitionNode>? Params { get; set; }

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitParamList(this);
		}
	}
	
	public class ParamDefinitionNode(int line, int column, string identifier, string type)
		: AstNode(line, column, NodeType.ParamDefinition)
	{
		public string Identifier { get; set; } = identifier;
		public new string Type { get; set; } = type;

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitParamDefinition(this);
		}
	}
	
	public class ActionDefinitionNode(int line, int column, FunctionDefinitionNode functionDefinition)
		: AstNode(line, column, NodeType.ActionDefinition)
	{
		public FunctionDefinitionNode FunctionDefinition { get; set; } = functionDefinition;

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitActionNode(this);
		}
	}
	
	public class FunctionDefinitionNode(int line, int column, ParamListNode paramList, StatementListNode statementList)
		: AstNode(line, column, NodeType.FunctionDefinition)
	{
		public ParamListNode ParamList { get; set; } = paramList;
		public StatementListNode StatementList { get; set; } = statementList;

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitFunctionDefinition(this);
		}
	}
	
	/*--------STATEMENTS----------------*/
	public class StatementListNode(int line, int column, List<StatementNode> statementNode)
		: AstNode(line, column, NodeType.StatementList)
	{
		public List<StatementNode> StatementNode { get; set; } = statementNode;

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitStatement(this);
		}
	}
	
	public abstract class StatementNode(int line, int column, StatementType type)
		: AstNode(line, column, NodeType.Statement)
	{
		public new StatementType Type { get; set; } = type;
		public new int Line { get; set; } = line;
		public new int Column { get; set; } = column;

		public abstract override T Accept<T>(IAstVisitor<T> visitor);
	}
	
	public class WhileStatementNode(int line, int column) : StatementNode(line, column, StatementType.WhileStatement)
	{
		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitWhileStatement(this);
		}
	}
	
	public class ForStatementNode(int line, int column) : StatementNode(line, column, StatementType.ForStatement)
	{
		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitForStatement(this);
		}
	}
	
	public class IfStatementNode(int line, int column) : StatementNode(line, column, StatementType.IfStatement)
	{
		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitIfStatement(this);
		}
	}
	
	public class AssignmentStatementNode(int line, int column)
		: StatementNode(line, column, StatementType.AssignmentStatement)
	{
		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitAssignment(this);
		}
	}
	
	/*--------EXPRESSIONS---------------*/
	public class NumberLiteralNode(int line, int column, int value) : AstNode(line, column, NodeType.NumberLiteral)
	{
		public int Value { get; set;} = value;

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitNumberLiteral(this);
		}
	}
	
	public class StringLiteralNode(int line, int column, string value) : AstNode(line, column, NodeType.StringLiteral)
	{
		public string? Value { get; set;} = value;

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitString(this);
		}
	}
	
	public class VariableNode(int line, int column, VariableValue value) : AstNode(line, column, NodeType.Variable)
	{
		public VariableValue Value { get; set; } = value;

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitVariable(this);
		}
	}
	
	public class VariableValue(int line, int column, object value) : AstNode(line, column, NodeType.Variable)
	{
		public object Value { get; set; } = value;

		public override T Accept<T>(IAstVisitor<T> visitor)
        {
            return visitor.VisitVariableValue(this);
        }
    }
	
	//FUNCTION CALL
	public class FunctionCallNode(int line, int column, string identifier, List<ArgumentNode> arguments)
		: AstNode(line, column, NodeType.FunctionCall)
	{
		public string Identifier { get; set; } = identifier;
		public List<ArgumentNode> Arguments { get; set;} = arguments;

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitFunctionCall(this);
		}
	}
	
	public class ArgumentNode(int line, int column, ExpressionNode expression)
		: AstNode(line, column, NodeType.Argument)
	{
		public ExpressionNode Expression { get; set;} = expression;

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitArgument(this);
		}
	}
	
	public abstract class ExpressionNode : AstNode
	{
		protected ExpressionNode(int line, int column, NodeType nodeType)
		: base(line, column, nodeType)
		{	
		}

		public abstract override T Accept<T>(IAstVisitor<T> visitor);
	}
	
	public class BinaryExpressionNode(int line, int column, ExpressionNode left, string @operator, ExpressionNode right)
		: AstNode(line, column, NodeType.BinaryExpression)
	{
		public ExpressionNode Left { get; set;} = left;
		public string Operator { get; set; } = @operator;
		public ExpressionNode Right { get; set;} = right;

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitBinaryExpression(this);	
		}
	}
	
	public class UnaryExpressionNode(int line, int column, ExpressionNode operand, string @operator)
		: AstNode(line, column, NodeType.UnaryExpression)
	{
		public ExpressionNode Operand { get; set;} = operand;
		public string Operator { get; set; } = @operator;

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitUnaryExpression(this);
		}
	}
}
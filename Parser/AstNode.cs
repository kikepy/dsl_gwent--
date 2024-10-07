
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
		VariableDeclaration,
		UnaryExpression,
		Argument,
		//FUNCTION CALLS
		FunctionCall,
		Expression,
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
		protected AstNode(int line, int column, NodeType nodeType)
		{
			Type = nodeType;
			Line = line;
			Column = column;
		}

		public NodeType Type { get; set; }
		public int Line { get; set; }
		public int Column { get; set; }

		public abstract T Accept<T>(IAstVisitor<T> visitor);
		
		public virtual IEnumerable<AstNode> GetChildren()
		{
			return [];
		}
	}
	
	public class EffectDefinitionNode : AstNode
	{
		public EffectDefinitionNode(int line,
			int column,
			NameDefinitionNode name,
			ParamsDefinitionNode @params,
			ActionDefinitionNode action) : base(line, column, NodeType.EffectDefinition)
		{
			Name = name;
			Params = @params;
			Action = action;
		}

		private NameDefinitionNode Name { get; set; }
		private ParamsDefinitionNode Params { get; set; }
		private ActionDefinitionNode Action { get; set; }

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitEffectDefinition(this);
		}

		public override IEnumerable<AstNode> GetChildren()
		{
			return [Name, Params, Action];
		}
	}
	
	public class NameDefinitionNode : AstNode
	{
		public NameDefinitionNode(int line, int column, string value) : base(line, column, NodeType.NameDefinition)
		{
			Value = value;
		}

		public string Value { get; set; }

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitNameDefinition(this);
		}
		public override IEnumerable<AstNode> GetChildren()
		{
			return [];
		}
	}
	
	public class ParamsDefinitionNode : AstNode
	{
		public ParamsDefinitionNode(int line, int column, ParamListNode paramList) : base(line, column, NodeType.ParamsDefinition)
		{
			ParamList = paramList;
		}

		private ParamListNode ParamList { get; set; }

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitParamsDefinition(this);
		}

		public override IEnumerable<AstNode> GetChildren()
		{
			return [ParamList];
		}
	}
	
	public class ParamListNode : AstNode
	{
		public ParamListNode(int line, int column) : base(line, column, NodeType.ParamList)
		{
		}

		public List<ParamDefinitionNode>? Params { get; set; }

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitParamList(this);
		}

		public override IEnumerable<AstNode> GetChildren()
		{
			return Params;
		}
	}
	
	public class ParamDefinitionNode : AstNode
	{
		public ParamDefinitionNode(int line, int column, string identifier, string type) 
			: base(line, column, NodeType.ParamDefinition)
		{
			Identifier = identifier;
			Type = type;
		}

		public string Identifier { get; set; }
		public new string Type { get; set; }

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitParamDefinition(this);
		}

		public override IEnumerable<AstNode> GetChildren()
		{
			return [];
		}
	}
	
	public class ActionDefinitionNode : AstNode
	{
		public ActionDefinitionNode(int line, int column, FunctionDefinitionNode functionDefinition) : base(line, column, NodeType.ActionDefinition)
		{
			FunctionDefinition = functionDefinition;
		}

		private FunctionDefinitionNode FunctionDefinition { get; set; }

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitActionNode(this);
		}

		public override IEnumerable<AstNode> GetChildren()
		{
			return [FunctionDefinition];
		}
	}
	
	public class FunctionDefinitionNode : AstNode
	{
		public FunctionDefinitionNode(int line, int column, ParamListNode paramList, BlockStatementNode statementList) : base(line, column, NodeType.FunctionDefinition)
		{
			ParamList = paramList;
			StatementList = statementList;
		}

		private ParamListNode ParamList { get; set; }
		private BlockStatementNode StatementList { get; set; }

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitFunctionDefinition(this);
		}

		public override IEnumerable<AstNode> GetChildren()
		{
			return [ParamList, StatementList];
		}
	}
	
	/*--------STATEMENTS----------------*/
	public class BlockStatementNode : AstNode
	{
		public BlockStatementNode(int line, int column, List<StatementNode> statements) : base(line, column, NodeType.StatementList)
		{
			Statements = statements;
		}

		private List<StatementNode> Statements { get; set; }

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitBlockStatement(this);
		}

		public override IEnumerable<AstNode> GetChildren()
		{
			return Statements;
		}
	}
	
	public abstract class StatementNode : AstNode
	{
		protected StatementNode(int line, int column, StatementType type) : base(line, column, NodeType.Statement)
		{
			Type = type;
			Line = line;
			Column = column;
		}

		public new StatementType Type { get; set; }
		public new int Line { get; set; }
		public new int Column { get; set; }

		public abstract override T Accept<T>(IAstVisitor<T> visitor);

		public override IEnumerable<AstNode> GetChildren()
		{
			return [];
		}
	}
	
	public class WhileStatementNode : StatementNode
	{
		public ExpressionNode Condition { get; set; }
		public BlockStatementNode Body { get; set; }
		public WhileStatementNode(int line, int column, ExpressionNode condition, BlockStatementNode body) 
		: base(line, column, StatementType.WhileStatement)
		{
			Condition = condition;
			Body = body;
		}

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitWhileStatement(this);
		}

		public override IEnumerable<AstNode> GetChildren()
		{
			return [Condition, Body];
		}
	}
	
	public class ForStatementNode : StatementNode
	{
		public ForStatementNode(int line, int column, 
			ExpressionNode initialization, 
			ExpressionNode condition, 
			ExpressionNode increment ,
			BlockStatementNode body) 
		: base(line, column, StatementType.ForStatement)
		{
			Initialization = initialization;
			Condition = condition;
			Increment = increment;
			Body = body;
		}

		private ExpressionNode Initialization { get; set; }
		private ExpressionNode Condition { get; set; }
		private ExpressionNode Increment { get; set; }
		private BlockStatementNode Body { get; set; }
		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitForStatement(this);
		}

		public override IEnumerable<AstNode> GetChildren()
		{
			return [Initialization, Condition, Increment, Body];
		}
	}
	
	public class IfStatementNode : StatementNode
	{
		public IfStatementNode(int line, int column, 
		ExpressionNode condition,
		BlockStatementNode thenBody,
		BlockStatementNode elseBody) 
		: base(line, column, StatementType.IfStatement)
		{
			Condition = condition;
			ThenBody = thenBody;
			ElseBody = elseBody;
		}

		private ExpressionNode Condition { get; set; }
		private BlockStatementNode ThenBody { get; set; }
		private BlockStatementNode ElseBody { get; set; }
		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitIfStatement(this);
		}

		public override IEnumerable<AstNode> GetChildren()
		{
			return [Condition, ThenBody, ElseBody];
		}
	}
	
	public class AssignmentStatementNode : StatementNode
	{
		public AssignmentStatementNode(int line, int column, ExpressionNode left, ExpressionNode right)
		: base(line, column, StatementType.AssignmentStatement)
		{
			Left = left;
			Right = right;
		}
		
		private ExpressionNode Left { get; set;}
		private ExpressionNode Right { get; set;}

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitAssignment(this);
		}

		public override IEnumerable<AstNode> GetChildren()
		{
			return [ Left, Right ];
			
		}
	}
	
	/*--------EXPRESSIONS---------------*/
	public class NumberLiteralNode : AstNode
	{
		public NumberLiteralNode(int line, int column, int value) : base(line, column, NodeType.NumberLiteral)
		{
			Value = value;
		}

		public int Value { get; set;}

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitNumberLiteral(this);
		}

		public override IEnumerable<AstNode> GetChildren()
		{
			return [];
		}
	}
	
	public class StringLiteralNode : AstNode
	{
		public StringLiteralNode(int line, int column, string value) : base(line, column, NodeType.StringLiteral)
		{
			Value = value;
		}

		public string? Value { get; set;}

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitString(this);
		}

		public override IEnumerable<AstNode> GetChildren()
		{
			return [];
		}
	}
	
	public class VariableDeclarationNode : AstNode
	{
		public VariableDeclarationNode(int line, int column, string name, ExpressionNode initializer) 
		: base(line, column, NodeType.VariableDeclaration)
		{
			Name = name;
			Initializer = initializer;
		}

		public string Name { get; set; }
		private ExpressionNode Initializer { get; set; }

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitVariableDeclaration(this);
		}

		public override IEnumerable<AstNode> GetChildren()
		{
			return [Initializer];
		}
	}
	
	//FUNCTION CALL
	public class FunctionCallNode : AstNode
	{
		public FunctionCallNode(int line, int column, string identifier, List<ArgumentNode> arguments) : base(line, column, NodeType.FunctionCall)
		{
			Identifier = identifier;
			Arguments = arguments;
		}

		public string Identifier { get; set; }
		public List<ArgumentNode> Arguments { get; set;}

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitFunctionCall(this);
		}
	}
	
	public class ArgumentNode : AstNode
	{
		public ArgumentNode(int line, int column, ExpressionNode expression) : base(line, column, NodeType.Argument)
		{
			Expression = expression;
		}

		public ExpressionNode Expression { get; set;}

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitArgument(this);
		}
	}
	
	public abstract class ExpressionNode : AstNode
	{
		protected ExpressionNode(int line, int column)
		: base(line, column, NodeType.Expression)
		{	
		}

		public abstract override T Accept<T>(IAstVisitor<T> visitor);

		public override IEnumerable<AstNode> GetChildren()
		{
			return [];
		}
	}
	
	public class BinaryExpressionNode : AstNode
	{
		public BinaryExpressionNode(int line, int column, ExpressionNode left, string @operator, ExpressionNode right) : base(line, column, NodeType.BinaryExpression)
		{
			Left = left;
			Operator = @operator;
			Right = right;
		}

		private ExpressionNode Left { get; set;}
		public string Operator { get; set; }
		private ExpressionNode Right { get; set;}

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitBinaryExpression(this);	
		}
		
		public ExpressionNode GetLeft()
		{
			return Left;
		}
		
		public ExpressionNode GetRight()
		{
			return Right;
		}

		public override IEnumerable<AstNode> GetChildren()
		{
			return [Left, Right];
		}
	}
	
	public class UnaryExpressionNode : AstNode
	{
		public UnaryExpressionNode(int line, int column, ExpressionNode operand, string @operator) : base(line, column, NodeType.UnaryExpression)
		{
			Operand = operand;
			Operator = @operator;
		}

		private ExpressionNode Operand { get; set;}
		public string Operator { get; set; }

		public override T Accept<T>(IAstVisitor<T> visitor)
		{
			return visitor.VisitUnaryExpression(this);
		}

		public override IEnumerable<AstNode> GetChildren()
		{
			return [Operand];
		}
	}
}
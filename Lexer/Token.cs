

namespace Gwent__.Lexer
{
	public enum TokenType
	{
		KEYWORDS,
		IDENTIFIERS,
		BOOLEAN_TRUE,
		BOOLEAN_FALSE,
		//OPERATORS
		PLUS,
		MULTIPLY,
		MINUS,
		DIVIDE,
		EQUALS,
		NOT_EQUALS,
		LESS_THAN,
		GREATER_THAN,
		LESS_THAN_OR_EQUAL,
		GREATER_THAN_OR_EQUAL,
		ASSIGNMENT,
		CONCAT,
		PLUS_ONE,
		MINUS_ONE,
		AND,
		OR,
		LAMBDA,
		//SEPARATORS
		COMMA,
		COLON,
		SEMICOLON,
		//LITERALS
		FLOAT,
		INTEGER,
		STRING,
		//SYMBOLS
		LEFT_PAREN,
		RIGHT_PAREN,
		LBRACKET,
		RBRACKET,
		LBRACE,
		RBRACE,
		//OTHER
		EOF,
		WHITESPACE
		
	}
	//TOKEN
	public sealed class Token(TokenType type, string value, int line, int column)
	{
		public TokenType Type { get; set; } = type;
		public string Value { get; set; } = value;
		public int Line { get; set; } = line;

		public int Column { get; set; } = column;

		
	}
	//MAP FOR KEYWORDS
	public sealed class KeywordsHashmap
	{
		public Dictionary<string, TokenType> Keywords { get; } = new()
		{
			{"effect" , TokenType.KEYWORDS},
			{"card" , TokenType.KEYWORDS},
			{"Name" , TokenType.KEYWORDS},
			{"while" , TokenType.KEYWORDS},
			{"if" , TokenType.KEYWORDS},
			{"for" , TokenType.KEYWORDS},
			{"Action" , TokenType.KEYWORDS},
			{"in" , TokenType.KEYWORDS},
			{"Params" , TokenType.KEYWORDS},
			{"" , TokenType.KEYWORDS},
				
		};
	}
	//MAP FOR EACH SYMBOL
	public sealed class Symbols
	{
		public Dictionary<string, TokenType> SymbolsMap { get; } = new()
		{
			{ "+", TokenType.PLUS },
			{ "-", TokenType.MINUS },
			{ "*", TokenType.MULTIPLY },
			{ "/", TokenType.DIVIDE },
			{ "(", TokenType.LEFT_PAREN },
			{ ")", TokenType.RIGHT_PAREN },
			{ "=", TokenType.ASSIGNMENT },
			{ "<", TokenType.LESS_THAN },
			{ ">", TokenType.GREATER_THAN },
			{ ",", TokenType.COMMA },
			{ ":", TokenType.COLON },
			{ ";", TokenType.SEMICOLON },
			{ "[", TokenType.LBRACKET },
			{ "]", TokenType.RBRACKET },
			{ "{", TokenType.LBRACE },
			{ "}", TokenType.RBRACE },
			{ "@", TokenType.CONCAT },
			//MULTI-CHARACTER SYMBOL
			{ "++", TokenType.PLUS_ONE },
			{ "--", TokenType.MINUS_ONE },
			{ "&&", TokenType.AND },
			{ "||", TokenType.OR },
			{ "==", TokenType.EQUALS },
			{ "!=", TokenType.NOT_EQUALS },
			{ "<=", TokenType.LESS_THAN_OR_EQUAL },
			{ ">=", TokenType.GREATER_THAN_OR_EQUAL },
			{ "=>", TokenType.LAMBDA },
			{ "@@", TokenType.CONCAT },
				
		};
	}
}
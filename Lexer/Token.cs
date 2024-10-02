using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
	public sealed class Token
	{
		public TokenType Type { get; set; }
		public string Value { get; set; }
		public int Line { get; set; }
		public int Column { get; set; }
		//CONSTRUCTOR
		public Token(TokenType type, string value, int line, int column)
		{
			Type = type;
			Value = value;
			Line = line;
			Column = column;
		}
	}
	//MAP FOR KEYWORDS
	public sealed class KeywordsHashmap
	{
		public Dictionary<string, TokenType> Keywords { get; set; }
		public KeywordsHashmap()
		{
			Keywords = new Dictionary<string, TokenType>
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
	}
	//MAP FOR EACH SYMBOL
	public sealed class Symbols
	{
		public Dictionary<string, TokenType> SymbolsMap { get; set; }
		public Symbols()
		{
			SymbolsMap = new Dictionary<string, TokenType>
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
}
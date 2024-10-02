using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gwent__.Lexer
{
    public class Tokenizer
	{
		private readonly KeywordsHashmap _keywordsHashmap;
		private readonly Symbols _symbols;
		
		public Tokenizer()
		{
			_keywordsHashmap = new KeywordsHashmap();
			_symbols = new Symbols();
		}
		
		public List<Token> Tokenize(string input)
		{	
			if(input == null)
			{
				throw new ArgumentNullException(nameof(input), "Input cannot be null");
			}
			
			
			var tokens = new List<Token>();
			int currentIndex = 0;
			int lineNumber = 1;
			int columnNumber = 1;
			
			while(currentIndex < input.Length)
			{
				//SKIP WHITESPACE
				if (char.IsWhiteSpace(input[currentIndex]))
				{
					currentIndex++;
					columnNumber++;
					continue;
				}
				
				if (input[currentIndex] == '\n' || input[currentIndex] == '\r')
				{
					lineNumber++;
					columnNumber = 1;
					currentIndex++;
					continue;
				}
				
				var token = GetToken(input, currentIndex, lineNumber, columnNumber);
				if(token != null)
				{
					tokens.Add(token);
					currentIndex += token.Value.Length;
					columnNumber += token.Value.Length;
				}
				else
				{
					throw new TokenizerException($"Unexpected character at line {lineNumber}, column {columnNumber}", input, lineNumber, columnNumber);
				}
			}
			
			//ADD EOF TOKEN
			tokens.Add(new Token(TokenType.EOF, "", 0, 0));
			
			return tokens;
		}
		
		private Token GetToken(string input, int startIndex, int lineNumber, int columnNumber)
		{
			//CHECK FOR ONE-CHARACTER OR MULTI-CHARACTER SYMBOL
			var symbol = GetSymbol(input, startIndex, lineNumber, columnNumber);
			if(symbol != null)
			{
				return symbol;
			}
			//CHECK FOR KEYWORDS
			var keyword = GetKeyword(input, startIndex, lineNumber, columnNumber);
			if(keyword != null)
			{
				return keyword;
			}
			//CHECK FOR IDENTIFIERS
			var identifier = GetIdentifier(input, startIndex, lineNumber, columnNumber);
			if (identifier != null)
			{
				return identifier;
			}

			//CHECK FOR LITERALS
			var literal = GetLiteral(input, startIndex, lineNumber, columnNumber);
			if (literal != null)
			{
				return literal;
			}

			throw new TokenizerException($"Unexpected character at line: {lineNumber}, column: {columnNumber}", input, lineNumber, columnNumber);
		}
		
		private Token GetSymbol(string input, int startIndex, int lineNumber, int columnNumber)
		{
			int currentIndex = startIndex;
			string symbol = string.Empty;
			
			//CHECK FOR MULTI-CHARACTER SYMBOLS
			if(currentIndex + 1 < input.Length)
			{
				string twoCharSymbol = input.Substring(startIndex, 2);
				if(_symbols.SymbolsMap.ContainsKey(twoCharSymbol))
				{
					symbol = twoCharSymbol;
					currentIndex += 2;
				}
				else
				{
					//CHECK FOR ONE-CHARACTER SYMBOLS
					symbol = input[currentIndex].ToString();
					currentIndex++;
				}
			}
			else
			{
				//IF ONLY ONE CHARACTER LEFT, ITS A ONE-CHARACTER SYMBOL
				symbol = input[currentIndex].ToString();
				currentIndex++;
			}
			
			//CHECK IF THE SYMBOL IS VALID
			if(_symbols.SymbolsMap.TryGetValue(symbol, out TokenType tokenType))
			{
				return new Token(tokenType, symbol, lineNumber, columnNumber);
			}
			return null;
		}
		private Token GetKeyword(string input, int startIndex, int lineNumber, int columnNumber)
		{
			int currentIndex = startIndex;
			string keyword = string.Empty;

			while (currentIndex < input.Length && char.IsLetter(input[currentIndex]))
			{
				keyword += input[currentIndex];
				currentIndex++;
			}

			if (_keywordsHashmap.Keywords.ContainsKey(keyword))
			{
				TokenType tokenType = _keywordsHashmap.Keywords[keyword];
				return new Token(tokenType, keyword, lineNumber, columnNumber);
			}
			return null;
		}

		private Token GetIdentifier(string input, int startIndex, int lineNumber, int columnNumber)
		{
			int currentIndex = startIndex;
			string identifier = string.Empty;
			
			

			if (currentIndex < input.Length && (char.IsLetter(input[currentIndex]) || input[currentIndex] == '_'))
			{
				identifier += input[currentIndex];
				currentIndex++;
				
				while (currentIndex < input.Length && (char.IsLetterOrDigit(input[currentIndex]) || input[currentIndex] == '_'))
				{
					identifier += input[currentIndex];
					currentIndex++;
				}
				return new Token(TokenType.IDENTIFIERS, identifier, lineNumber, columnNumber);
			}
			return null;
		}

		private Token GetLiteral(string input, int startIndex, int lineNumber, int columnNumber)
		{
			int currentIndex = startIndex;
			string literal = string.Empty;
			
			const int StartState = 0;
			const int InStringState = 1;
			const int EscapeSequenceState = 2;
			const int TrueState = 3;
			const int FalseState = 4;
			
			int currentState = StartState;
			
			while (currentIndex < input.Length)
			{
				char currentChar = input[currentIndex];
				
				switch (currentState)
				{
					case StartState:
						if (currentChar == '"')
						{
							currentState = InStringState;
							currentIndex++;
						}
						else if(char.IsDigit(currentChar))
						{
							return GetNumberLiteral(input, currentIndex, lineNumber, columnNumber);
						}
						else if(currentChar == 't')
						{
							currentState = TrueState;
							literal += currentChar;
							currentIndex++;
						}
						else if(currentChar == 'f')
						{
							currentState = FalseState;
							literal += currentChar;
							currentIndex++;
						}
						else
						{
							throw new TokenizerException($"Unexpected character '{currentChar}' at line {lineNumber}, column {columnNumber}", input, lineNumber, columnNumber);
						}
						break;
					case InStringState:
						if(currentChar == '\\')
						{
							currentState = EscapeSequenceState;
							currentIndex++;
						}
						else if(currentChar == '"')
						{
							currentIndex++;
							return new Token(TokenType.STRING, literal, lineNumber, columnNumber);
						}	
						else
						{
							literal += currentChar;
							currentIndex++;
						}
						break;
					case EscapeSequenceState:
						switch (currentChar)
						{
							case 'n': literal += '\n'; break;
							case 't': literal += '\t'; break;
							case '\\': literal += '\\'; break;
							case '"': literal += '"'; break; 
							default:
								throw new TokenizerException($"Invalid escape sequence: \\{currentChar} at line {lineNumber}, column {columnNumber}", input, lineNumber, columnNumber); 
						}
						currentState = InStringState;
						currentIndex++;
						break;
					case TrueState:
						if(currentIndex < input.Length && input[currentIndex] == "true"[literal.Length])
						{
							literal += input[currentIndex++];
						}
						else
						{
							if(literal == "true")
							{
								return new Token(TokenType.BOOLEAN_TRUE, literal, lineNumber, columnNumber);
							}
							currentState = StartState;
							literal = string.Empty;
						}
						break;
					case FalseState:
						if (currentIndex < input.Length && input[currentIndex] == "false"[literal.Length])
						{
							literal += input[currentIndex++]; 
						}
						else
						{
							if (literal == "false")
							{
								return new Token(TokenType.BOOLEAN_FALSE, literal, lineNumber, columnNumber);
							}
							currentState = StartState;
							literal = string.Empty;
						}
						break;
				}
			}
			
			if(currentState == InStringState || currentState == EscapeSequenceState)
			{
				throw new TokenizerException($"Unterminated string at line {lineNumber}, column {columnNumber}", input, lineNumber, columnNumber);
			}
			return null;
		}
		private Token GetNumberLiteral(string input, int startIndex, int lineNumber, int columnNumber)
		{
			int currentIndex = startIndex;
			string literal = string.Empty;

			while (currentIndex < input.Length && char.IsDigit(input[currentIndex]))
			{
				literal += input[currentIndex];
				currentIndex++;
			}

			if (currentIndex < input.Length && input[currentIndex] == '.')
			{
				literal += input[currentIndex];
				currentIndex++;

				while (currentIndex < input.Length && char.IsDigit(input[currentIndex]))
				{
					literal += input[currentIndex];
					currentIndex++;
				}
				return new Token(TokenType.FLOAT, literal, lineNumber, columnNumber);
			}
			else if (literal.Length > 0)
			{
				return new Token(TokenType.INTEGER, literal, lineNumber, columnNumber);
			}
			return null;
		}

		public class TokenizerException : Exception
		{
			public TokenizerException(string message, string input, int lineNumber, int columnNumber) 
			: base(message)
			{
				Input = input;
				LineNumber = lineNumber;
				ColumnNumber = columnNumber;
			}
				
			public string Input { get; set;}
			public int LineNumber { get; set;}
			public int ColumnNumber { get; set;}

			public override string ToString()
			{
				return $"{Message} at line {LineNumber}, column {ColumnNumber} in input: {Input}";			
			}
		}	
	}
}
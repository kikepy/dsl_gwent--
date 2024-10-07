
namespace Gwent__.Lexer
{
	public class Tokenizer
	{
		private readonly KeywordsHashmap _keywordsHashmap = new();
		private readonly Symbols _symbols = new();

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
				while(currentIndex < input.Length && char.IsWhiteSpace(input[currentIndex]))
				{
					if (input[currentIndex] == '\n' || input[currentIndex] == '\r')
					{
						lineNumber++;
						columnNumber = 1;
					}
					else
					{
						columnNumber++;
					}
					currentIndex++;
				}
				
				var token = GetToken(input, currentIndex, lineNumber, columnNumber);
				if (tokens != null) tokens.Add(token);
				currentIndex += token.Value.Length;
				columnNumber += token.Value.Length;
			}
			
			return tokens;
		}
		
		private Token GetToken(string input, int startIndex, int lineNumber, int columnNumber)
		{
			if(startIndex >= input.Length)
			{
				//HANDLE THE END OF THE INPUT FILE
				return new Token(TokenType.EOF, "",lineNumber, columnNumber);
			}
			
			if(char.IsLetter(input[startIndex]))
			{
				return GetKeywordOrIdentifier(input, startIndex, lineNumber, columnNumber);
			}
			else if(char.IsDigit(input[startIndex]))
			{
				return GetNumberLiteral(input, startIndex, lineNumber, columnNumber);
			}
			else if(input[startIndex] == '"')
			{
				return GetStringLiteral(input, startIndex, lineNumber, columnNumber);
			}
			else if(input[startIndex] == '/' && startIndex + 1 < input.Length && input[startIndex + 1] == '/')
			{
				//HANDLE WITH COMMENTS
				while(startIndex < input.Length && input[startIndex] != '\n')
				{
					startIndex++;
				}
				return GetToken(input, startIndex, lineNumber, columnNumber);
			}
			else
			{
				return GetSymbol(input, startIndex, lineNumber, columnNumber);
			}
		}
		
		private Token GetSymbol(string input, int startIndex, int lineNumber, int columnNumber)
		{
			string symbol = input[startIndex].ToString();
			
			if(startIndex + 1 < input.Length)
			{
				string symbolMulti = symbol + input[startIndex + 1];
				
				if(_symbols.SymbolSet.Contains(symbolMulti))
				{
					return new Token(_symbols.SymbolsMap[symbolMulti], symbolMulti, lineNumber, columnNumber);
				}
			}
			
			if(_symbols.SymbolSet.Contains(symbol))
			{
				return new Token(_symbols.SymbolsMap[symbol], symbol, lineNumber, columnNumber);
			}
			else
			{
				return new Token(TokenType.ERROR, symbol, lineNumber, columnNumber);
			}
		}
		private Token GetKeywordOrIdentifier(string input, int startIndex, int lineNumber, int columnNumber)
		{
			int currentIndex = startIndex;
			string keywordOrIdentifier = string.Empty;

			while (currentIndex < input.Length && char.IsLetter(input[currentIndex]))
			{
				keywordOrIdentifier += input[currentIndex];
				currentIndex++;
			}

			if (_keywordsHashmap.Keywords.Contains(keywordOrIdentifier))
			{
				return new Token(TokenType.KEYWORDS, keywordOrIdentifier, lineNumber, columnNumber);
			}
			else
			{
				return new Token(TokenType.IDENTIFIERS, keywordOrIdentifier, lineNumber, columnNumber);
			}
			
		}
		private Token GetStringLiteral(string input, int startIndex, int lineNumber, int columnNumber)
		{
			int currentIndex = startIndex + 1;
			string literal = string.Empty;
			
			while(currentIndex < input.Length && input[currentIndex] != '"')
			{
				if(input[currentIndex] == '\\')
				{
					currentIndex++;
					
					if(currentIndex < input.Length)
					{
						switch (input[currentIndex])
						{
							case 'n':
								literal += '\n';
								break;
							case 't':
								literal += '\t';
								break;
							case '\\':
								literal += '\\';
								break;
							case '"':
								literal += '"';
								break;
							default:
								throw new TokenizerException($"Space character is not valid: \\ {input[currentIndex]}", input, lineNumber, columnNumber);
						}
					}
					else
					{
						throw new TokenizerException($"Unexpected end of string literal", input, lineNumber, columnNumber);
					}
				}
				else
				{
					literal += input[currentIndex];
				}
				
				currentIndex++;
			}
			
			if(currentIndex >= input.Length)
			{
				throw new TokenizerException("String is never closed", input, lineNumber, columnNumber);
			}
			
			return new Token(TokenType.STRING, literal, lineNumber, columnNumber);
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
			else
			{
				return new Token(TokenType.INTEGER, literal, lineNumber, columnNumber);
			}
		}

		private class TokenizerException(string message, string input, int lineNumber, int columnNumber)
			: Exception(message)
		{
			private string Input { get; set;} = input;
			private int LineNumber { get; set;} = lineNumber;
			private int ColumnNumber { get; set;} = columnNumber;

			public override string ToString()
			{
				return $"{Message} at line {LineNumber}, column {ColumnNumber} in input: {Input}";			
			}
		}	
	}
}
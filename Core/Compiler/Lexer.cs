using System;
using System.Text;
using System.IO;
using TYP.Angular.Core.Models.Compiler;
using System.Text.RegularExpressions;
using TYP.Angular.Core.ExtensionMethods;
using TYP.Angular.Core.Algorithms;

namespace TYP.Angular.Core.Compile

    
{
    public class Lexer
    {
  
        int lineNo = 1, columnNo = 1; //initialise line and column numbers to 1
        StreamReader fileReader;
        char currentChar;

        public Lexer(StreamReader fileReader)
        {
            this.fileReader = fileReader;
            this.currentChar= (char)fileReader.Read(); //Get the first character
        }


        TOKEN returnTok(string lexVal, int tok_type, double value)
        {
            var return_tok = new TOKEN();
            return_tok.lexeme = lexVal;
            return_tok.type = tok_type;
            return_tok.value = value;
            return_tok.lineNumber = lineNo;
            return_tok.columnNumber = columnNo - lexVal.Length;
            return return_tok;
        }

        TOKEN returnTok(string lexVal, int tok_type)
        {
            var return_tok = new TOKEN();
            return_tok.lexeme = lexVal;
            return_tok.type = tok_type;
            return_tok.lineNumber = lineNo;
            return_tok.columnNumber = columnNo - lexVal.Length;
            
            return return_tok;
        }

        // Read file line by line -- or look for \n and if found add 1 to line number
        // and reset column number to 0
        /// gettok - Return the next token from standard input.
        public TOKEN getToken()
        {
                char nextChar = (char)fileReader.Peek();

                // Skip any whitespace.
                while (Char.IsWhiteSpace(currentChar))
                {
                    if (currentChar == '\n' || currentChar == '\r')
                    {
                        lineNo++;
                        columnNo = 1;
                    }
                    currentChar = (char)fileReader.Read();
                    columnNo++;
                }

                // identifier: [a-zA-Z_][a-zA-Z_0-9]*
                if (currentChar.IsAlpha() || (currentChar == '_'))
                {
                    string identifierString=""+currentChar; 
                    columnNo++;

                    while ((currentChar = (char)fileReader.Read()).IsAlphaNum() || (currentChar == '_'))
                    {
                        identifierString += currentChar;
                        columnNo++;
                    }

                    //Keywords
                    if (identifierString == "MIN") return returnTok("MIN", (int)TOKEN_TYPE.MIN);
                    if (identifierString == "MAX") return returnTok("MAX", (int)TOKEN_TYPE.MAX);

                    //Else just return the ident
                    return returnTok(identifierString, (int)TOKEN_TYPE.IDENT); 

                }
                //Number:[0-9]*.[0-9]*

                if (currentChar.IsNum() || currentChar == '.')
                {
                    string NumStr = "";
                    while (currentChar.IsNum() || currentChar == '.')
                    {
                        NumStr += currentChar;
                        currentChar = (char)fileReader.Read();
                        columnNo++;
                    }
                    return returnTok(NumStr, (int)TOKEN_TYPE.NUM_LIT, double.Parse(NumStr));
                }
                //Equality operations
                if (currentChar == '=')
                {
                    columnNo++;
                    return returnTok("=", (int)TOKEN_TYPE.EQ);
                }
                if (currentChar == '!') //!= currently not supported possibility to expand
                {
                    nextChar = (char)fileReader.Read();
                    if (nextChar == '=')
                    {
                    throw new SemanticError(returnTok("<", (int)TOKEN_TYPE.LT),
                        "The != operation is currently not supported, please see help and about section");
                     }
                     throw new LexicalError(returnTok(currentChar.ToString(), (int)TOKEN_TYPE.INVALID));
                }
                    if (currentChar == '<')
                    {
                    nextChar = (char)fileReader.Read();
                    if (nextChar == '=')
                    { // LE: <=
                        currentChar = (char)fileReader.Read();
                    columnNo += 2;
                        return returnTok("<=", (int)TOKEN_TYPE.LE);
                    }
                    else
                    {
                        throw new SemanticError(returnTok("<", (int)TOKEN_TYPE.LT),
                            "The < operation is currently not supported, please see help and about section");
                    }
                    }

                if (currentChar == '>')
                {
                    nextChar = (char)fileReader.Read();
                    if (nextChar == '=')
                    { // GE: >=
                        currentChar = (char)fileReader.Read();
                        columnNo += 2;
                        return returnTok(">=", (int)TOKEN_TYPE.GE);
                    }
                    else
                    {
                    throw new SemanticError(returnTok(">", (int)TOKEN_TYPE.GT),
                        "The > operation is currently not supported, please see help and about section");
                    }
                }

                if (currentChar == '+')
                {
                    currentChar = (char)fileReader.Read();
                    columnNo++;
                    return returnTok("+", (int)TOKEN_TYPE.PLUS);
                }

                if (currentChar == '-')
                {
                    currentChar = (char)fileReader.Read();
                    columnNo++;
                    return returnTok("-", (int)TOKEN_TYPE.MINUS);
                }
                if (currentChar == '{')
                {
                    currentChar = (char)fileReader.Read();
                    columnNo++;
                    return returnTok("{", (int)TOKEN_TYPE.LBRA);
                }
                if (currentChar == '}')
                {
                    currentChar = (char)fileReader.Read();
                    columnNo++;
                    return returnTok("}", (int)TOKEN_TYPE.RBRA);
                }
                if (currentChar == ':')
                {
                    currentChar = (char)fileReader.Read();
                    columnNo++;
                    return returnTok(":", (int)TOKEN_TYPE.COLON);
                }

                //Check that eof hasn't been reached
                if (fileReader.EndOfStream) // No next?
                {
                return returnTok("$", (int)TOKEN_TYPE.EOF_TOK);
                }

                columnNo++;

                throw new LexicalError(returnTok(currentChar.ToString(), (int)TOKEN_TYPE.INVALID));
            }
        
    }

  
}
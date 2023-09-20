using System;
using System.Runtime.Serialization;


namespace TYP.Angular.Core.Models.Compiler{

[Serializable]
public class LexicalError : Exception
{    
    public LexicalError(TOKEN token)
    :base("Invalid character detected at: "+token.lineNumber+","+token.columnNumber
         + " Character found: " + token.lexeme){}
}

[Serializable]
public class SyntaxError : Exception
{    
    public SyntaxError(TOKEN token, string expected)
    :base("Syntax error detected at: "+token.lineNumber+","+
    token.columnNumber+" Expected: "+expected+" But found: "+token.lexeme){}
}

[Serializable]
public class SemanticError : Exception
{
    public SemanticError(TOKEN token, string message)
    :base(string.Format("Semantic error detected at " + token.lineNumber+ ","+
    token.columnNumber+ " Error Message: "+message)){}

    public SemanticError(string message)
    : base(string.Format("Semantic error detected. Error Message: " + message))
        { }
    }
}


[Serializable]
public class MathematicalError : Exception
{
    public MathematicalError(string message)
    : base(string.Format("Mathematical error detected while running the algorithm "+ " Error Message: " + message))
    { }
    }

[Serializable]
public class InputError : Exception
{
    public InputError(string message)
    : base(message)
    { }
}

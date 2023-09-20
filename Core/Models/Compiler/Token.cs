using System.Text.RegularExpressions;
using TYP.Angular.Core.Contracts;
namespace TYP.Angular.Core.Models.Compiler{

public class TOKEN : ITOKEN {

    public string lexeme{get;set;}

    public double value{get;set;}

    public int type{get; set;}

    public int lineNumber{get; set;}

    public int columnNumber{get; set;}
}

    public enum TOKEN_TYPE{
    //number literal
    NUM_LIT=1,
    // operators
    PLUS = 2,    // addition or unary plus
    MINUS = 3,   // substraction or unary negative
    // comparison operators
    EQ = 7,      // equal
    NE = 8,      // not equal
    LE = 9,      // less than or equal to
    LT = 10, // less than
    GE = 11,      // greater than or equal to
    GT = 12, // greater than
    // parenthesis
    LPAR = 13,
    RPAR = 14,
    RBRA=15,
    LBRA=16,
    MIN=17,
    MAX=18,
    COLON=19,
    IDENT=20,
    // special tokens
    EOF_TOK = 0, // signal end of file
    NL = 100, //new line

    // invalid
    INVALID = -100 // signal invalid token
 }

}




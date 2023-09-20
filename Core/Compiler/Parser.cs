
using TYP.Angular.Core.Models.Compiler;
using System.Collections.Generic;
using System;
using System.Reflection.Emit;

namespace TYP.Angular.Core.Compile
{

    public class Parser
    {

        Lexer Lexer;

        TOKEN currentToken;

        LinkedList<TOKEN> TokenBuffer;

        public Parser(StreamReader fileReader)
        {
            this.Lexer = new Lexer(fileReader);
            this.currentToken = new TOKEN();
            this.TokenBuffer= new LinkedList<TOKEN>();

        }

        TOKEN getNextToken()
        {
            if (TokenBuffer.Count == 0)
            {
                TokenBuffer.AddLast(Lexer.getToken());
            }
            var temp = TokenBuffer.First.Value;
            TokenBuffer.RemoveFirst();
            return currentToken = temp;

        }

        //putBackToken-puts a token back in the queue this allows parts 
        //of the parser to use lookahead
        void putBackToken(TOKEN tok)
        {
            TokenBuffer.AddFirst(tok);
        }

        //match-If token doesnt match expected throw syntax error if it matches then eat the matched token
        void match(int[] expectedTokens, string printedExpectedTokenType)
        {
            if (!Array.Exists(expectedTokens, x => x == currentToken.type))
            {
                throw new SyntaxError(currentToken,
                printedExpectedTokenType);
            }
            getNextToken(); //eat the correctly matched token
        }

        public LinearProgramASTnode parseLinearProgram()
        {
            getNextToken();

            var minOrmax = currentToken;

            match(new int[2] { (int)TOKEN_TYPE.MIN, (int)TOKEN_TYPE.MAX }, "MIN or MAX");

            match(new int[1] { (int)TOKEN_TYPE.COLON }, ":");

            match(new int[1] { (int)TOKEN_TYPE.LBRA }, "{");

            var objectiveFunction = parseObjectiveFunction();

            match(new int[1] { (int)TOKEN_TYPE.RBRA }, "}");

            var constraints = parseConstraints();

            return new LinearProgramASTnode(minOrmax, objectiveFunction, constraints);
        }

        //Objective function should only contain varables as constants do not change the result
        ObjectiveFunctionASTnode parseObjectiveFunction()
        {
            var Variables = parseVariables();

            //Make sure there actually are variables in the list, if not throw an error
            if (!Variables.Any()) throw new SyntaxError(currentToken, "Objective Function");

            return new ObjectiveFunctionASTnode(Variables);

        }

        List<ConstraintASTnode> parseConstraints()
        {

            var constraints = new List<ConstraintASTnode>();

            //Don't consume the EOF token
            while (currentToken.type != (int)TOKEN_TYPE.EOF_TOK)
            {
                constraints.Add(parseConstraint());
               
            }

            //Make sure there actually are constraints, if not throw an error
            if (!constraints.Any()) throw new SyntaxError(currentToken, "Constraints");

            return constraints;

        }

        ConstraintASTnode parseConstraint()
        {
            var Variables = parseVariables();

            //Make sure there actually is a constraint, if not throw an error
            if (!Variables.Any()) throw new SyntaxError(currentToken, "Constraint");

            var operation = currentToken;

            match(new int[2] { (int)TOKEN_TYPE.LE, (int)TOKEN_TYPE.GE }, "Inequality Operation");

            var ConstrainingValue = parseNumber();

            return new ConstraintASTnode(Variables, operation, ConstrainingValue);

        }

        IList<VariableASTnode> parseVariables()
        {
            var Variables = new List<VariableASTnode>();

            while (Array.Exists(new int[4]{(int)TOKEN_TYPE.NUM_LIT,(int)TOKEN_TYPE.IDENT,
                (int)TOKEN_TYPE.MINUS,(int)TOKEN_TYPE.PLUS}, x => x == currentToken.type))
            {
                Variables.Add(parseVariable());

            }
            

            return Variables;
        }


        //Parses variables
        VariableASTnode parseVariable()
        {
            TOKEN? coefficient=null;
            TOKEN? sign=null;
            TOKEN ident;
            switch (currentToken.type)
            {
                default:
                    {

                        throw new SyntaxError(currentToken, "Variables");
                    }

                //For the purposes of easy standard form generation + will be considered as a unary operator
                case (int)TOKEN_TYPE.MINUS or (int)TOKEN_TYPE.PLUS:
                    {

                        sign = currentToken;

                        getNextToken();

                        var operand = currentToken;

                        match(new int[2] { (int)TOKEN_TYPE.NUM_LIT, (int)TOKEN_TYPE.IDENT }, "Operand");

                        if (operand.type == (int)TOKEN_TYPE.NUM_LIT)
                        {
                            coefficient= operand;

                            ident = currentToken;

                            match(new int[1] { (int)TOKEN_TYPE.IDENT }, "Variable Identifier");

                        }
                        else
                        {
                            ident= operand;

                        }
                        return new VariableASTnode(ident, coefficient, sign);
                    } 

                case (int)TOKEN_TYPE.IDENT:
                    {
                        ident=currentToken;

                        getNextToken();

                        return new VariableASTnode(ident, coefficient, sign);
                    }

                case (int)TOKEN_TYPE.NUM_LIT: //works fine
                    {

                        coefficient = currentToken;

                        getNextToken();

                        ident = currentToken;

                        match(new int[1] { (int)TOKEN_TYPE.IDENT }, "Variable Identifier");

                        return new VariableASTnode(ident, coefficient, sign);
                    }

                    
            }
            
        }
        //Parses numbers and if no sign is detetcted defaults to "+"
        NumberASTnode parseNumber()
        {

            var sign = new TOKEN();

            if (currentToken.type == (int)TOKEN_TYPE.PLUS || currentToken.type == (int)TOKEN_TYPE.MINUS)
            {

                sign = currentToken;
                getNextToken();
            }
            else
            {
                sign = new TOKEN { lexeme = "+", type = (int)TOKEN_TYPE.PLUS, lineNumber = 0, columnNumber = 0 }; //"+" is default sign
            }

            var number = currentToken;

            match(new int[1] { (int)TOKEN_TYPE.NUM_LIT }, "Constraining Value");

            return new NumberASTnode(number, sign);
        }

    }
}
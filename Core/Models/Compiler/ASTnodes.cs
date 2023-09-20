
using System.Collections.Generic;
namespace TYP.Angular.Core.Models.Compiler{


public class ASTnode{}

public class ExpressionASTnode:ASTnode{}


public class LinearProgramASTnode:ASTnode{

    public LinearProgramASTnode(TOKEN MinOrMax,
     ObjectiveFunctionASTnode ObjectiveFunction, IList<ConstraintASTnode> Constraints){
        this.MinOrMax=MinOrMax;
        this.Constraints=Constraints;
        this.ObjectiveFunction=ObjectiveFunction;
    }

    public TOKEN MinOrMax {get;set;}

    public ObjectiveFunctionASTnode ObjectiveFunction {get;set;}

    public IList<ConstraintASTnode> Constraints{get;set;}

}


public class ConstraintASTnode : ExpressionASTnode{

    public ConstraintASTnode(IList<VariableASTnode> Variables, TOKEN Operation, NumberASTnode ConstrainingValue){
        this.Variables=Variables;
        this.Operation=Operation;
        this.ConstrainingValue=ConstrainingValue;
    }

    public IList<VariableASTnode> Variables{get; set;}

    public TOKEN Operation {get; set;}

    public NumberASTnode ConstrainingValue{get; set;}

}


public class ObjectiveFunctionASTnode:ASTnode
    {

    public ObjectiveFunctionASTnode(IList<VariableASTnode> ObjectiveFunction){
        this.ObjectiveFunction=ObjectiveFunction;
    }

    public IList<VariableASTnode> ObjectiveFunction{get; set;}
}

//Variable AST node requires three different constructors 
public class VariableASTnode:ExpressionASTnode{

    public VariableASTnode(TOKEN ident, TOKEN? coefficient,TOKEN? sign){
            this.ident=ident;

            this.coefficient = coefficient is null ? new TOKEN
            {
                lexeme = "1",
                type = (int)TOKEN_TYPE.NUM_LIT,
                value = 1,
                columnNumber = 0,
                lineNumber = 0
            } : coefficient;

            this.sign = sign is null ? new TOKEN
            {
                lexeme = "+",
                type = (int)TOKEN_TYPE.PLUS,
                columnNumber = 0,
                lineNumber = 0

            } : sign;

    }

    public VariableASTnode(TOKEN ident, TOKEN SignOrCoefficent,bool isSignOrCoeffient){
        if(isSignOrCoeffient){
            this.ident=ident;
            this.sign=SignOrCoefficent;
            //if no coefficent TOKEN given default to 1
            this.coefficient=new TOKEN{lexeme="1",type= (int)TOKEN_TYPE.NUM_LIT,value=1,columnNumber=0,lineNumber=0};

        }else{
            this.ident=ident;
            this.coefficient=SignOrCoefficent;
            //if no sign TOKEN given default to positive
            this.sign= new TOKEN{lexeme="+",type=(int)TOKEN_TYPE.PLUS,columnNumber=0,lineNumber=0};
        }
    }

    public VariableASTnode(TOKEN ident){
        this.ident=ident;
        this.coefficient=new TOKEN{lexeme="1",type= (int)TOKEN_TYPE.NUM_LIT,value=1,columnNumber=0,lineNumber=0};
        this.sign= new TOKEN{lexeme="+",type=(int)TOKEN_TYPE.PLUS,columnNumber=0,lineNumber=0};
    }

    public TOKEN ident {get; set;}

    public TOKEN coefficient {get; set;}

    public TOKEN sign {get; set;}

}


public class NumberASTnode:ExpressionASTnode{

    public NumberASTnode(TOKEN number, TOKEN sign){
        this.number=number;
        this.sign=sign;
        
    }

    public TOKEN sign {get; set;}

    public TOKEN number {get; set;}

}



}


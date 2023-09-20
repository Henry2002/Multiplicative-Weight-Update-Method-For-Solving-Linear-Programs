using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TYP.Angular.Core.Contracts.LinearProgram;
using TYP.Angular.Core.Contracts.LP;
using TYP.Angular.Core.Models.Compiler;
using TYP.Angular.Core.Models.LP;

//Linear Program generator- performs sematic checks for linear programs and detokenises the input
//This follows the factory design pattern

namespace TYP.Angular.Core.Compiler
{
    public class LPGenerator
    {
        public ILinearProgram generateLinearProgram(LinearProgramASTnode linearProgramASTnode)
        {
            //Perform a semantic check//

            //Gets the set differnce of the variables in the Constraints and the Variables in the OF
            var OFandConstraintsSetDifference = linearProgramASTnode.ObjectiveFunction.ObjectiveFunction.Except(
                linearProgramASTnode.Constraints.SelectMany(i => i.Variables));

            //Checks that there are no variables that can't be matched
            if (OFandConstraintsSetDifference.Count() > 0)
            {
                throw new SemanticError(OFandConstraintsSetDifference.First().ident, "The Variables in the Objective Function " +
                    "do not match the variables in the Constraints");
            }

            IList<Variable> ObjectiveFunction = linearProgramASTnode.ObjectiveFunction.ObjectiveFunction.Select(i => new Variable
            {
                Name = i.ident.lexeme,
                Coefficient = getValue(i),
            }).ToList();

            IList<Constraint> Constraints = new List<Constraint>();

            foreach (var ConstraintAST in linearProgramASTnode.Constraints)
            {
                var constraint = new Constraint
                {
                    Variables = ConstraintAST.Variables.Select(i => new Variable
                    {
                        Name = i.ident.lexeme,
                        Coefficient = getValue(i),
                    }).ToList(),
                    ConstrainingValue = getValue(ConstraintAST.ConstrainingValue),
                    Operation = ConstraintAST.Operation.lexeme,
                };
                var variableNames = constraint.Variables.Select(i => i.Name).ToList();
                if (variableNames.Count() != variableNames.Distinct().Count())
                {
                    throw new SemanticError("Duplicate variable detected in constraint"); //Make sure there are no duplicate variable names in a constraint
                }

                Constraints.Add(constraint);
            }

            return new LinearProgram
            {
                ObjectiveFunction = ObjectiveFunction,
                Constraints = Constraints,
                MinOrMax = linearProgramASTnode.MinOrMax.lexeme,
            };

        }

        public static ILinearProgram generatePackingCoveringLinearProgram(LinearProgramASTnode linearProgramASTnode)
        {
            //Perform semantic checks//
            var ConstraintVariables = linearProgramASTnode.Constraints.SelectMany(i => i.Variables);

            var OFVariableNames = linearProgramASTnode.ObjectiveFunction.ObjectiveFunction.Select(i => i.ident.lexeme);
            var ConstraintVariableNames=ConstraintVariables.Select(j=>j.ident.lexeme);

            //Gets the set differnce of the variables in the Constraints and the Variables in the OF
            var OFandConstraintsSetDifference = OFVariableNames.Except(ConstraintVariableNames);

            //Checks that there are no variables that can't be matched
            if (OFandConstraintsSetDifference.Any())
            {
                throw new SemanticError("The Variables in the Objective Function " +
                    "do not match the variables in the Constraints");
            }


            if (linearProgramASTnode.MinOrMax.lexeme == "MIN")

            {
                var LToperations = linearProgramASTnode.Constraints.Where(i => i.Operation.lexeme == "<=");

                if (LToperations.Any()) throw new SemanticError(LToperations.First().Operation,
                   "<= is not allowed in the constraints of a covering LP");
               
            }
            if (linearProgramASTnode.MinOrMax.lexeme == "MAX")
            {
                var GToperations = linearProgramASTnode.Constraints.Where(i => i.Operation.lexeme == ">=");

                if (GToperations.Any()) throw new SemanticError(GToperations.First().Operation,
                   ">= is not allowed in the constraints of a packing LP");


            }

            var negativeCoefficientsC = ConstraintVariables.Where(j => j.sign.lexeme == "-");

            if (negativeCoefficientsC.Any()) throw new SemanticError(negativeCoefficientsC.First().ident, "Negative coefficients are" +
          " not allowed in the constraints of a Packing-Covering LP ");

            var negativeCoefficientsOF = linearProgramASTnode.ObjectiveFunction.ObjectiveFunction.Where(i => i.sign.lexeme == "-");

            if (negativeCoefficientsOF.Any()) throw new SemanticError(negativeCoefficientsOF.First().ident, "Negative coefficients are" +
            " not allowed in the constraints of a Packing-Covering LP ");

            var negativeConstrainingValues = linearProgramASTnode.Constraints.Where(i => i.ConstrainingValue.sign.lexeme == "-");

            if (negativeConstrainingValues.Any()) throw new SemanticError(negativeConstrainingValues.First().ConstrainingValue.number, "Negative values are" +
                " not allowed as constraining values of a Packing-Covering LP ");


            IList<Variable> ObjectiveFunction = linearProgramASTnode.ObjectiveFunction.ObjectiveFunction.Select(i => new Variable
            {
                Name = i.ident.lexeme,
                Coefficient = getValue(i),
            }).ToList();

            IList<Constraint> Constraints = new List<Constraint>();

            foreach (var ConstraintAST in linearProgramASTnode.Constraints)
            {
                var constraint = new Constraint
                {
                    Variables = ConstraintAST.Variables.Select(i => new Variable
                    {
                        Name = i.ident.lexeme,
                        Coefficient = getValue(i),
                    }).ToList(),
                    ConstrainingValue = getValue(ConstraintAST.ConstrainingValue),
                    Operation = ConstraintAST.Operation.lexeme,
                };
                var variableNames = constraint.Variables.Select(i => i.Name).ToList();

                if (variableNames.Count() != variableNames.Distinct().Count())
                {
                    throw new SemanticError("Duplicate variables detected in constraint at line number "
                        +ConstraintAST.ConstrainingValue.number.lineNumber + ". Please simplify"); //Make sure there are no duplicate variable names in a constraint
                }

                Constraints.Add(constraint);
            }

            return new PackingCoveringLinearProgram
            {
                ObjectiveFunction = ObjectiveFunction,
                Constraints = Constraints,
                MinOrMax = linearProgramASTnode.MinOrMax.lexeme,
            };

        }

        public static ILinearProgram generateLinearProgram(ILPMatrix LPMatrix)
        {
            ILinearProgram LinearProgram= new LinearProgram();

            LinearProgram.MinOrMax = LPMatrix.MinOrMax;

            LinearProgram.ObjectiveFunction = LPMatrix.ObjectiveFunctionVector.ToList().Select(element => (Variable)element).ToList();

            LinearProgram.Constraints = Enumerable.Range(0, LPMatrix.ConstraintsMatrix.Rows)
                .Select(i => new Constraint
                {
                    Variables = LPMatrix.ConstraintsMatrix.GetRow(i).Select(element => (Variable)element).ToList(),
                    ConstrainingValue = (double)LPMatrix.ConstrainingValueVector[i]
                }).ToList();

            return LinearProgram;
        }


        private static double getValue(VariableASTnode variable)
        {
            return variable.sign.lexeme == "+" ? variable.coefficient.value : -variable.coefficient.value;
        }

        private static double getValue(NumberASTnode number)
        {
            return number.sign.lexeme == "+" ? number.number.value : -number.number.value;
        }
    }
}

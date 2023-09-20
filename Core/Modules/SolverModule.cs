using TYP.Angular.Core.Models.BaseResult;
using System.Text;
using Microsoft.VisualBasic;
using TYP.Angular.Core.Compile;
using TYP.Angular.Core.Models.LP;
using TYP.Angular.Core.Models.Simplex;
using TYP.Angular.Core.Models.Web;
using System;
using TYP.Angular.Core.Algorithms;
using TYP.Angular.Core.Contracts.Modules;
using static TYP.Angular.Core.Algorithms.MWUM;
using System.Diagnostics;
using TYP.Angular.Core.Extensions;
using TYP.Angular.Core.Models.MatrixModels;

namespace TYP.Angular.Core.Modules
{
    public class SolverModule : ISolverModule
    {
        public BaseResult<string> SolveLinearProgram(string linearProgram, string selectedAlgorithm, double SelectedEpsilon)
        {

            if (selectedAlgorithm == "Simplex")
            {

                var Iterations = new SimplexTableauIterations();

                return SolveWithSimplex(linearProgram);
            }
            else if (selectedAlgorithm == "MWUM")
            {

                return SolveWithMWUM(linearProgram, SelectedEpsilon);
            } else
            {
                throw new InputError("An algorithm must be selected to solve a Linear Program");
            }

        }

        public BaseResult CompareAlgorithms(string LinearProgram)
        {
            var linearProgramMatrix = LPCompiler.compileAsPCLP(LinearProgram);

            var MWUM = new MWUM(linearProgramMatrix, 0.1);

            MWUM.RunStaticWhackAMole();

            if (linearProgramMatrix.MinOrMax == "MIN") linearProgramMatrix.ConvertToDual();

            var SimplexTableau = new SimplexTableau(linearProgramMatrix); //Initialise a Simplex Tableau

            SimplexTableau.runSimplex();

            return new BaseResult(true);

        }



        public BaseResult<string> SolveWithSimplex(string input)
        {
            var linearProgramMatrix = LPCompiler.compileAsPCLP(input); //Compile the LP as a packing covering LP

            //If the LP is a minimisation LP convert to to its dual form
            if (linearProgramMatrix.MinOrMax == "MIN") linearProgramMatrix.ConvertToDual();

            var SimplexTableau = new SimplexTableau(linearProgramMatrix); //Initialise a Simplex Tableau

            var Results = Time<SimplexTableau>.TimeInMilliseconds(() => { return SimplexTableau.runSimplex(); });

            return new SimplexResult(Results.Result.GetColumn(SimplexTableau.Columns-1), Results.Performance);

        }

        public BaseResult<string> SolveWithMWUM(string input, double epsilon)
        {
            var linearProgramMatrix = LPCompiler.compileAsPCLP(input);

            //If the LP is a minimisation LP convert to to its dual form
            if (linearProgramMatrix.MinOrMax == "MAX") linearProgramMatrix.ConvertToDual();

            var MWUM = new MWUM(linearProgramMatrix, epsilon);

            var Result = Time<(double ObjectiveValue, IList<Element> Variables)>.TimeInMilliseconds(()=> {return MWUM.RunStaticWhackAMole();});

            return new MWUMResult(Result.Result.ObjectiveValue, Result.Result.Variables, Result.Performance);

        }

       
    }
}

using System;
using System.Numerics;
using System.Reflection;
using System.Runtime.Intrinsics;
using System.Transactions;
using TYP.Angular.Core.Contracts.LinearProgram;
using TYP.Angular.Core.Models;
using TYP.Angular.Core.Models.MatrixModels;
using TYP.Angular.Core.Models.MWUM;
using Vector = TYP.Angular.Core.Models.MatrixModels.Vector;

namespace TYP.Angular.Core.Algorithms
{



    /// <summary>
    /// Im pretty sure all the 1s and identities can be swapped out for the OF and the Constraining Values
    /// Weird theoretical bollocks just leaving it like that i reckon
    /// </summary>
    public class MWUM
    {

        private Matrix Constraints;

        private Vector ConstrainingValues;

        private Vector ObjectiveFunction;

        private Matrix ScaledConstraints;

        private double Epsilon;

        private int Lambda;

        private int LambdaUpperBound;

        private double LowerBound;

        private double UpperBound;
        public MWUMIterations Iterations { get; set; } = new MWUMIterations();


        public MWUM(ILPMatrix LPMatrix, double Epsilon)
        {
            if (LPMatrix.MinOrMax == "MAX") LPMatrix.ConvertToDual(); //Algorithm only accepts Covering LPs

            Constraints = LPMatrix.ConstraintsMatrix;  //Extract attributes of LPMatrix
            ConstrainingValues = LPMatrix.ConstrainingValueVector;
            ObjectiveFunction = LPMatrix.ObjectiveFunctionVector;

            this.Epsilon = Epsilon; //Extract epsilon value

            var L = LPMatrix.MinNonZero()?.Value ?? throw new MathematicalError ( "Algorithm cannot be run on a null LP");

            var U = LPMatrix.MaxNonZero()?.Value ?? throw new MathematicalError("Algorithm cannot be run on a null LP"); 

            Lambda = (int)Math.Round(U / L * L);

            LambdaUpperBound = (int)Math.Round(Constraints.Rows * U * U * U / L * L * L);
           
            UpperBound = U * U / L; LowerBound = L * L / U;

            Iterations.LowerBound = LowerBound; Iterations.UpperBound = UpperBound;

            ScaledConstraints = LPMatrix.ScaledConstraints();

        }

   
        public (double ObjectiveValue, IList<Element> Variables) RunStaticWhackAMole()
        {
            var DualCprime = ScaledConstraints.Transpose();

            //Binary Search over all guesses.

            var BinarySearch = new ConditionalBinarySearch((index) =>
            {
                var Guess = Math.Pow(1 + Epsilon, index); //Calculate Guess

                var GuessLambda = (int) Guess * Lambda; //Update Lambda

                var Item = StaticWhackAMole.Run(ScaledConstraints * Guess, Epsilon, 
                    GuessLambda>LambdaUpperBound? LambdaUpperBound: GuessLambda);  //Run Static Whack-A-Mole for Guess

                var isSatisfied = true; //Assume Item satisifed the Constraints

                //Check if it actually did
                for (int i = 0; i < (Item.solvedDual ? ScaledConstraints.Columns : ScaledConstraints.Rows); i++)
                {
                    if (Item.solvedDual)
                    {
                        if (Vector.DotProduct(Item.variables, DualCprime.GetRowAsVector(i)) < 1 + Epsilon) isSatisfied = false;
                    }
                    else
                    {
                        if (Vector.DotProduct(Item.variables, ScaledConstraints.GetRowAsVector(i)) > 1 - Epsilon) isSatisfied = false;
                    }

                }

                //Update the Iterations

                Iterations.Add(Item.SummaryTable, Guess, isSatisfied, Item.solvedDual);

                return (isSatisfied, (Item, Guess)); //Return value to Binary Search
            });

            var result = BinarySearch.SearchForMinimum((int)Math.Log(LowerBound, 1 + Epsilon), (int)Math.Log(UpperBound, 1 + Epsilon));






            var ObjectiveValue = result.MinimumFound.Item2;

            var SolvedDual = result.MinimumFound.Item1.Item2;

            Vector YVariables = result.MinimumFound.Item1.Item1 * ObjectiveValue;

            var XVariables = SolvedDual ? YVariables.Zip(ConstrainingValues, (a, b) => b / a) : YVariables.Zip(ObjectiveFunction, (a, b) => b / a);

            XVariables.DoToAll((ref Element element, int index) => element.Name = SolvedDual ? ConstrainingValues[index].Name : ObjectiveFunction[index].Name);

            IterationsManager.CurrentMWUMIterations = Iterations;

            return (ObjectiveValue, XVariables.ToList());

        }



    }

}